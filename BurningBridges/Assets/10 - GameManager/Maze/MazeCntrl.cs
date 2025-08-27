using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class MazeCntrl : MonoBehaviour
{
    [SerializeField] private GameObject mazeNodePrefab;

    private float size = 30.69f;
    private int width = 5;
    private int height = 5;

    private MazeNode[,] mazeNode;

    private Stack<MazeNode> mazeNodeStack;

    private int pathSize = 0;
    private MazeNode[] mazePath = null;

    private Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        InitializeMaze();

        CreateMaze();

        //StartCoroutine(RenderMaze());
        RenderMaze();
    }

    private void CreateMaze()
    {
        while (mazeNodeStack.Count != 0)
        {
            MazeNode currentNode = mazeNodeStack.Peek();

            List<MazeNode> neighbors = GetAllNeighbors(currentNode);

            if (neighbors.Count == 0)
            {
                mazeNodeStack.Pop();
            }
            else
            {
                MazeNode nextMazeNode = neighbors[RandomNumber(neighbors.Count)];
                nextMazeNode.MarkNodeAsClosed();
                mazeNodeStack.Push(nextMazeNode);
                SetupNodeLink(currentNode, nextMazeNode);

                if (mazeNodeStack.Count > pathSize)
                {
                    mazePath = mazeNodeStack.ToArray();
                    pathSize = mazeNodeStack.Count;
                }
            }
        }
    }

    private void SetupNodeLink(MazeNode currentNode, MazeNode nextMazeNode)
    {
        switch (nextMazeNode.GetDir())
        {
            case MazeNodeDir.NORTH:
                currentNode.NorthNode = nextMazeNode;
                nextMazeNode.SouthNode = currentNode;
                break;
            case MazeNodeDir.SOUTH:
                currentNode.SouthNode = nextMazeNode;
                nextMazeNode.NorthNode = currentNode;
                break;
            case MazeNodeDir.EAST:
                currentNode.EastNode = nextMazeNode;
                nextMazeNode.WestNode = currentNode;
                break;
            case MazeNodeDir.WEST:
                currentNode.WestNode = nextMazeNode;
                nextMazeNode.EastNode = currentNode;
                break;
        }
    }

    private void InitializeMaze()
    {
        mazeNode = new MazeNode[width, height];

        for (int w = 0; w < width; w++)
        {
            for (int h = 0; h < height; h++)
            {
                mazeNode[w, h] = new MazeNode(w, h);
            }
        }

        mazeNodeStack = new Stack<MazeNode>();

        MazeNode startingPoint = GetRandomMazeNode();
        startingPoint.MarkNodeAsClosed();

        mazeNodeStack.Push(startingPoint);
    }

    private List<MazeNode> GetAllNeighbors(MazeNode center)
    {
        List<MazeNode> neighbors = new List<MazeNode>();

        int w = center.Getw();
        int h = center.Geth();

        MazeNode northNode = CheckNeighbor(MazeNodeDir.NORTH, w, h + 1, center);
        MazeNode southNode = CheckNeighbor(MazeNodeDir.SOUTH, w, h - 1, center);
        MazeNode eastNode = CheckNeighbor(MazeNodeDir.EAST, w + 1, h, center);
        MazeNode westNode = CheckNeighbor(MazeNodeDir.WEST, w - 1, h, center);

        if (northNode != null) neighbors.Add(northNode);
        if (southNode != null) neighbors.Add(southNode);
        if (eastNode != null) neighbors.Add(eastNode);
        if (westNode != null) neighbors.Add(westNode);

        return (neighbors);
    }

    private MazeNode CheckNeighbor(MazeNodeDir direction, int w, int h, MazeNode center)
    {
        MazeNode node = null;

        if (IsOnBoard(w, h) && mazeNode[w, h].IsNodeOpen())
        {
            node = mazeNode[w, h];
            node.SetDir(direction);
        }

        return (node);
    }

    private void RenderMaze()
    {
        bool firstnode = true;
        MazeNode startingPoint = null;
        MazeNode endingPoint = null;

        foreach (MazeNode mazeNode in mazePath)
        {
            if (firstnode)
            {
                firstnode = false;
                startingPoint = mazeNode;
            }

            mazeNode.MarkAsPathNode();
            endingPoint = mazeNode;
        }

        startingPoint.MarkAsStartNode();
        endingPoint.MarkAsEndingNode();

        for (int h = 0; h < height; h++)
        {
            for (int w = 0; w < width; w++)
            {
                GameObject go = Instantiate(mazeNodePrefab, gameObject.transform);
                go.transform.SetLocalPositionAndRotation(new Vector3(w * size, 0.0f, h * size), Quaternion.identity);

                if ((w == 0) && (h == 0))
                {
                    go.GetComponent<MazeNodeCntrl>().SetCornerNode(mazeNode[w, h], MazeNodeDir.SOUTH);
                } 
                else if ((w == (width-1)) && (h == (height-1))) {
                    go.GetComponent<MazeNodeCntrl>().SetCornerNode(mazeNode[w, h], MazeNodeDir.NORTH);
                } 
                else if ((w == 0) && (h == (height - 1)))
                {
                    go.GetComponent<MazeNodeCntrl>().SetCornerNode(mazeNode[w, h], MazeNodeDir.WEST);
                } 
                else if ((h == 0) && (w == (width - 1)))
                {
                    go.GetComponent<MazeNodeCntrl>().SetCornerNode(mazeNode[w, h], MazeNodeDir.EAST);
                }
                else if (w == 0)
                {
                    go.GetComponent<MazeNodeCntrl>().SetTNode(mazeNode[w, h], MazeNodeDir.WEST);
                } 
                else if (h == 0)
                {
                    go.GetComponent<MazeNodeCntrl>().SetTNode(mazeNode[w, h], MazeNodeDir.SOUTH);
                } 
                else if (w == (width - 1))
                {
                    go.GetComponent<MazeNodeCntrl>().SetTNode(mazeNode[w, h], MazeNodeDir.EAST);
                } 
                else if (h == (height - 1))
                {
                    go.GetComponent<MazeNodeCntrl>().SetTNode(mazeNode[w, h], MazeNodeDir.NORTH);
                } 
                else
                {
                    go.GetComponent<MazeNodeCntrl>().SetCrossNode(mazeNode[w, h]);
                }

            }
        }

    }

    /**
     * IsOnBoard() - Determines if the w,h coordinate is on the maze 
     * board.  If it is not, a false is returned else true is
     * returned to the caller.
     */
    private bool IsOnBoard(int w, int h)
    {
        return (((w >= 0) && (w < width)) && ((h >= 0) && (h < height)));
    }

    private MazeNode GetRandomMazeNode()
    {
        int w = RandomNumber(width);
        int h = RandomNumber(height);

        return (mazeNode[w, h]);
    }

    private int RandomNumber(int n)
    {
        return (Random.Range(0, n));
    }
}

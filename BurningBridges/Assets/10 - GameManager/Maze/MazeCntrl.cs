using UnityEngine;
using System.Collections.Generic;
using Unity.AI.Navigation;
using System.Collections;

public class MazeCntrl : MonoBehaviour
{
    [SerializeField] private GameObject mazeNodePrefab;
    [SerializeField] private GameObject player;

    private float size = 30.69f;
    private int width = 5;
    private int height = 5;

    private MazeNode[,] mazeNode;

    private Stack<MazeNode> mazeNodeStack;

    private int pathSize = 0;
    private MazeNode[] mazePath = null;

    private Vector3 playerStartPos = Vector3.zero;

    private NavMeshSurface navMeshSurface = null;

    private void Awake()
    {
        navMeshSurface = GetComponent<NavMeshSurface>();
    }

    void Start()
    {
        InitializeMaze();

        CreateMaze();

        SetNodeType();

        RenderMaze();

        PositionPlayer();

        navMeshSurface.BuildNavMesh();
    }

    private void PositionPlayer()
    {
        player.SetActive(true);
        player.transform.position = playerStartPos;
    }

    /**
     * CreateMaze() - 
     */
    private void CreateMaze()
    {
        while (mazeNodeStack.Count != 0)
        {
            MazeNode currentNode = mazeNodeStack.Peek();

            List<MazeNode> neighbors = GetAllNeighbors(currentNode);

            if (neighbors.Count == 0)
            {
                mazeNodeStack.Peek().PrintIt("Pop: ");
                mazeNodeStack.Pop();
            }
            else
            {
                MazeNode nextMazeNode = neighbors[RandomNumber(neighbors.Count)];
                nextMazeNode.MarkNodeAsClosed();
                mazeNodeStack.Push(nextMazeNode);
                nextMazeNode.PrintIt("Push: ");
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
        mazeNodeStack.Push(GetRandomMazeNode());
        mazeNodeStack.Peek().MarkNodeAsClosed();
        mazeNodeStack.Peek().PrintIt("Start");
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
        for (int h = 0; h < height; h++)
        {
            for (int w = 0; w < width; w++)
            {
                GameObject mazeNose = Instantiate(mazeNodePrefab, gameObject.transform);
                mazeNose.transform.SetLocalPositionAndRotation(new Vector3(w * size, 0.0f, h * size), Quaternion.identity);

                RenderNode(mazeNose, w, h);
            }
        }

    }

    private void SetNodeType()
    {
        for (int i = 0; i < mazePath.Length; i++)
        {
            if (i == 0)
            {
                mazePath[i].MarkAsStartNode();

                int w = mazePath[i].Getw();
                int h = mazePath[i].Geth();
                playerStartPos = new Vector3(w * size, 0.0f, h * size);
            }

            if (i == (mazePath.Length-1))
            {
                mazePath[i].MarkAsEndingNode();
            }
        }
    }

   

    private void OnDrawGizmos()
    {
        if (pathSize > 0)
        {
            Gizmos.color = Color.red;

            for (int i = 1; i < pathSize; i++)
            {
                Vector3 posStart = new(mazePath[i - 1].Getw() * size, 0.0f, mazePath[i - 1].Geth() * size);
                Vector3 posEnd = new(mazePath[i].Getw() * size, 0.0f, mazePath[i].Geth() * size);
                Gizmos.DrawLine(posStart, posEnd);
            }
        }
    }

    /**
     * RenderNode() - 
     */
    private void RenderNode(GameObject go, int w, int h)
    {
        if ((w == 0) && (h == 0))
        {
            go.GetComponent<MazeNodeCntrl>().SetCornerNode(player.transform, mazeNode[w, h], MazeNodeDir.SOUTH);
        }
        else if ((w == (width - 1)) && (h == (height - 1)))
        {
            go.GetComponent<MazeNodeCntrl>().SetCornerNode(player.transform, mazeNode[w, h], MazeNodeDir.NORTH);
        }
        else if ((w == 0) && (h == (height - 1)))
        {
            go.GetComponent<MazeNodeCntrl>().SetCornerNode(player.transform, mazeNode[w, h], MazeNodeDir.WEST);
        }
        else if ((h == 0) && (w == (width - 1)))
        {
            go.GetComponent<MazeNodeCntrl>().SetCornerNode(player.transform, mazeNode[w, h], MazeNodeDir.EAST);
        }
        else if (w == 0)
        {
            go.GetComponent<MazeNodeCntrl>().SetTNode(player.transform, mazeNode[w, h], MazeNodeDir.WEST);
        }
        else if (h == 0)
        {
            go.GetComponent<MazeNodeCntrl>().SetTNode(player.transform, mazeNode[w, h], MazeNodeDir.SOUTH);
        }
        else if (w == (width - 1))
        {
            go.GetComponent<MazeNodeCntrl>().SetTNode(player.transform, mazeNode[w, h], MazeNodeDir.EAST);
        }
        else if (h == (height - 1))
        {
            go.GetComponent<MazeNodeCntrl>().SetTNode(player.transform, mazeNode[w, h], MazeNodeDir.NORTH);
        }
        else
        {
            go.GetComponent<MazeNodeCntrl>().SetCrossNode(player.transform, mazeNode[w, h]);
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

    /**
     * GetRandomMazeNode() - Return a random node from the maze.
     */
    private MazeNode GetRandomMazeNode()
    {
        int w = RandomNumber(width);
        int h = RandomNumber(height);

        return (mazeNode[w, h]);
    }

    /**
     * RandomNumber() - Returns a random number between 0 and n - 1.
     */
    private int RandomNumber(int n)
    {
        return (Random.Range(0, n));
    }
}

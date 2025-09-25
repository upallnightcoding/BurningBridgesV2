using UnityEngine;
using System.Collections.Generic;
using Unity.AI.Navigation;
using System.Collections;

public class MazeCntrl : MonoBehaviour
{
    [SerializeField] private GameData gameData;
    [SerializeField] private GameObject mazeNodePrefab;
    [SerializeField] private GameObject player;
    [SerializeField] private Transform arrowLayerParent;
    [SerializeField] private GameObject arrowPrefab;

    private float islandDistance = 0.0f;
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
        
    }

    public int GetWidth() => width;
    public int GetHeight() => height;

    public void StartNewGame(Transform parent)
    {
        InitializeMaze();

        CreateMaze();

        SetNodeType();

        RenderMaze(parent);

        PositionPlayer();

        BuildDirectionArrows(arrowLayerParent);

        navMeshSurface.BuildNavMesh();
    }

    private void ChangeGameLevel(int level)
    {
        width = level;
        height = level;
    }

    private void PositionPlayer()
    {
        player.SetActive(true);
        player.transform.position = playerStartPos;

        EventManager.Instance.InvokeOnPlayerPosition();
    }

    public void ResetPlayer(BridgeCntrl bridgeCntrl)
    {
        bridgeCntrl.DestroyBridge();

        //StartCoroutine(BuildNavMesh());
    }

    private IEnumerator BuildNavMesh()
    {
        yield return new WaitForEndOfFrame();
        navMeshSurface.RemoveData();
        navMeshSurface.BuildNavMesh();

        yield return new WaitForEndOfFrame();
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

        islandDistance = gameData.islandDistance;

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

    /**
     * RenderMaze() - 
     */
    private void RenderMaze(Transform parent)
    {
        for (int h = 0; h < height; h++)
        {
            for (int w = 0; w < width; w++)
            {
                GameObject mazeNode = Instantiate(mazeNodePrefab, parent);
                Vector3 position = new Vector3(w * islandDistance, 0.0f, h * islandDistance);
                mazeNode.transform.SetLocalPositionAndRotation(position, Quaternion.identity);

                RenderNode(mazeNode, w, h);
            }
        }

    }

    /**
     * SetNodeType() - 
     */
    private void SetNodeType()
    {
        MazeNode[] suffleMazeNodes = new MazeNode[width * height];
        int shuffle = 0, n = width * height;

        mazePath[0].MarkAsStartNode();
        playerStartPos = mazePath[0].GetPosition(islandDistance);
        mazePath[mazePath.Length - 1].MarkAsEndingNode();

        for (int w = 0; w < width; w++)
        {
            for (int h = 0; h < height; h++)
            {
                if (!mazeNode[w, h].isStartOrEnd())
                {
                    suffleMazeNodes[shuffle++] = mazeNode[w, h];
                }
            }
        }

        for (int i = 0; i < shuffle; i++)
        {
            int t1 = RandomNumber(shuffle);
            int t2 = RandomNumber(shuffle);

            MazeNode tempNode = suffleMazeNodes[t1];
            suffleMazeNodes[t1] = suffleMazeNodes[t2];
            suffleMazeNodes[t2] = tempNode;
        }

        int mark = 0;

        int nHealth = (int) (n * gameData.healthPercent / 100.0f);
        for (int i = 0; i < nHealth; i++)
        {
            suffleMazeNodes[mark++].MarkAsHealth();
        }

        int nEnemies = (int)(n * gameData.enemiesPercent / 100.0f);
        for (int i = 0; i < nEnemies; i++)
        {
            suffleMazeNodes[mark++].MarkAsEnemy(); 
        }
    }

    private void OnDrawGizmos()
    {
        if (pathSize > 0)
        {
            Gizmos.color = Color.red;

            for (int i = 1; i < pathSize; i++)
            {
                Vector3 posStart = new(mazePath[i - 1].Getw() * islandDistance, 0.0f, mazePath[i - 1].Geth() * islandDistance);
                Vector3 posEnd = new(mazePath[i].Getw() * islandDistance, 0.0f, mazePath[i].Geth() * islandDistance);
                Gizmos.DrawLine(posStart, posEnd);
            }
        }
    }

    private void BuildDirectionArrows(Transform parent)
    {
        if (pathSize > 0)
        {
            for (int i = 1; i < pathSize; i++)
            {
                //Vector3 posStart = new(mazePath[i - 1].Getw() * islandDistance, 0.0f, mazePath[i - 1].Geth() * islandDistance);
                //Vector3 posEnd = new(mazePath[i].Getw() * islandDistance, 0.0f, mazePath[i].Geth() * islandDistance);
                //Gizmos.DrawLine(posStart, posEnd);

                MazeNodeDir direction = mazePath[i - 1].GetDirection(mazePath[i]);

                GameObject mazeNode = Instantiate(arrowPrefab, arrowLayerParent);
                mazeNode.transform.localPosition = new Vector3(mazePath[i - 1].Getw() * islandDistance, 0.0f, mazePath[i - 1].Geth() * islandDistance);

                switch (direction)
                {
                    case MazeNodeDir.NORTH:
                        break;
                    case MazeNodeDir.SOUTH:
                        mazeNode.transform.localRotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
                        break;
                    case MazeNodeDir.EAST:
                        mazeNode.transform.localRotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
                        break;
                    case MazeNodeDir.WEST:
                        mazeNode.transform.localRotation = Quaternion.Euler(0.0f, 270.0f, 0.0f);
                        break;
                }
            }
        }
    }

    /**
     * RenderNode() - 
     */
    private void RenderNode(GameObject mazeNode, int w, int h)
    {
        PlayerCntrl playerCntrl = player.GetComponent<PlayerCntrl>();

        if ((w == 0) && (h == 0))
        {
            mazeNode.GetComponent<MazeNodeCntrl>().SetCornerNode(playerCntrl, this.mazeNode[w, h], MazeNodeDir.SOUTH);
        }
        else if ((w == (width - 1)) && (h == (height - 1)))
        {
            mazeNode.GetComponent<MazeNodeCntrl>().SetCornerNode(playerCntrl, this.mazeNode[w, h], MazeNodeDir.NORTH);
        }
        else if ((w == 0) && (h == (height - 1)))
        {
            mazeNode.GetComponent<MazeNodeCntrl>().SetCornerNode(playerCntrl, this.mazeNode[w, h], MazeNodeDir.WEST);
        }
        else if ((h == 0) && (w == (width - 1)))
        {
            mazeNode.GetComponent<MazeNodeCntrl>().SetCornerNode(playerCntrl, this.mazeNode[w, h], MazeNodeDir.EAST);
        }
        else if (w == 0)
        {
            mazeNode.GetComponent<MazeNodeCntrl>().SetTNode(playerCntrl, this.mazeNode[w, h], MazeNodeDir.WEST);
        }
        else if (h == 0)
        {
            mazeNode.GetComponent<MazeNodeCntrl>().SetTNode(playerCntrl, this.mazeNode[w, h], MazeNodeDir.SOUTH);
        }
        else if (w == (width - 1))
        {
            mazeNode.GetComponent<MazeNodeCntrl>().SetTNode(playerCntrl, this.mazeNode[w, h], MazeNodeDir.EAST);
        }
        else if (h == (height - 1))
        {
            mazeNode.GetComponent<MazeNodeCntrl>().SetTNode(playerCntrl, this.mazeNode[w, h], MazeNodeDir.NORTH);
        }
        else
        {
            mazeNode.GetComponent<MazeNodeCntrl>().SetCrossNode(playerCntrl, this.mazeNode[w, h]);
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

    private void OnEnable()
    {
        EventManager.Instance.OnResetPlayer += ResetPlayer;
        EventManager.Instance.OnLevelChange += ChangeGameLevel;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnResetPlayer -= ResetPlayer;
        EventManager.Instance.OnLevelChange -= ChangeGameLevel;
    }


}

using UnityEngine;

public class MazeNode
{
    public MazeNode NorthNode { get; set; } = null;
    public MazeNode SouthNode { get; set; } = null;
    public MazeNode EastNode { get; set; } = null;
    public MazeNode WestNode { get; set; } = null;

    public MazeNodeType Type { get; set; } = MazeNodeType.EMPTY;

    public MazeNodeStatus status = MazeNodeStatus.OPEN;

    private int w = 0;
    private int h = 0;

    private MazeNodeDir direction = MazeNodeDir.NONE;

    public int Getw() => w;
    public int Geth() => h;

    public void SetDir(MazeNodeDir direction) => this.direction = direction;

    public MazeNodeDir GetDir() => direction;

    public MazeNode(int w, int h)
    {
        this.w = w;
        this.h = h;
    }

    public bool IsNodeOpen()
    {
        return (status == MazeNodeStatus.OPEN);
    }

    public void PrintIt(string text)
    {
        //Debug.Log($"{text} => w:{w},h:{h},Type:{Type.ToString()}");
    }

    public void MarkNodeAsClosed()
    {
        status = MazeNodeStatus.CLOSED;
    }

    public void MarkAsStartNode()   => Type = MazeNodeType.STARTING;
    public void MarkAsEndingNode()  => Type = MazeNodeType.ENDING;
    public void MarkAsPathNode()    => Type = MazeNodeType.PATH;
}

public enum MazeNodeType
{
    EMPTY,
    STARTING,
    ENDING,
    PATH
}

public enum MazeNodeStatus
{
    CLOSED,
    OPEN
}

public enum MazeNodeDir
{
    NONE,
    NORTH,
    SOUTH,
    EAST,
    WEST
}

using UnityEngine;

public class MazeNode
{
    public MazeNode NorthNode { get; set; } = null;
    public MazeNode SouthNode { get; set; } = null;
    public MazeNode EastNode { get; set; } = null;
    public MazeNode WestNode { get; set; } = null;

    public MazeNodeType Type { get; set; } = MazeNodeType.PATH;

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

    public void PrintIt(string text)
    {
        Debug.Log($"{text}: {w},{h}");
    }

    public Vector3 GetPosition(float size)
    {
        return (new Vector3(w * size, 0.0f, h * size));
    }

    public bool IsNodeOpen()
    {
        return (status == MazeNodeStatus.OPEN);
    }

    public void MarkNodeAsClosed()
    {
        status = MazeNodeStatus.CLOSED;
    }

    public bool isStartOrEnd()
    {
        return ((Type == MazeNodeType.STARTING) || (Type == MazeNodeType.ENDING));
    }

    public void MarkAsStartNode()   => Type = MazeNodeType.STARTING;
    public void MarkAsEndingNode()  => Type = MazeNodeType.ENDING;
    public void MarkAsPathNode()    => Type = MazeNodeType.PATH;
    public void MarkAsEnemy()       => Type = MazeNodeType.ENEMY;
    public void MarkAsHealth()      => Type = MazeNodeType.HEALTH;
    public void MarkAsTreasure()    => Type = MazeNodeType.TREASURE;
}

public enum MazeNodeType
{
    ENEMY,
    STARTING,
    ENDING,
    PATH,
    HEALTH,
    TREASURE
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

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MazeNodeCntrl : MonoBehaviour
{
    // The prefabs are set and therefore the line is not null
    //-------------------------------------------------------
    [SerializeField] private GameObject northLink;
    [SerializeField] private GameObject southLink;
    [SerializeField] private GameObject eastLink;
    [SerializeField] private GameObject westLink;

    [SerializeField] private GameObject roadCrossNode;
    [SerializeField] private GameObject roadTNode;
    [SerializeField] private GameObject readCornerNode;

    [SerializeField] private GameObject startSign;
    [SerializeField] private GameObject endCastle;

    private MazeNode node = null;

    void Start()
    {

    }

    /**
     * SetupLinks() -
     */
    private void CloseAllPaths(MazeNode node)
    {
        this.node = node;

        if (node.NorthNode == null) northLink.SetActive(false);
        if (node.SouthNode == null) southLink.SetActive(false);
        if (node.EastNode == null) eastLink.SetActive(false);
        if (node.WestNode == null) westLink.SetActive(false);
    }

    private void RenderNodeType()
    {
        GameObject go = null;

        switch (node.Type)
        {
            case MazeNodeType.STARTING:
                go = Instantiate(startSign, transform);
                go.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
                break;
            case MazeNodeType.ENDING:
                go = Instantiate(endCastle, transform);
                go.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
                break;
            case MazeNodeType.PATH:
                break;
        }
    }

    public void SetCrossNode(MazeNode node)
    {
        CloseAllPaths(node);

        roadCrossNode.SetActive(true);

        RenderNodeType();
    }

    public void SetTNode(MazeNode node, MazeNodeDir direction)
    {
        CloseAllPaths(node);

        roadTNode.SetActive(true);

        switch(direction)
        {
            case MazeNodeDir.NORTH:
                break;
            case MazeNodeDir.SOUTH:
                roadTNode.transform.Rotate(0.0f, 180.0f, 0.0f);
                break;
            case MazeNodeDir.EAST:
                roadTNode.transform.Rotate(0.0f, 90.0f, 0.0f);
                break;
            case MazeNodeDir.WEST:
                roadTNode.transform.Rotate(0.0f, -90.0f, 0.0f);
                break;
        }

        RenderNodeType();
    }

    public void SetCornerNode(MazeNode node, MazeNodeDir direction)
    {
        CloseAllPaths(node);

        readCornerNode.SetActive(true);

        switch (direction)
        {
            case MazeNodeDir.NORTH:
                break;
            case MazeNodeDir.SOUTH:
                readCornerNode.transform.Rotate(0.0f, 180.0f, 0.0f);
                break;
            case MazeNodeDir.EAST:
                readCornerNode.transform.Rotate(0.0f, 90.0f, 0.0f);
                break;
            case MazeNodeDir.WEST:
                readCornerNode.transform.Rotate(0.0f, -90.0f, 0.0f);
                break;
        }

        RenderNodeType();
    }
}

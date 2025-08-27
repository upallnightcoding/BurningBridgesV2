using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MazeNodeCntrl : MonoBehaviour
{
    [SerializeField] private GameObject northLink;
    [SerializeField] private GameObject southLink;
    [SerializeField] private GameObject eastLink;
    [SerializeField] private GameObject westLink;

    [SerializeField] private GameObject roadCrossNode;
    [SerializeField] private GameObject roadTNode;
    [SerializeField] private GameObject readCornerNode;

    private void SetNorthLink()     => northLink.SetActive(false);
    private void SetSouthLink()     => southLink.SetActive(false);
    private void SetEastLink()      => eastLink.SetActive(false);
    private void SetWestLink()      => westLink.SetActive(false);

    void Start()
    {

    }

    private void SetupLinks(MazeNode node)
    {
        if (node.NorthNode == null) SetNorthLink();
        if (node.SouthNode == null) SetSouthLink();
        if (node.EastNode == null) SetEastLink();
        if (node.WestNode == null) SetWestLink();
    }

    public void SetCrossNode(MazeNode node)
    {
        SetupLinks(node);

        roadCrossNode.SetActive(true);
    }

    public void SetTNode(MazeNode node, MazeNodeDir direction)
    {
        SetupLinks(node);

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
    }

    public void SetCornerNode(MazeNode node, MazeNodeDir direction)
    {
        SetupLinks(node);

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
    }
}

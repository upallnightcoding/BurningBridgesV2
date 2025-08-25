using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MazeNodeCntrl : MonoBehaviour
{
    //[SerializeField] private GameObject nodeBase;
    [SerializeField] private GameObject northLink;
    [SerializeField] private GameObject southLink;
    [SerializeField] private GameObject eastLink;
    [SerializeField] private GameObject westLink;

    private void SetNorthLink() => northLink.SetActive(false);
    private void SetSouthLink() => southLink.SetActive(false);
    private void SetEastLink() => eastLink.SetActive(false);
    private void SetWestLink() => westLink.SetActive(false);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //northLink.SetActive(false);
        //southLink.SetActive(false);
        //eastLink.SetActive(false);
        //westLink.SetActive(false);
    }

    public void Set(MazeNode node)
    {
        if (node.NorthNode == null) SetNorthLink();
        if (node.SouthNode == null) SetSouthLink();
        if (node.EastNode == null) SetEastLink();
        if (node.WestNode == null) SetWestLink();

        /*switch(node.GetMazeNodeType())
        {
            case MazeNodeType.STARTING:
                nodeBase.GetComponent<Renderer>().material.color = Color.green;
                break;
            case MazeNodeType.ENDING:
                nodeBase.GetComponent<Renderer>().material.color = Color.red;
                break;
            case MazeNodeType.PATH:
                nodeBase.GetComponent<Renderer>().material.color = Color.blue;
                break;
        }*/
    }
}

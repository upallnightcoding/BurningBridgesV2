using System.Collections;
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

    [SerializeField] private GameObject northExp;
    [SerializeField] private GameObject southExp;
    [SerializeField] private GameObject eastExp;
    [SerializeField] private GameObject westExp;

    [SerializeField] private GameObject roadCrossNode;
    [SerializeField] private GameObject roadTNode;
    [SerializeField] private GameObject readCornerNode;

    [SerializeField] private GameObject startSign;
    [SerializeField] private GameObject endCastle;

    [SerializeField] private GameObject[] enemiesPrefab;

    [SerializeField] private GameObject enemyAppearPrefab;
    [SerializeField] private GameObject healthPrefab;
    [SerializeField] private GameObject treasurePrefab;

    private MazeNode node = null;

    private PlayerCntrl playerCntrl = null;

    private bool enemyRendered = false;

    void Start()
    {

    }

    private void Update()
    {
        if((!enemyRendered) && (RandomNumber(2000) == 0) && (node.Type == MazeNodeType.ENEMY))
        {
            StartCoroutine(RenderEnemy());
        }
    }

    private IEnumerator RenderEnemy()
    {
        Vector3 position = new Vector3(0.0f, 0.0f, 0.0f);
        enemyRendered = true;

        GameObject enemyAppear = Instantiate(enemyAppearPrefab, transform);
        enemyAppear.transform.localPosition = position;

        yield return new WaitForSeconds(3.0f);

        int n = enemiesPrefab.Length;
        GameObject enemy = Instantiate(enemiesPrefab[RandomNumber(n)], transform);
        enemy.transform.localPosition = position;
        enemy.GetComponent<EnemyCntrl>().Set(playerCntrl);

        Destroy(enemyAppear);
    }

    /**
     * SetupLinks() -
     */
    private void CloseAllPaths(MazeNode node)
    {
        this.node = node;

        if (node.NorthNode == null) northLink.GetComponent<BridgeCntrl>().SetTrigger();
        if (node.SouthNode == null) southLink.GetComponent<BridgeCntrl>().SetTrigger();
        if (node.EastNode == null) eastLink.GetComponent<BridgeCntrl>().SetTrigger();
        if (node.WestNode == null) westLink.GetComponent<BridgeCntrl>().SetTrigger();
    }

    /**
     * RenderNodeType() - 
     */
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
            case MazeNodeType.HEALTH:
                go = Instantiate(healthPrefab, transform);
                go.transform.localPosition = new Vector3(0.0f, 1.2f, 0.0f);
                go.transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
                break;
        }
    }

    /**
     * SetCrossNode() - 
     */
    public void SetCrossNode(PlayerCntrl playerCntrl, MazeNode node)
    {
        this.playerCntrl = playerCntrl;

        CloseAllPaths(node);

        roadCrossNode.SetActive(true);

        RenderNodeType();
    }

    public void SetTNode(PlayerCntrl playerCntrl, MazeNode node, MazeNodeDir direction)
    {
        this.playerCntrl = playerCntrl;

        CloseAllPaths(node);

        roadTNode.SetActive(true);

        switch(direction)
        {
            case MazeNodeDir.NORTH:
                northLink.SetActive(false);
                break;
            case MazeNodeDir.SOUTH:
                roadTNode.transform.Rotate(0.0f, 180.0f, 0.0f);
                southLink.SetActive(false);
                break;
            case MazeNodeDir.EAST:
                roadTNode.transform.Rotate(0.0f, 90.0f, 0.0f);
                eastLink.SetActive(false);
                break;
            case MazeNodeDir.WEST:
                roadTNode.transform.Rotate(0.0f, -90.0f, 0.0f);
                westLink.SetActive(false);
                break;
        }

        RenderNodeType();
    }

    public void SetCornerNode(PlayerCntrl playerCntrl, MazeNode node, MazeNodeDir direction)
    {
        this.playerCntrl = playerCntrl;

        CloseAllPaths(node);

        readCornerNode.SetActive(true);

        switch (direction)
        {
            case MazeNodeDir.NORTH:
                northLink.SetActive(false);
                eastLink.SetActive(false);
                break;
            case MazeNodeDir.SOUTH:
                readCornerNode.transform.Rotate(0.0f, 180.0f, 0.0f);
                southLink.SetActive(false);
                westLink.SetActive(false);
                break;
            case MazeNodeDir.EAST:
                readCornerNode.transform.Rotate(0.0f, 90.0f, 0.0f);
                southLink.SetActive(false);
                eastLink.SetActive(false);
                break;
            case MazeNodeDir.WEST:
                readCornerNode.transform.Rotate(0.0f, -90.0f, 0.0f);
                northLink.SetActive(false);
                westLink.SetActive(false);
                break;
        }

        RenderNodeType();
    }

    /**
    * RandomNumber() - Returns a random number between 0 and n - 1.
    */
    private int RandomNumber(int n)
    {
        return (Random.Range(0, n));
    }
}

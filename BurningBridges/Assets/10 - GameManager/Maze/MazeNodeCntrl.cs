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

    [SerializeField] private GameObject roadCrossNode;
    [SerializeField] private GameObject roadTNode;
    [SerializeField] private GameObject readCornerNode;

    [SerializeField] private GameObject startSign;
    [SerializeField] private GameObject endCastle;

    [SerializeField] private GameObject[] enemiesPrefab;

    [SerializeField] private GameObject enemyAppearPrefab;

    private MazeNode node = null;

    private PlayerCntrl playerCntrl = null;

    private bool enemyRendered = false;

    void Start()
    {

    }

    private void Update()
    {
        if((!enemyRendered) && (RandomNumber(1000) == 0) && (node.Type == MazeNodeType.ENEMY))
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

    public void SetCornerNode(PlayerCntrl playerCntrl, MazeNode node, MazeNodeDir direction)
    {
        this.playerCntrl = playerCntrl;

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

    /**
    * RandomNumber() - Returns a random number between 0 and n - 1.
    */
    private int RandomNumber(int n)
    {
        return (Random.Range(0, n));
    }
}

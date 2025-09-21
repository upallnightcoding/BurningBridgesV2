using UnityEngine;

public class EnvironmentCntrl : MonoBehaviour
{
    [SerializeField] private GameData gameData;
    [SerializeField] private GameObject[] waterPrefabs = null;
    [SerializeField] private GameObject[] clusterPrefabs = null;
    [SerializeField] private GameObject skullPrefab = null;
    [SerializeField] private GameObject planes; 

    // Embillsihment Object
    private Embellish decorate = null;

    // Environement space size
    private float gameDecorateSpace = 0.0f;

    // Number of attack skulls
    private int nSkulls = 0;

    // Number of water objects
    private int nWaterObjects = 0;

    // Number of water cluster objects
    private int nWaterClusters = 0;

    public void Start()
    {
        UpdateGameDecorateSpace(gameData.defaultGameLevel);
    }

    /**
     * Create()- 
     */
    public void Create()
    {
        planes.SetActive(true);

        decorate = new Embellish();

        decorate.
          Prefab(skullPrefab).
          Parent(transform).
          Volumn(0.0f, gameDecorateSpace, 0.0f, gameDecorateSpace).
          Rotate(0.0f, 0.0f, 0.0f, 360.0f, 0.0f, 0.0f).
          Render(nSkulls);

        decorate.
            PrefabList(waterPrefabs).
            Parent(transform).
            Volumn(0.0f, gameDecorateSpace, 0.0f, gameDecorateSpace).
            Rotate(0.0f, 0.0f, 0.0f, 360.0f, 0.0f, 0.0f).
            Scale(2.25f, 4.0f, 1.0f, 1.0f, 2.25f, 4.0f).
            Render(nWaterObjects);

        decorate.
            PrefabList(clusterPrefabs).
            Parent(transform).
            Volumn(0.0f, gameDecorateSpace, 0.0f, gameDecorateSpace).
            Rotate(0.0f, 0.0f, 0.0f, 360.0f, 0.0f, 0.0f).
            Scale(2.25f, 4.0f, 1.0f, 1.0f, 2.25f, 4.0f).
            Cluster(nWaterClusters, 3);

        planes.SetActive(false);
    }

    /**
     * UpdateGameDecorateSpace() - Sets the environment space depending on
     * the current game level.  Also sets the limits to the number of 
     * skulls, objects and object clusters that will be rendered in the
     * water.
     */
    public void UpdateGameDecorateSpace(int level)
    {
        gameDecorateSpace = gameData.islandDistance * (level - 1);

        int levelSquared = level * level;

        nSkulls = (int)((300.0f * (levelSquared)) / 25.0f);
        nWaterObjects = (int)((1000.0f * (levelSquared)) / 25.0f);
        nWaterClusters = (int)((100.0f * (levelSquared)) / 25.0f);
    }

    private void OnDisable()
    {
        EventManager.Instance.OnLevelChange -= UpdateGameDecorateSpace;
    }

    private void OnEnable()
    {
        EventManager.Instance.OnLevelChange += UpdateGameDecorateSpace;
    }
}
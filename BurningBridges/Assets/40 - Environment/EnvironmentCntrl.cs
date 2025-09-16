using UnityEngine;

public class EnvironmentCntrl : MonoBehaviour
{
    [SerializeField] private GameObject[] waterPrefabs = null;
    [SerializeField] private GameObject[] clusterPrefabs = null;
    [SerializeField] private GameObject skullPrefab = null;
    [SerializeField] private GameObject planes; 

    private Embellish decorate = null;

    public void Create()
    {
        planes.SetActive(true);

        decorate = new Embellish();

        decorate.
          Prefab(skullPrefab).
          Parent(transform).
          Volumn(0.0f, 122.0f, 0.0f, 122.0f).
          Rotate(0.0f, 0.0f, 0.0f, 360.0f, 0.0f, 0.0f).
          Render(300);

        decorate.
            PrefabList(waterPrefabs).
            Parent(transform).
            Volumn(0.0f, 122.0f, 0.0f, 122.0f).
            Rotate(0.0f, 0.0f, 0.0f, 360.0f, 0.0f, 0.0f).
            Scale(2.25f, 4.0f, 1.0f, 1.0f, 2.25f, 4.0f).
            Render(1000);

        decorate.
            PrefabList(clusterPrefabs).
            Parent(transform).
            Volumn(0.0f, 122.0f, 0.0f, 122.0f).
            Rotate(0.0f, 0.0f, 0.0f, 360.0f, 0.0f, 0.0f).
            Scale(2.25f, 4.0f, 1.0f, 1.0f, 2.25f, 4.0f).
            Cluster(100, 3);

        planes.SetActive(false);
    }

    
}

using UnityEngine;

public class EnvironmentCntrl : MonoBehaviour
{

    [SerializeField] private GameObject[] prefabList = null;
    [SerializeField] private GameObject planes;

    private Embellish decorate = null;

    public void Create()
    {
        planes.SetActive(true);

        decorate = new Embellish();

        decorate.
            PrefabList(prefabList).
            Parent(transform).
            Volumn(0.0f, 122.0f, 0.0f, 122.0f).
            Rotate(0.0f, 0.0f, 0.0f, 360.0f, 0.0f, 0.0f).
            Render(800);

        planes.SetActive(false);
    }

    
}

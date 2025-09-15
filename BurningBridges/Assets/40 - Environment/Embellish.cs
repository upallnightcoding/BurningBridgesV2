using UnityEngine;

public class Embellish
{
    private GameObject[] prefabList = null;

    private Transform parent = null;

    private int nPrefabList = 0;

    private XYZMinMax xyzRotate = null;
    private XYZMinMax xyzScale = null;

    private float xMin;
    private float xMax;
    private float zMin;
    private float zMax;

    public Embellish()
    {

    }

    public Embellish PrefabList(GameObject[] prefabList)
    {
        this.prefabList = prefabList;
        this.nPrefabList = prefabList.Length;

        return (this);
    }

    public Embellish Prefab(GameObject prefab)
    {
        this.prefabList = new GameObject[] { prefab };
        this.nPrefabList = 1;

        return (this);
    }

    public Embellish Parent(Transform parent)
    {
        this.parent = parent;

        return (this);
    }

    public Embellish Volumn(float xMin, float xMax, float zMin, float zMax)
    {
        this.xMin = xMin;
        this.xMax = xMax;
        this.zMin = zMin;
        this.zMax = zMax;

        return (this);
    }

    public Embellish Rotate(float xMin, float xMax, float yMin, float yMax, float zMin, float zMax)
    {
        xyzRotate = new XYZMinMax(xMin, xMax, yMin, yMax, zMin, zMax);

        return (this);
    }

    public Embellish Scale(float xMin, float xMax, float yMin, float yMax, float zMin, float zMax)
    {
        xyzScale = new XYZMinMax(xMin, xMax, yMin, yMax, zMin, zMax);

        return (this);
    }

    public void Cluster(int n, int nCluster)
    {
        for (int i = 0; i < n; i++)
        {
            float x = Random.Range(xMin, xMax);
            float z = Random.Range(zMin, zMax);

            Vector3 dropPoint = new Vector3(x, 10.0f, z);

            RaycastHit[] hit = Physics.RaycastAll(dropPoint, Vector3.down, 100.0f);

            if ((hit != null) && hit.Length == 1)
            {
                float y = hit[0].collider.gameObject.transform.position.y;
                Vector2 center = new Vector2(x, z);

                for (int j = 0; j < nCluster; j++)
                {
                    GameObject go = Object.Instantiate(prefabList[GetRandom(nPrefabList)], parent);

                    Vector2 position = center + Random.insideUnitCircle * 1.0f;

                    go.transform.position = new Vector3(position.x, y, position.y);

                    if (xyzRotate != null) go.transform.localRotation = Quaternion.Euler(xyzRotate.GetVector());
                    if (xyzScale != null) go.transform.localScale = xyzScale.GetVector();
                }
            }
        }
    }

    public void Render(int n)
    {
        for (int i = 0; i < n; i++)
        {
            float x = Random.Range(xMin, xMax);
            float z = Random.Range(zMin, zMax);
            
            Vector3 dropPoint = new Vector3(x, 10.0f, z);

            RaycastHit[] hit = Physics.RaycastAll(dropPoint, Vector3.down, 100.0f);

            if ((hit != null) && hit.Length == 1)
            {
                GameObject go = Object.Instantiate(prefabList[GetRandom(nPrefabList)], parent);

                float y = hit[0].collider.gameObject.transform.position.y;
                go.transform.position = new Vector3(x, y, z);

                if (xyzRotate != null) go.transform.localRotation = Quaternion.Euler(xyzRotate.GetVector());
                if (xyzScale != null) go.transform.localScale = xyzScale.GetVector();
            } 
        }
    }

    private int GetRandom(int n)
    {
        return (Random.Range(0, n));
    }
}

public class XYZMinMax
{
    private MinMax x = null;
    private MinMax y = null;
    private MinMax z = null;

    public XYZMinMax(float xMin, float xMax, float yMin, float yMax, float zMin, float zMax)
    {
        x = new MinMax(xMin, xMax);
        y = new MinMax(yMin, yMax);
        z = new MinMax(zMin, zMax);
    }

    public Vector3 GetVector()
    {
        return (new Vector3(x.GetRandom(), y.GetRandom(), z.GetRandom()));
    }
}

public class MinMax
{
    private float minValue;
    private float maxValue;

    public MinMax(float minValue, float maxValue)
    {
        this.minValue = minValue;
        this.maxValue = maxValue;
    }

    public float GetRandom()
    {
        return (Random.Range(minValue, maxValue));
    }
}

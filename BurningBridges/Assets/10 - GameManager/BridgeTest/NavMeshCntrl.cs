using UnityEngine;
using Unity.AI.Navigation;

public class NavMeshCntrl : MonoBehaviour
{
    private NavMeshSurface navMeshSurface = null;

    void Start()
    {
        navMeshSurface = GetComponent<NavMeshSurface>();

        navMeshSurface.BuildNavMesh();
    }

    void Update()
    {
        
    }
}

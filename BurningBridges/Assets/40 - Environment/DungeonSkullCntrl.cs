using System.Collections;
using UnityEngine;

public class DungeonSkullCntrl : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject fireballPrefab;

    private bool alreadyFiring = false;

    void Start()
    {
        
    }



    public void FireSkull(Vector3 target)
    {
        if (!alreadyFiring)
        {
            alreadyFiring = true;

            StartCoroutine(StartFireSkull(target));           
        }
    }

    private IEnumerator StartFireSkull(Vector3 target)
    {
        Vector3 direction = target - gameObject.transform.position;
        Quaternion rotateTarget = Quaternion.LookRotation(direction);
        transform.rotation = rotateTarget;

        yield return null;

        GameObject go = Instantiate(fireballPrefab);
        Vector3 angles = transform.rotation.eulerAngles;
        go.transform.localRotation = Quaternion.Euler(-45.0f, angles.y, 0.0f);
        go.transform.position = firePoint.position;
        Destroy(go, 4.0f);

        alreadyFiring = false;
    }
}

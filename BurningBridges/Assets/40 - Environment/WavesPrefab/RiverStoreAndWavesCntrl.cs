using UnityEngine;

public class RiverStoreAndWavesCntrl : MonoBehaviour
{
    [SerializeField] private GameObject wavesOnOff;

    void Start()
    {
        wavesOnOff.SetActive(0 == Random.Range(0, 2));
    }
}

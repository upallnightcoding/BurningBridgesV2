using UnityEngine;

public class TurningBoxCntrl : MonoBehaviour
{
    [SerializeField] private MazeNodeCntrl mazeNodeCntrl;

    private void OnTriggerEnter(Collider other)
    {
        EventManager.Instance.InvokeOnEnterTurningBox();
    }

    private void OnTriggerExit(Collider other)
    {
        EventManager.Instance.InvokeOnExitingTurningBox();
    }
}

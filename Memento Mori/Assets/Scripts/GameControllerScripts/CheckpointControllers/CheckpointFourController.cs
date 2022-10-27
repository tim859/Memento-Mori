using UnityEngine;

public class CheckpointFourController : MonoBehaviour
{
    [SerializeField] GameController gameController;
    bool checkpointPassed = false;
    private void OnTriggerEnter(Collider other)
    {
        if (!checkpointPassed)
        {
            if (other.CompareTag("Player"))
            {
                gameController.CheckpointFour();
                checkpointPassed = true;
            }
        }
    }
}

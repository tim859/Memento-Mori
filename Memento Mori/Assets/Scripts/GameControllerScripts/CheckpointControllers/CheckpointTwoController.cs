using UnityEngine;

public class CheckpointTwoController : MonoBehaviour
{
    [SerializeField] GameController gameController;
    bool checkpointPassed = false;
    private void OnTriggerEnter(Collider other)
    {
        if (!checkpointPassed)
        {
            if (other.CompareTag("Player"))
            {
                gameController.CheckpointTwo();
                checkpointPassed = true;
            }
        }
    }
}
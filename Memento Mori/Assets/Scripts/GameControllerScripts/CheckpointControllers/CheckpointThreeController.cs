using UnityEngine;

public class CheckpointThreeController : MonoBehaviour
{
    [SerializeField] GameController gameController;
    bool checkpointPassed = false;
    private void OnTriggerEnter(Collider other)
    {
        if (!checkpointPassed)
        {
            if (other.CompareTag("Player"))
            {
                gameController.CheckpointThree();
                checkpointPassed = true;
            }
        }
    }
}

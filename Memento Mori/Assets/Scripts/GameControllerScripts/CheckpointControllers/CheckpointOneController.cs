using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointOneController : MonoBehaviour
{
    [SerializeField] GameController gameController;
    bool checkpointPassed = false;
    private void OnTriggerEnter(Collider other)
    {
        if (!checkpointPassed)
        {
            if (other.CompareTag("Player"))
            {
                gameController.CheckpointOne();
                checkpointPassed = true;
            }
        }
    }
}

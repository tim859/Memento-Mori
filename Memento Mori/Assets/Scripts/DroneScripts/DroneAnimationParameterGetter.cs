using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneAnimationParameterGetter : MonoBehaviour
{
    [SerializeField] DroneController droneController;
    [SerializeField] Animator droneAnimationController;

    void Update()
    {
        droneAnimationController.SetBool("walking", droneController.GetWalking());
        droneAnimationController.SetBool("chasing", droneController.GetChasing());
        droneAnimationController.SetBool("looking_around", droneController.GetLookingAround());
    }
}
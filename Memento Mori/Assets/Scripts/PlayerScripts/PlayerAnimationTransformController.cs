using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 class PlayerAnimationTransformController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] InventoryController inventoryController;
    [SerializeField] GameObject playerModel;
    [SerializeField] GameObject playerShotgun;

    // vector positions and rotations that will be used to change the local position and rotation of the player model when certain animations are played
    [SerializeField] Vector3 sprintingPosition;
    [SerializeField] Vector3 sprintingRotation;

    [SerializeField] Vector3 holdingShotgunPosition;
    [SerializeField] Vector3 holdingShotgunRotation;

    [SerializeField] Vector3 sprintingWithShotgunPlayerPosition;
    [SerializeField] Vector3 sprintingWithShotgunPlayerRotation;

    [SerializeField] Vector3 sprintingWithShotgunShotgunPosition;
    [SerializeField] Vector3 sprintingWithShotgunShotgunRotation;

    [SerializeField] Vector3 holdingPhonePosition;
    [SerializeField] Vector3 holdingPhoneRotation;

    [SerializeField] Vector3 holdingLunchBoxPosition;
    [SerializeField] Vector3 holdingLunchBoxRotation;

    Vector3 originalPlayerPosition;
    Vector3 originalPlayerRotation;
    Vector3 originalShotgunPosition;
    Vector3 originalShotgunRotation;

    void Start()
    {
        originalPlayerPosition = playerModel.transform.localPosition;
        originalPlayerRotation = playerModel.transform.localEulerAngles;
        originalShotgunPosition = playerShotgun.transform.localPosition;
        originalShotgunRotation = playerShotgun.transform.localEulerAngles;
    }
    
    void Update()
    {
        if (playerController.GetSprinting() && inventoryController.GetHoldingShotgun())
        {
            SprintingAndHoldingShotgunTransform();
        }
        else if (playerController.GetSprinting())
        {
            SprintingTransform();
        }
        else if (inventoryController.GetHoldingShotgun())
        {
            HoldingShotgunTransform();
        }
        else if (inventoryController.GetHoldingPhone())
        {
            HoldingPhoneTransform();
        }
        else if (inventoryController.GetHoldingLunchBox())
        {
            HoldingLunchBoxTransform();
        }
        else
        {
            OriginalTransform();
        }
    }
    
    void OriginalTransform()
    {
        playerModel.transform.localPosition = originalPlayerPosition;
        playerModel.transform.localEulerAngles = originalPlayerRotation;
        playerShotgun.transform.localPosition = originalShotgunPosition;
        playerShotgun.transform.localEulerAngles = originalShotgunRotation;
    }

    void SprintingTransform()
    {
        playerModel.transform.localPosition = sprintingPosition;
        playerModel.transform.localEulerAngles = sprintingRotation;
    }

    void HoldingShotgunTransform()
    {
        playerModel.transform.localPosition = holdingShotgunPosition;
        playerModel.transform.localEulerAngles = holdingShotgunRotation;
        playerShotgun.transform.localPosition = originalShotgunPosition;
        playerShotgun.transform.localEulerAngles = originalShotgunRotation;
    }

    void SprintingAndHoldingShotgunTransform()
    {
        playerModel.transform.localPosition = sprintingWithShotgunPlayerPosition;
        playerModel.transform.localEulerAngles = sprintingWithShotgunPlayerRotation;

        playerShotgun.transform.localPosition = sprintingWithShotgunShotgunPosition;
        playerShotgun.transform.localEulerAngles = sprintingWithShotgunShotgunRotation;
    }

    void HoldingPhoneTransform()
    {
        playerModel.transform.localPosition = holdingPhonePosition;
        playerModel.transform.localEulerAngles = holdingPhoneRotation;
    }

    void HoldingLunchBoxTransform()
    {
        playerModel.transform.localPosition = holdingLunchBoxPosition;
        playerModel.transform.localEulerAngles = holdingLunchBoxRotation;
    }
}

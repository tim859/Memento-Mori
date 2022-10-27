using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationParameterGetter : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] Animator playerAnimationController;
    [SerializeField] InventoryController inventoryController;


    void Update()
    {
        // no item run bool
        if (getNoItemRunParamater())
        {
            playerAnimationController.SetBool("no_item_run", true);
        }
        else
        {
            playerAnimationController.SetBool("no_item_run", false);
        }

        playerAnimationController.SetInteger("holding_shotgun", getHoldingShotgunParameter());
        playerAnimationController.SetInteger("holding_phone", getHoldingPhoneParameter());
        playerAnimationController.SetInteger("holding_lunch_box", getHoldingLunchBoxParameter());
    }

    bool getNoItemRunParamater()
    {
        if (playerController.GetSprinting() & inventoryController.GetHoldingShotgun() == false & inventoryController.GetHoldingPhone() == false & inventoryController.GetHoldingLunchBox() == false)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    int getHoldingShotgunParameter()
    {
        if (inventoryController.GetHoldingShotgun() & playerController.GetSprinting() == false)
        {
            return 1;
        }
        else if (inventoryController.GetHoldingShotgun() & playerController.GetSprinting())
        {
            return 2;
        }
        else
        {
            return 0;
        }
    }

    int getHoldingPhoneParameter()
    {
        if (inventoryController.GetHoldingPhone() & playerController.GetSprinting() == false)
        {
            return 1;
        }
        else if (inventoryController.GetHoldingPhone() & playerController.GetSprinting())
        {
            return 2;
        }
        else
        {
            return 0;
        }
    }

    int getHoldingLunchBoxParameter()
    {
        if (inventoryController.GetHoldingLunchBox() & playerController.GetSprinting() == false)
        {
            return 1;
        }
        else if (inventoryController.GetHoldingLunchBox() & playerController.GetSprinting())
        {
            return 2;
        }
        else
        {
            return 0;
        }
    }
}

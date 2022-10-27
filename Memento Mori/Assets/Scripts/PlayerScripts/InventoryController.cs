using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] GameObject playerShotgun;
    [SerializeField] GameObject playerPhone;
    [SerializeField] GameObject playerLunchBox;
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] AudioSource shotgunBlast;

    void Start()
    {
        // deactivates all items at the beginning to be sure that they will all work as intended
        // DeActivateAllItems();
        playerPhone.SetActive(false);
        playerLunchBox.SetActive(false);
    }

    void Update()
    {
        if (Input.GetButtonDown("1Key"))
        {
            // same as having no weapon
            DeActivateAllItems();
        }
        else if (Input.GetButtonDown("2Key"))
        {
            HoldShotgun();
        }
        //else if (Input.GetButtonDown("3Key"))
        //{
        //    HoldPhone();
        //}
        //else if (Input.GetButtonDown("4Key"))
        //{
        //    HoldLunchBox();
        //}
    }

    void HoldShotgun()
    {
        DeActivateAllItems();
        playerShotgun.SetActive(true);
        muzzleFlash.Stop();
        shotgunBlast.Stop();
        // line above required because otherwise the particle system for the muzzle flash will start playing when playerShotgun is set to active as it is a child of playerShotgun
        // easiest fix is just to stop it playing immediately (same thing for the shotgun sound)
    }

    void HoldPhone()
    {
        DeActivateAllItems();
        playerPhone.SetActive(true);
    }

    void HoldLunchBox()
    {
        DeActivateAllItems();
        playerLunchBox.SetActive(true);
    }

    void DeActivateAllItems()
    {
        playerShotgun.SetActive(false);
        playerPhone.SetActive(false);
        playerLunchBox.SetActive(false);
    }

    public bool GetHoldingShotgun()
    {
        if (playerShotgun.activeSelf)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool GetHoldingPhone()
    {
        if (playerPhone.activeSelf)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool GetHoldingLunchBox()
    {
        if (playerLunchBox.activeSelf)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellController : Interactable
{
    [SerializeField] PlayerController playerController;
    [SerializeField] GunController gunController;
    [SerializeField] AudioClip pickUpSound;
    [SerializeField] int numberOfShells;
    public override void Interact()
    {
        gunController.ammo += numberOfShells;
        gunController.UpdateAmmoCount();
        AudioSource.PlayClipAtPoint(pickUpSound, transform.position);
        playerController.CloseInteractableIcon();
        Destroy(gameObject);
    }
}

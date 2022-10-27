using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GunController : MonoBehaviour
{
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] Light muzzleFlashLight;
    [SerializeField] Camera fpsCam;
    [SerializeField] GameObject barrelExit;
    [SerializeField] AudioSource shotgunBlast;
    [SerializeField] AudioSource shotgunEmpty;
    [SerializeField] GameObject playerModel;

    [SerializeField] float damagePerPellet;
    [SerializeField] float maxRange;
    [SerializeField] float pelletSpread;
    [SerializeField] float timeBetweenShots;
    [SerializeField] int pelletCount;
    [SerializeField] float recoilSpeed = 0.5f;
    [SerializeField] public int ammo;

    bool currentlyBetweenShots = false;
    Vector3 originalShotgunPosition;
    Vector3 originalShotgunRotation;
    Vector3 shotgunRecoilPosition;
    Vector3 shotgunRecoilRotation;

    Vector3 originalPlayerModelPosition;
    Vector3 originalPlayerModelRotation;
    Vector3 playerModelRecoilPosition;
    Vector3 playerModelRecoilRotation;

    [SerializeField] TextMeshProUGUI ammoTextMesh;

    void Start()
    {
        muzzleFlashLight.enabled = false;
        originalShotgunPosition = transform.localPosition;
        originalShotgunRotation = transform.localEulerAngles;
        shotgunRecoilPosition = originalShotgunPosition + new Vector3(0f, 0f, 0);
        //-0.17 -0.31
        //shotgunRecoilRotation = new Vector3(-118.7f, 84.4f, 22.5f);
        // original values -60 137.5 -30
        // recoil values -118.7 84.4 22.5
        shotgunRecoilRotation = originalShotgunRotation + new Vector3(-60, -50, 50);

        originalPlayerModelPosition = playerModel.transform.localPosition;
        originalPlayerModelRotation = playerModel.transform.localEulerAngles;
        playerModelRecoilPosition = originalPlayerModelPosition + new Vector3(0f, 0f, 0f);
        playerModelRecoilRotation = originalPlayerModelRotation + new Vector3(0, 40, 0);

        UpdateAmmoCount();
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!currentlyBetweenShots)
            {
                Shoot();
            }
        }
    }

    void Shoot()
    {
        if (ammo > 0)
        {

            StartCoroutine(MuzzleFire());

            StartCoroutine(Recoil());

            muzzleFlash.Play();
            shotgunBlast.Play();

            Vector3 rayOrigin = fpsCam.transform.position;
            float totalDamage = 0f;
            bool hitEnemy = false;
            RaycastHit finalHit; // the raycasthit for the final pellet hit needs to be defined outside the for loop so that it can still be used at the end of the function

            // unfortunately, although this raycast is not actually used for anything, it still needs to be here. c# detects that the raycasthit finalHit defined above
            // could technically be null when i use it below the for loop, and doesn't realise that the bool condition hitEnemy (which needs to be true for finalHit to
            // even be used) could only ever be true if finalHit had a valid raycasthit currently inside of it
            bool uselessRaycast = Physics.Raycast(rayOrigin, fpsCam.transform.forward, out finalHit);

            for (int i = 0; i < pelletCount; i++)
            {
                // initial raycast to get the distance for the pellet raycasts
                if (Physics.Raycast(rayOrigin, fpsCam.transform.forward, out RaycastHit initialHit, maxRange))
                {
                    // get a random ray/ray direction to be used in the below calculations
                    Vector3 rayDirection = GetRandomRayDirection(rayOrigin, initialHit.distance);

                    // this raycast simulates the mechanics of a shotgun pellet. 
                    // it uses the ray origin (otherwise known as just the transform.position of the camera)
                    // and the rayDirection which will be a vector3 that will be going towards a random point within the radius of a circle
                    // the radius of the circle is calculated using the programmer defined pellet spread and the (technically player defined) distance of the inital raycast
                    // for an accurate simulation of how the pellets of a shotgun shell will spread out naturally the further away the shooter gets away from the target
                    if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hit))
                    {
                        DroneController enemy = hit.transform.GetComponent<DroneController>(); // ensures that there is a DroneController script on the hit object

                        if (enemy != null) // ensures that 
                        {
                            hitEnemy = true; // ensures that 

                            finalHit = hit;

                            enemy.SpawnHitFX(initialHit, hit);

                            totalDamage += damagePerPellet;

                            Debug.DrawRay(rayOrigin, rayDirection, Color.green, 5);
                        }
                        else
                        {
                            Debug.DrawRay(rayOrigin, rayDirection, Color.red, 5);
                        }
                    }
                }
            }

            if (hitEnemy)
            {
                DroneController rayEnemy = finalHit.transform.GetComponent<DroneController>();// the very last shotgun shell pellet/raycast will be used to calculate
                                                                                              // the velocity that is applied to the drone
                rayEnemy.TakeDamage(totalDamage, finalHit);
                // this is much more optimised now, mention in report
            }

            ammo -= 1;

            UpdateAmmoCount();

            currentlyBetweenShots = true;

            StartCoroutine(WaitForNextShot());
        }
        else
        {
            shotgunEmpty.Play();
        }
    }

    // i recieved some major guidance with being able to calculate this random ray direction thanks to thierry258 (https://forum.unity.com/threads/solved-shoot-a-raycast-toward-a-direction-inside-a-givin-radius.730826/)
    Vector3 GetRandomRayDirection(Vector3 rayOrigin, float rayDistance)
    {
        // the further away the target is, the wider the spread of the shot will be due to multiplying the offset by the non-random raycast distance
        Vector2 offset = Random.insideUnitCircle * 0.02f * pelletSpread * rayDistance;

        // calculate the destination of the random raycast by adding the random ray origin, the camera forward vector multiplied by the ray distance and the camera x and camera y vectors multiplied by the offset
        Vector3 rayDestination = rayOrigin + (fpsCam.transform.forward * rayDistance) + (fpsCam.transform.right * offset.x) + (fpsCam.transform.up * offset.y);

        // calculate the direction of the random ray cast and return it
        Vector3 rayDirection = rayDestination - rayOrigin;
        return rayDirection;
    }

    IEnumerator Recoil()
    {
        float timeElapsed = 0f;

        while (timeElapsed < recoilSpeed * 0.3f)
        {
            // shotgun position and rotation moving to the recoil position and rotation
            //transform.localPosition = Vector3.Lerp(originalShotgunPosition, shotgunRecoilPosition, timeElapsed / (recoilSpeed * 0.3f));
            transform.localEulerAngles = Vector3.Lerp(originalShotgunRotation, shotgunRecoilRotation, timeElapsed / (recoilSpeed * 0.3f));

            // player model position and rotation moving to the recoil position and rotation
            //playerModel.transform.localPosition = Vector3.Lerp(originalPlayerModelPosition, playerModelRecoilPosition, timeElapsed / (recoilSpeed * 0.3f));
            //playerModel.transform.localEulerAngles = Vector3.Lerp(originalPlayerModelRotation, playerModelRecoilRotation, timeElapsed / (recoilSpeed * 0.3f));

            timeElapsed += Time.deltaTime;

            yield return null;
        }

        timeElapsed = 0f;

        while (timeElapsed < recoilSpeed / 0.7f)
        {
            // shotgun position and rotation moving back to their original position and rotation
            //transform.localPosition = Vector3.Lerp(shotgunRecoilPosition, originalShotgunPosition, timeElapsed / (recoilSpeed * 0.7f));
            transform.localEulerAngles = Vector3.Lerp(shotgunRecoilRotation, originalShotgunRotation, timeElapsed / (recoilSpeed * 0.7f));

            // player model position and rotation moving back to their original position and rotation
            //playerModel.transform.localPosition = Vector3.Lerp(playerModelRecoilPosition, originalPlayerModelPosition, timeElapsed / (recoilSpeed * 0.7f));
            //playerModel.transform.localEulerAngles = Vector3.Lerp(playerModelRecoilRotation, originalPlayerModelRotation, timeElapsed / (recoilSpeed * 0.7f));

            timeElapsed += Time.deltaTime;

            yield return null;
        }
    }

    IEnumerator MuzzleFire()
    {
        muzzleFlashLight.enabled = true;

        yield return new WaitForSeconds(0.1f);

        muzzleFlashLight.enabled = false;
    }

    IEnumerator WaitForNextShot()
    {
        yield return new WaitForSeconds(timeBetweenShots);

        currentlyBetweenShots = false;
    }

    public void UpdateAmmoCount()
    {
        ammoTextMesh.SetText(ammo.ToString());
    }
}

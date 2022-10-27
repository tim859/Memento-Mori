using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockerController : Interactable
{
    [SerializeField] GameObject lockerDoor;
    [SerializeField] float lockerOpeningTime;
    [SerializeField] GameObject player;
    [SerializeField] Camera mainCamera;
    [SerializeField] Camera lockerCamera;

    private Vector3 lockerDoorOpen = new Vector3(0, -90, 0);
    private Vector3 lockerDoorClosed = new Vector3(0, 0, 0);
    private bool doorBusy = false;
    private bool isHiding = false;

    public override void Interact()
    {
        // doorBusy is the check to ensure that this code will not run again while the door is opening/closing i.e. if the players spams the e button, nothing strange will happen.
        if (doorBusy == false)
        {
            if (isHiding == false)
            {
                doorBusy = true;
                StartCoroutine(EnterLocker());
            }
            else if (isHiding)
            {
                doorBusy = true;
                StartCoroutine(ExitLocker());
            }
        }
    }

    private IEnumerator EnterLocker()
    {

       // player.GetComponent<PlayerController>().enabled = false;

        // this code opens the door using the lerp function
        float timeElapsed = 0f;

        while (timeElapsed < lockerOpeningTime)
        {
            lockerDoor.transform.localEulerAngles = Vector3.Lerp(lockerDoorClosed, lockerDoorOpen, timeElapsed / lockerOpeningTime);

            timeElapsed += Time.deltaTime;

            yield return null;
        }
        // this is just to ensure that the door finishes its movement in the right place, lerp often is out on the final position by a small margin
        lockerDoor.transform.localEulerAngles = lockerDoorOpen;


        SwitchToLockerCamera();


        // this code closes the door, basically the same as the open door code but reversed
        timeElapsed = 0f;

        while (timeElapsed < lockerOpeningTime)
        {
            lockerDoor.transform.localEulerAngles = Vector3.Lerp(lockerDoorOpen, lockerDoorClosed, timeElapsed / lockerOpeningTime);

            timeElapsed += Time.deltaTime;

            yield return null;
        }
        lockerDoor.transform.localEulerAngles = lockerDoorClosed;

        // the door has finished opening and closing at this point so doorBusy is now set to false
        doorBusy = false;

        // the reason isHiding is put here and not just after the SwitchToLocker method is called is because as soon as this bool is set to true, 
        // the player is able to press e to get out of the locker using the update function in this script. Putting it here ensures that the coroutine wont start running again midway through if the player presses e.
        isHiding = true;
    }

    private IEnumerator ExitLocker()
    {
        float timeElapsed = 0f;

        while (timeElapsed < lockerOpeningTime)
        {
            lockerDoor.transform.localEulerAngles = Vector3.Lerp(lockerDoorClosed, lockerDoorOpen, timeElapsed / lockerOpeningTime);

            timeElapsed += Time.deltaTime;

            yield return null;
        }

        lockerDoor.transform.localEulerAngles = lockerDoorOpen;

        SwitchToMainCamera();

        timeElapsed = 0f;

        while (timeElapsed < lockerOpeningTime)
        {
            lockerDoor.transform.localEulerAngles = Vector3.Lerp(lockerDoorOpen, lockerDoorClosed, timeElapsed / lockerOpeningTime);

            timeElapsed += Time.deltaTime;

            yield return null;
        }

        lockerDoor.transform.localEulerAngles = lockerDoorClosed;

        doorBusy = false;
        isHiding = false;
    }

    void SwitchToLockerCamera()
    {
        // disable player
        player.SetActive(false);

        //enable locker camera
        lockerCamera.enabled = true;
        lockerCamera.GetComponent<AudioListener>().enabled = true;
    }

    // the reason that these similar methods have their code switched round
    // is because i dont want two cameras to be active at the same time

    void SwitchToMainCamera()
    {
        lockerCamera.GetComponent<AudioListener>().enabled = false;

        //disable locker camera
        lockerCamera.enabled = false;

        // re-enable player
        player.SetActive(true);
    }

    private void Start()
    {
        // no need for a camera that isn't currently doing anything to be rendering
        lockerCamera.enabled = false;
    }

    private void Update()
    {
        // since the player game object is disabled when the player is inside the locker, the player controller script that usually handles inputs related to the player
        // is inactive. Therefore, this script temporarily takes over handling the input of the e buttton to ensure that the player is able to leave the locker

        if (isHiding)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Interact();
            }
        }
    }
}

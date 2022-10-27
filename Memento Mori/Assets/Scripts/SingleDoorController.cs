using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleDoorController : Interactable
{
    public bool doorLocked;
    [SerializeField] float doorOpeningTime;
    [SerializeField] GameObject door;
    bool doorBusy = false;
    bool doorOpen = false;
    Vector3 doorClosedTransform;
    Vector3 doorOpenTransform;

    void Start()
    {
        doorClosedTransform = door.transform.localEulerAngles;
        doorOpenTransform = doorClosedTransform + new Vector3(0, 0, 90);
    }

    public override void Interact()
    {
        if (!doorLocked)
        {
            if (!doorBusy)
            {
                if (!doorOpen)
                {
                    StartCoroutine(OpenDoor());
                }
                else
                {
                    StartCoroutine(CloseDoor());
                }
            }
        }
    }

    IEnumerator OpenDoor()
    {
        doorBusy = true;

        float timeElapsed = 0f;

        while (timeElapsed < doorOpeningTime)
        {
            door.transform.localEulerAngles = Vector3.Lerp(doorClosedTransform, doorOpenTransform, timeElapsed / doorOpeningTime);

            timeElapsed += Time.deltaTime;

            yield return null;
        }

        door.transform.localEulerAngles = doorOpenTransform;

        doorOpen = true;

        doorBusy = false;
    }

    IEnumerator CloseDoor()
    {
        doorBusy = true;

        float timeElapsed = 0f;

        while (timeElapsed < doorOpeningTime)
        {
            door.transform.localEulerAngles = Vector3.Lerp(doorOpenTransform, doorClosedTransform, timeElapsed / doorOpeningTime);

            timeElapsed += Time.deltaTime;

            yield return null;
        }

        door.transform.localEulerAngles = doorClosedTransform;

        doorOpen = false;

        doorBusy = false;
    }
}

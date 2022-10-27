using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleDoorController : Interactable
{
    public bool doorLocked;
    [SerializeField] float doorOpeningTime;
    [SerializeField] GameObject doorA;
    [SerializeField] GameObject doorB;
    bool doorBusy = false;
    bool doorOpen = false;
    Vector3 doorAClosedTransform;
    Vector3 doorAOpenTransform;
    Vector3 doorBClosedTransform;
    Vector3 doorBOpenTransform;

    void Start()
    {
        doorAClosedTransform = doorA.transform.localEulerAngles;
        doorAOpenTransform = doorAClosedTransform + new Vector3(0, 0, 90);

        doorBClosedTransform = doorB.transform.localEulerAngles;
        doorBOpenTransform = doorBClosedTransform + new Vector3(0, 0, -90);
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
            doorA.transform.localEulerAngles = Vector3.Lerp(doorAClosedTransform, doorAOpenTransform, timeElapsed / doorOpeningTime);
            doorB.transform.localEulerAngles = Vector3.Lerp(doorBClosedTransform, doorBOpenTransform, timeElapsed / doorOpeningTime);

            timeElapsed += Time.deltaTime;

            yield return null;
        }

        doorA.transform.localEulerAngles = doorAOpenTransform;
        doorB.transform.localEulerAngles = doorBOpenTransform;

        doorOpen = true;

        doorBusy = false;
    }

    IEnumerator CloseDoor()
    {
        doorBusy = true;

        float timeElapsed = 0f;

        while (timeElapsed < doorOpeningTime)
        {
            doorA.transform.localEulerAngles = Vector3.Lerp(doorAOpenTransform, doorAClosedTransform, timeElapsed / doorOpeningTime);
            doorB.transform.localEulerAngles = Vector3.Lerp(doorBOpenTransform, doorBClosedTransform, timeElapsed / doorOpeningTime);

            timeElapsed += Time.deltaTime;

            yield return null;
        }

        doorA.transform.localEulerAngles = doorAClosedTransform;
        doorB.transform.localEulerAngles = doorBClosedTransform;

        doorOpen = false;

        doorBusy = false;
    }
}

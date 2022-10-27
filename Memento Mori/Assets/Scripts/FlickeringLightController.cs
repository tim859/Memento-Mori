using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLightController : MonoBehaviour
{
    [SerializeField] Material emissiveMaterial;
    [SerializeField] Material nonEmissiveMaterial;
    [SerializeField] GameObject lightBulb;
    [SerializeField] Light lightEmitter;
    [SerializeField] float minimumTimeDelay;
    [SerializeField] float maximumTimeDelay;

    // light audio clips
    [SerializeField] AudioClip lightFlicker1;
    [SerializeField] AudioClip lightFlicker2;
    [SerializeField] AudioClip lightFlicker3;
    [SerializeField] AudioClip lightFlicker4;
    [SerializeField] AudioClip lightFlicker5;
    [SerializeField] AudioClip lightFlicker6;
    [SerializeField] AudioClip lightFlicker7;

    bool isFlickering = false;
    float timeDelay;
    float volume = 0.2f;

    void Update()
    {
        if (isFlickering == false)
        {
            StartCoroutine(FlickeringLight());
        }
    }

    IEnumerator FlickeringLight()
    {
        isFlickering = true;

        lightEmitter.enabled = false;
        lightBulb.GetComponent<MeshRenderer>().material = nonEmissiveMaterial;

        timeDelay = Random.Range(minimumTimeDelay, maximumTimeDelay);
        yield return new WaitForSeconds(timeDelay);

        lightEmitter.enabled = true;
        lightBulb.GetComponent<MeshRenderer>().material = emissiveMaterial;

        int randNum = Random.Range(1, 7);

        switch (randNum)
        {
            case 1:
                AudioSource.PlayClipAtPoint(lightFlicker1, transform.position, volume);
                break;

            case 2:
                AudioSource.PlayClipAtPoint(lightFlicker2, transform.position, volume);
                break;

            case 3:
                AudioSource.PlayClipAtPoint(lightFlicker3, transform.position, volume);
                break;

            case 4:
                AudioSource.PlayClipAtPoint(lightFlicker4, transform.position, volume);
                break;

            case 5:
                AudioSource.PlayClipAtPoint(lightFlicker5, transform.position, volume);
                break;

            case 6:
                AudioSource.PlayClipAtPoint(lightFlicker6, transform.position, volume);
                break;

            case 7:
                AudioSource.PlayClipAtPoint(lightFlicker2, transform.position, volume);
                break;
        }

        timeDelay = Random.Range(minimumTimeDelay, maximumTimeDelay);
        yield return new WaitForSeconds(timeDelay);
        isFlickering = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchController : MonoBehaviour
{
    [SerializeField] bool torchIsOn = false;
    [SerializeField] GameObject torchLight;
    [SerializeField] AudioSource torchClickSound;

    private void Start()
    {
        torchLight.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("FKey"))
        {
            if (torchIsOn == false)
            {
                torchLight.SetActive(true);
                torchClickSound.Play();
                torchIsOn = true;
            } 
            else
            {
                torchLight.SetActive(false);
                torchClickSound.Play();
                torchIsOn = false;
            } 
        }
    }
}

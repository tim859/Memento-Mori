using System.Collections;
using UnityEngine;
using TMPro;

public class Padlock3Controller : Interactable
{
    [SerializeField] InventoryController inventoryController;
    [SerializeField] GameController gameController;
    [SerializeField] Canvas canvas;
    [SerializeField] TMP_InputField inputField;
    [SerializeField] GameObject codeEntry;
    [SerializeField] GameObject lightBulb;
    [SerializeField] Material greenLight;
    int codeToOpen;
    bool enteringCode;

    private void Start()
    {
        // a tiny delay before getting the code is neccessary otherwise the value for the code
        // will be retrieved from gameController before it has been randomised, i.e. without the 
        // delay, the code will always just be 0.
        canvas.enabled = false;
        StartCoroutine(GetCode());
    }

    private void Update()
    {
        if (enteringCode)
        {
            if (Input.GetKeyUp(KeyCode.Return))
            {
                enteringCode = false;
                // check whether the user entered code is correct
                if (inputField.text == codeToOpen.ToString())
                {
                    gameController.padlock3Unlocked = true;
                    gameObject.GetComponent<AudioSource>().Play();
                    codeEntry.SetActive(false);
                    lightBulb.GetComponent<MeshRenderer>().material = greenLight;
                }
                inputField.text = "";
                inputField.DeactivateInputField();
                canvas.enabled = false;
                inventoryController.enabled = true;
            }
        }
    }

    IEnumerator GetCode()
    {
        yield return new WaitForSeconds(0.5f);
        codeToOpen = gameController.lockPassword3;
    }

    public override void Interact()
    {
        enteringCode = true;
        inventoryController.enabled = false;
        canvas.enabled = true;
        inputField.Select();
        inputField.ActivateInputField();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().CloseInteractableIcon();

            enteringCode = false;
            inputField.DeactivateInputField();
            canvas.enabled = false;
        }
    }
}

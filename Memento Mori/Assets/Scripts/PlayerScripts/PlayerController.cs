using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    [SerializeField] Transform playerCamera = null;

    [SerializeField] float mouseSensitivityX = 3.5f;
    [SerializeField] float mouseSensitivityY = 3.5f;
    [SerializeField] float walkSpeed = 4.0f;
    [SerializeField] float sprintSpeed = 6.0f;
    [SerializeField] float gravity = -13.0f;
    [SerializeField] float jumpHeight = 1.0f;
    [SerializeField] float playerHealth = 100f;

    [SerializeField] [Range(0.0f, 0.5f)] float moveSmoothTime = 0.3f;
    [SerializeField] [Range(0.0f, 0.5f)] float mouseSmoothTime = 0.03f;

    [SerializeField] bool lockCursor = true;

    [SerializeField] GameObject interactableIcon;

    float cameraPitch = 0.0f;
    float velocityY = 0.0f;
    float moveSpeed = 6.0f;
    CharacterController controller = null;

    Vector2 currentDirection = Vector2.zero;
    Vector2 currentDirectionVelocity = Vector2.zero;

    Vector2 currentMouseDelta = Vector2.zero;
    Vector2 currentMouseDeltaVelocity = Vector2.zero;
    bool interacting = false;

    public Slider slider;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void Update()
    {
        //Debug.Log(playerHealth);
        UpdateMouseLook();
        UpdateMovement();

        if (Input.GetButtonDown("EKey"))
        {
            CheckInteraction();
        }
    }

    void UpdateMouseLook()
    {
        Vector2 targetmouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        // smoothing out looking around with the mouse to make it less jarring and more natural
        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetmouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);

        cameraPitch -= currentMouseDelta.y * mouseSensitivityY;
        cameraPitch = Mathf.Clamp(cameraPitch, -90.0f, 90.0f);

        playerCamera.localEulerAngles = Vector3.right * cameraPitch;

        transform.Rotate(currentMouseDelta.x * mouseSensitivityX * Vector3.up);
    }

    void UpdateMovement()
    {
        Vector2 targetDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        // normalise the movement vector to ensure that diagonal movement will be locked at 1
        targetDirection.Normalize();

        // smoothing out the movement to make it less jarring and more natural
        currentDirection = Vector2.SmoothDamp(currentDirection, targetDirection, ref currentDirectionVelocity, moveSmoothTime);

        if (controller.isGrounded)
        {
            velocityY = 0.0f;
        }

        // if statement that uses a standard jumping equation when the jump key is pressed
        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            velocityY += Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }

        // if statement that sets the move speed to the sprinting speed when the left shift key is pressed
        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = sprintSpeed;
        } 
        else
        {
            moveSpeed = walkSpeed;
        }

        velocityY += gravity * Time.deltaTime;

        Vector3 velocity = (transform.forward * currentDirection.y + transform.right * currentDirection.x) * moveSpeed + Vector3.up * velocityY;

        controller.Move(velocity * Time.deltaTime);
    }

    public void TakeDamage(float amount)
    {
        playerHealth -= amount;
        UpdateHealth();

        if (playerHealth <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public bool GetSprinting()
    {
        if (moveSpeed == sprintSpeed && controller.velocity.magnitude > walkSpeed)
        {
            return true;
        } 
        else
        {
            return false;
        }
    }

    public void OpenInteractableIcon()
    {
        // interactableIcon.SetActive(true);
        interacting = true;
    }

    public void CloseInteractableIcon()
    {
        // interactableIcon.SetActive(false);
        interacting = false;
    }

    void CheckInteraction()
    {
        if (interacting)
        {
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit hit, 4))
            {
                if (hit.transform.GetComponent<Interactable>())
                {
                    hit.transform.GetComponent<Interactable>().Interact();
                }
            }
        }
    }

    public void UpdateHealth()
    {
        slider.value = playerHealth;
    }

    private void OnGUI()
    {
        GUIStyle myStyle = new GUIStyle
        {
            fontSize = 50
        };
        myStyle.normal.textColor = Color.white;

        if (interacting)
        {
            GUI.TextField(new Rect(Screen.width / 2, Screen.height / 2, 120, 20), "press e to interact", myStyle);
        }
    }
}
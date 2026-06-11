using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    [Header("Input Actions")]
    [SerializeField] private InputActionAsset playerControls;

    // Assign the player body (parent) in the Inspector so we can rotate it horizontally
    [SerializeField] private Transform playerBody;

    private InputAction lookAction;
    private Vector2 lookInput;

    public float mouseSens = 100f;
    private float xRotation = 0f;

    private void Awake()
    {
        lookAction = playerControls.FindActionMap("Player").FindAction("Look");

        // Keep input updated via callbacks rather than polling
        lookAction.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        lookAction.canceled  += ctx => lookInput = Vector2.zero;
    }

    private void OnEnable()
    {
        lookAction.Enable();  // action must be enabled before it fires
    }

    private void OnDisable()
    {
        lookAction.Disable();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = lookInput.x * mouseSens * Time.deltaTime;
        float mouseY = lookInput.y * mouseSens * Time.deltaTime;

        // Pitch — applied to the camera (this transform), clamped to avoid flipping
        xRotation -= mouseY;
        xRotation = math.clamp(xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Yaw — applied to the parent body so the whole character turns
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float mouseSensitivity = 2f;
    public Transform cameraTransform;

    private CharacterController controller;
    private float xRotation = 0f;
    [SerializeField] private float gravity = -9.81f;
    public bool lockMovement = false;
    private float verticalVelocity;
    [SerializeField] private bool restaurarValores = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        //restaura la posicion anterior
        if (restaurarValores)
        {
            transform.position = DataRestorerer.Singleton.GetLastPos();
            transform.rotation = DataRestorerer.Singleton.GetLastRotation();
        }
    }

    void Update()
    {
        if (lockMovement)
        {
            return;
        }
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 move = (transform.right * horizontal + transform.forward * vertical).normalized;
        controller.Move(move * moveSpeed * Time.deltaTime);

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -70f, 20f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
        // Aplicar gravedad
        if (controller.isGrounded)
        {
            verticalVelocity = -1f; // pegado al suelo
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        move.y = verticalVelocity;

        // Mover personaje con colisiones
        controller.Move(move * Time.deltaTime);
    }
}
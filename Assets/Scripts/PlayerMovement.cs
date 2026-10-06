using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float rotationSpeed = 120f;
    [SerializeField] private float gravity = -9.81f;

    private CharacterController controller;
    private float verticalVelocity;

    public bool lockMovement = false;
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (lockMovement)
        {
            return;
        }
        float rot = Input.GetAxis("Horizontal");   // rotar izquierda/derecha
        float forward = Input.GetAxis("Vertical"); // avanzar/retroceder

        // Rotación del personaje
        transform.Rotate(0f, rot * rotationSpeed * Time.deltaTime, 0f);

        // Movimiento hacia adelante
        Vector3 move = transform.forward * forward * speed;

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

using Unity.Mathematics;
using UnityEngine;

public class InteriorMOvement : MonoBehaviour
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
    private float prevSpeed;
    public AudioClip andarSound;
    public AudioClip runSound;
    [SerializeField] private AudioSource audioSource;
    void Start()
    {
        mouseSensitivity = PlayerPrefs.GetFloat("Sensibilidad", 1f);//sensibilidad
        controller = GetComponent<CharacterController>();
        prevSpeed = moveSpeed;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        audioSource = GetComponent<AudioSource>();

        //restaura la posicion anterior
        if (restaurarValores)
        {
            DatosPartida datos = SavePartida.Singleton.Cargar(SavePartida.Singleton.GetKey());
            if (datos == null) datos = new DatosPartida();
            Teletransportar(datos.posicion, datos.rotacion);
            //transform.position = DataRestorerer.Singleton.GetLastPos();
            //transform.rotation = DataRestorerer.Singleton.GetLastRotation();
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
        bool moviendo = horizontal != 0 || vertical != 0;
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            moveSpeed = 2f * prevSpeed;
            audioSource.pitch = 2f;
            if ((audioSource.clip != runSound || !audioSource.isPlaying) && moviendo)
            {
                audioSource.clip = runSound;
                audioSource.Play();
            }
        }
        else
        {
            if (moviendo)
            {
                audioSource.pitch = 1.0f;
                moveSpeed = prevSpeed;
                if (audioSource.clip != andarSound || !audioSource.isPlaying)
                {
                    audioSource.clip = andarSound;
                    audioSource.Play();
                }
            }
        }
        if (!moviendo)
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
                audioSource.pitch = 1f;
                audioSource.clip = null;
            }
        }
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
    public void Teletransportar(Vector3 pos, Quaternion rot)
    {
        CharacterController cc = GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        transform.SetPositionAndRotation(pos, rot);
        cc.enabled = true;
    }
    //cosas sensibilidad
    private void OnEnable()
    {
        MusicController.OnSensibilidadCambiada += ActualizarSensibilidad;
    }

    private void OnDisable()
    {
        MusicController.OnSensibilidadCambiada -= ActualizarSensibilidad;
    }

    private void ActualizarSensibilidad(float nuevaSensibilidad)
    {
        mouseSensitivity = nuevaSensibilidad;
    }

}

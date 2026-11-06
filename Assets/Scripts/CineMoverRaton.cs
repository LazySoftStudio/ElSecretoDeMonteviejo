using Unity.Cinemachine;
using UnityEngine;

public class CineMoverRaton : MonoBehaviour
{
    
    public CinemachineCamera camaraVirtual;
    public float sensitividad = 20f;

    private float offsetZAcumulado = 0f;
    private CinemachineHardLookAt chla;

    void Start()
    {
        // Obtenemos el componente HardLookAt de la cámara virtual
        if (camaraVirtual != null)
        {
            chla = camaraVirtual.GetComponent<CinemachineHardLookAt>();
            // Inicializamos con el valor que ya tenga configurado en el inspector
            if (chla != null)
            {
                offsetZAcumulado = chla.LookAtOffset.z;
            }
        }
    }

    private void Update()
    {
        if (chla == null) return;

        // 1. Conseguimos el movimiento del ratón
        float mouseY = Input.GetAxis("Mouse Y") * sensitividad * Time.deltaTime;

        // 2. Acumulamos el valor en lugar de sobreescribirlo 
        offsetZAcumulado += mouseY;

        offsetZAcumulado = Mathf.Clamp(offsetZAcumulado, 20f, 100f);

        chla.LookAtOffset = new Vector3(chla.LookAtOffset.x, chla.LookAtOffset.y, offsetZAcumulado);
    }
}

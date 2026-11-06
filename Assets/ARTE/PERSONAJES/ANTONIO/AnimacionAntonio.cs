using UnityEngine;

public class AnimacionAntonio : MonoBehaviour
{
    private Animator animador;
    private FirstPersonMovement fpm;
    void Start()
    {
        animador = GetComponent<Animator>();
        fpm = GetComponent<FirstPersonMovement>();
    }
    void Update()
    {
        float movimientoHorizontal = Input.GetAxis("Horizontal");
        float movimientoVertical = Input.GetAxis("Vertical");

        bool moviendo = movimientoHorizontal != 0 || movimientoVertical != 0;

        if (moviendo)
        {
            if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) //Correr
            {
                fpm.moveSpeed = 6;//corriendo
                animador.SetBool("IsAndando",  false);
                animador.SetBool("IsCorriendo", true);
            }
            else // andar
            {
                fpm.moveSpeed = 1.5f;//andando
                animador.SetBool("IsAndando", true);
                animador.SetBool("IsCorriendo", false);
            }
        } 
        else // parar
        {
            animador.SetBool("IsAndando", false);
            animador.SetBool("IsCorriendo", false);
        }

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            animador.SetTrigger("Sentarse");
        }
        if(Input.GetKeyDown(KeyCode.Alpha2)) 
        {
            animador.SetTrigger("Saludar");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            animador.SetTrigger("Voltereta");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            animador.SetTrigger("Victoria");
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            animador.SetTrigger("Interactuar");
        }

    }
}

using UnityEngine;
using UnityEngine.Rendering;

public class CicloDiaNoche : MonoBehaviour 
{

    [Range(0.0f, 24f)] public float Hora = 12f; // Hora inicial 12 del mediodia. Solo puede ir de 0 a 24
    public Transform Sol;
    public Material dia;
    public Material noche;

    public float DuracionDia = 1; //Duración del día en minutos

    public float InicioAmanecer = 5f; // Hora de inicio del amanecer
    public float FinAmanecer = 7f; // Hora de fin del amanecer
    public float InicioAtardecer = 17f;
    public float FinAtardecer = 19f;

    private float SolX;

    private void Start()
    {
        SetMaterial(dia, 1f);
        SetMaterial(noche, 0f);
    }


    private void Update()
    {
        Hora += Time.deltaTime * (24 / (60 * DuracionDia)); //Actualizar la hora

        //Evitar que se salga del rango de horas
        if (Hora >= 24f){
            Hora = 0f;
        }

        RotacionSol();
        MaterialCielo(); 
    }



    // Dependiendo de la hora del día, el sol está en una posición u otra
    void RotacionSol()
    {
        SolX = 15 * Hora; // 360 grados / 24 horas = 15 grados por hora
        Sol.localEulerAngles = new Vector3(SolX, 0, 0);

    }


    void MaterialCielo()
    {
        if (Hora >= InicioAmanecer && Hora <= FinAmanecer) // Amanecer
        {
            float t = (Hora - InicioAmanecer) / (FinAmanecer - InicioAmanecer);
            SetMaterial(dia, t);
            SetMaterial(noche, 1 - t);

            Sol.GetComponent<Light>().intensity = t; // Encender la luz del sol

        }
        else if (Hora >= InicioAtardecer && Hora <= FinAtardecer) // Atardecer
        {
            float t = (Hora - InicioAtardecer) / (FinAtardecer - InicioAtardecer);
            SetMaterial(dia, 1 - t);
            SetMaterial(noche, t);

            Sol.GetComponent<Light>().intensity =1- t; // Apagar la luz del sol para que no alumine desde debajo del suelo
        }
    }


    void SetMaterial(Material mat, float alpha)
    {
        Color color = mat.color;
        color.a = alpha;
        mat.color = color;
    }
}

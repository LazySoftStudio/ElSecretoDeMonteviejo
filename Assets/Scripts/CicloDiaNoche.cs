using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Audio; 

public class CicloDiaNoche : MonoBehaviour
{
    [Range(0.0f, 24f)] public float Hora = 12f;
    public Transform Sol;
    public Material dia;
    public Material noche;

    public float DuracionDia = 1; // Duración del día en minutos

    public float InicioAmanecer = 5f;
    public float FinAmanecer = 7f;
    public float InicioAtardecer = 17f;
    public float FinAtardecer = 19f;

    [Header("Audio Ciclo Día/Noche")] 
    public AudioMixerSnapshot snapshotDia;
    public AudioMixerSnapshot snapshotNoche;
    private bool esDeDia;

    private float SolX;

    private void Start()
    {
        DatosPartida datos = SavePartida.Singleton.Cargar(SavePartida.Singleton.GetKey());
        if (datos == null) datos = new DatosPartida();
        Hora = datos.horaDia;

        SetMaterial(dia, 1f);
        SetMaterial(noche, 0f);


        if (Hora >= InicioAmanecer && Hora < InicioAtardecer)
        {
            esDeDia = true;
            snapshotDia.TransitionTo(0f); 
        }
        else
        {
            esDeDia = false;
            snapshotNoche.TransitionTo(0f);
        }
    }

    private void Update()
    {
        Hora += Time.deltaTime * (24 / (60 * DuracionDia));

        if (Hora >= 24f)
        {
            Hora = 0f;
        }

        RotacionSol();
        MaterialCielo();
        ControlarMusica();
    }

    void ControlarMusica()
    {
        if (Hora >= InicioAmanecer && Hora < InicioAtardecer && !esDeDia)
        {
            esDeDia = true;
            // segundos reales dura el amanecer para sincronizar el audio
            float horasAmanecer = FinAmanecer - InicioAmanecer;
            float segundosTransicion = (horasAmanecer / 24f) * (DuracionDia * 60f);

            snapshotDia.TransitionTo(segundosTransicion);
        }
        else if ((Hora >= InicioAtardecer || Hora < InicioAmanecer) && esDeDia)
        {
            esDeDia = false;
            float horasAtardecer = FinAtardecer - InicioAtardecer;
            float segundosTransicion = (horasAtardecer / 24f) * (DuracionDia * 60f);

            snapshotNoche.TransitionTo(segundosTransicion);
        }
    }

    void RotacionSol()
    {
        SolX = 15 * Hora;
        Sol.localEulerAngles = new Vector3(SolX, 0, 0);
    }

    void MaterialCielo()
    {
        if (Hora >= InicioAmanecer && Hora <= FinAmanecer)
        {
            float t = (Hora - InicioAmanecer) / (FinAmanecer - InicioAmanecer);
            SetMaterial(dia, t);
            SetMaterial(noche, 1 - t);

            Sol.GetComponent<Light>().intensity = t;
        }
        else if (Hora >= InicioAtardecer && Hora <= FinAtardecer)
        {
            float t = (Hora - InicioAtardecer) / (FinAtardecer - InicioAtardecer);
            SetMaterial(dia, 1 - t);
            SetMaterial(noche, t);

            Sol.GetComponent<Light>().intensity = 1 - t;
        }
    }

    void SetMaterial(Material mat, float alpha)
    {
        Color color = mat.color;
        color.a = alpha;
        mat.color = color;
    }
}

using UnityEngine;
using UnityEngine.Rendering;

public class Farolas : MonoBehaviour
{
    CicloDiaNoche ciclo;
    public float horaDia = 10f;
    public float horaEncendido = 18f;
    public float horaApagado = 6f;
    public GameObject luz; 
    void Start()
    {
        ciclo = GameObject.FindWithTag("DiaNoche").GetComponent<CicloDiaNoche>();
    }

    void Update()
    {
        if (ciclo) horaDia = ciclo.Hora;

        if (horaDia >= horaEncendido || horaDia <= horaApagado)
        {
            luz.SetActive(true);
        }
        else
        {
            luz.SetActive(false);
        }
    }
}

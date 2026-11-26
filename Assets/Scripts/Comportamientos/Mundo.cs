using UnityEngine;

public class Mundo : MonoBehaviour
{
    [SerializeField] private Transform[] lugaresDescansar;
    [SerializeField] private Transform[] lugaresComer;
    [SerializeField] private Transform[] lugaresBaño;
    [SerializeField] private Transform[] puntosRandom;




    public Transform GetLugarDescanso()
    {
        return lugaresDescansar[Random.Range(0, lugaresDescansar.Length)];
    }
    public Transform GetLugarComer()
    {
        return lugaresComer[Random.Range(0, lugaresComer.Length)];
    }

    public Transform GetLugarBaño()
    {
        return lugaresBaño[Random.Range(0, lugaresBaño.Length)];
    }

    public Transform GetLugarRandom()
    {
        return puntosRandom[Random.Range(0, puntosRandom.Length)];
    }

}

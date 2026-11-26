using UnityEngine;
using UnityEngine.AI;

public class TrabajoBandos : MonoBehaviour
{
    private NavMeshAgent agente;

    public Transform[] puntosAleatorios;

    private bool aldeanoCercano;
    private bool mismoBando; 

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
    }

    public void Pasear()
    {
        Debug.Log(this.name + ": Me voy a pasear");  
        int punto = Random.Range(0, puntosAleatorios.Length);
        agente.SetDestination(puntosAleatorios[punto].position); 
    }
}

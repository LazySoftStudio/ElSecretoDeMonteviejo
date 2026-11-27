using UnityEngine;
using UnityEngine.AI;

public class TrabajoBandos : MonoBehaviour
{
    private NavMeshAgent agente;

    [SerializeField]private AldeanoGenerico aldeanoGen;
    private bool aldeanoCercano;
    private bool mismoBando;
    public Sprite trabajo;

    private Transform ultimoDestino; //para evitar que cuando llegue escoja el mismo punto
    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //si llega elige otro punto nuevo
        if (HaLlegadoAlDestino() && aldeanoGen.accionActual == AldeanoGenerico.AccionActual.Trabajar)
        {
            IrANuevoPuntoRandom(); 
        }

    }
    public void Pasear()
    {
        aldeanoGen.objetoCabeza.sprite = trabajo;
        aldeanoGen.accionActual = AldeanoGenerico.AccionActual.Trabajar;
        Debug.Log(this.name + ": Me voy a pasear");
        IrANuevoPuntoRandom();
    }

    private void IrANuevoPuntoRandom()
    {
        Transform nuevoDestino = aldeanoGen.mundo.GetLugarRandom();

        // Evitamos elegir el mismo punto en el que ya está
        int intentos = 0;
        while (nuevoDestino == ultimoDestino && intentos < 10)
        {
            nuevoDestino = aldeanoGen.mundo.GetLugarRandom();
            intentos++;
        }

        ultimoDestino = nuevoDestino;
        agente.SetDestination(nuevoDestino.position);
    }


    bool HaLlegadoAlDestino()
    {
        if (agente.pathPending)
            return false;

        if (agente.remainingDistance <= agente.stoppingDistance + 0.2f)
        {
            return true;
        }
        return false;
    }
}

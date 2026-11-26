using UnityEngine;
using UnityEngine.AI;

public class TrabajoBandos : MonoBehaviour
{
    private NavMeshAgent agente;

    [SerializeField]private AldeanoGenerico aldeanoGen;
    private bool aldeanoCercano;
    private bool mismoBando;
    public Sprite trabajo;

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //si llega elige otro punto nuevo
        if (HaLlegadoAlDestino() && aldeanoGen.accionActual == AldeanoGenerico.AccionActual.Trabajar)
        {
            agente.SetDestination(aldeanoGen.mundo.GetLugarRandom().position);
        }

    }
    public void Pasear()
    {
        aldeanoGen.objetoCabeza.sprite = trabajo;
        aldeanoGen.accionActual = AldeanoGenerico.AccionActual.Trabajar;
        Debug.Log(this.name + ": Me voy a pasear");  
        agente.SetDestination(aldeanoGen.mundo.GetLugarRandom().position);
    }


    bool HaLlegadoAlDestino()
    {
        if (agente.pathPending)
            return false;

        if (agente.remainingDistance <= agente.stoppingDistance)
        {
            if (!agente.hasPath || agente.velocity.sqrMagnitude == 0f)
            {
                return true;
            }
        }
        return false;
    }
}

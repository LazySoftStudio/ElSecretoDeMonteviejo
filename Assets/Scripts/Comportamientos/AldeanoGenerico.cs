using System.Collections; // Necesario para usar corrutinas (IEnumerator)
using UnityEngine;
using UnityEngine.AI;

public class AldeanoGenerico : MonoBehaviour
{
    private NavMeshAgent agente;

    [Header("Puntos de destino")]
    public Transform puntoDescansar;
    public Transform puntoComer;
    public Transform puntoBanio;

    [Header("Percepciones")]
    public float cansancio = 0f;
    public float ganasComer = 0f;
    public float ganasIrBanio = 0f;
    public float ganasTrabajar = 0f;
    [Header("Percepciones incrementos")]
    public float incremento = 0.01f;
    [Header("Tiempos de espera")]
    public float tiempoEsperaComer = 3f;
    public float tiempoEsperaDescansar = 5f;
    public float tiempoEsperaBanio = 3f;
    private enum AccionActual { Ninguna, Descansar, Comer, Banio, Trabajar }
    private AccionActual accionActual = AccionActual.Ninguna;
    private bool estaEsperando = false;
    public GameObject pausa;

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (pausa.gameObject.activeInHierarchy) return;
        if (accionActual != AccionActual.Ninguna && !estaEsperando && HaLlegadoAlDestino())
        {
            StartCoroutine(EsperarYCompletarAccion());
        }
    }

    IEnumerator EsperarYCompletarAccion()
    {
        estaEsperando = true;
        float tiempoEspera = GetTiempoEspera();

        yield return new WaitForSeconds(tiempoEspera);

        if (accionActual == AccionActual.Descansar)
        {
            cansancio = 0f;
            ganasIrBanio += 0.1f;
        }
        else if (accionActual == AccionActual.Comer)
        {
            ganasComer = 0f;
            cansancio += 0.1f;
        }
        else if (accionActual == AccionActual.Banio)
        {
            ganasIrBanio = 0f;
            ganasComer += 0.1f;
        }

        accionActual = AccionActual.Ninguna;
        estaEsperando = false;
    }

    public void IrADescansar()
    {
        if (puntoDescansar != null && !estaEsperando)
        {
            agente.SetDestination(puntoDescansar.position);
            accionActual = AccionActual.Descansar;
        }
    }

    public void IrAComer()
    {
        if (puntoComer != null && !estaEsperando)
        {
            agente.SetDestination(puntoComer.position);
            accionActual = AccionActual.Comer;
        }
    }
    public void IrABanio()
    {
        if (puntoBanio != null && !estaEsperando)
        {
            agente.SetDestination(puntoBanio.position);
            accionActual = AccionActual.Banio;
        }
    }

    public float IncrementarCansancio()
    {
        if (pausa.gameObject.activeInHierarchy) return cansancio;
        cansancio += incremento;
        if (cansancio > 10f) cansancio = 10f;
        return cansancio;
    }

    public float IncrementarGanasComer()
    {
        if (pausa.gameObject.activeInHierarchy) return ganasComer;
        ganasComer += incremento;
        if (ganasComer > 10f) ganasComer = 10f;
        return ganasComer;
    }
    public float IncrementarGanasBanio()
    {
        if (pausa.gameObject.activeInHierarchy) return ganasIrBanio;
        ganasIrBanio += incremento;
        if (ganasIrBanio > 10f) ganasIrBanio = 10f;
        return ganasIrBanio;
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

    float GetTiempoEspera()
    {
        if (accionActual == AccionActual.Descansar)
        {
            return tiempoEsperaDescansar;
        }
        else if (accionActual == AccionActual.Comer)
        {
            return tiempoEsperaComer;
        }
        else if (accionActual == AccionActual.Banio)
        {
            return tiempoEsperaBanio;
        }
        return 0;
    }
}

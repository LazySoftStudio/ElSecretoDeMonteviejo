using System.Collections; // Necesario para usar corrutinas (IEnumerator)
using UnityEngine;
using UnityEngine.AI;

public class AldeanoGenerico : MonoBehaviour
{
    private NavMeshAgent agente;

    [Header("Puntos de destino")]
    public Mundo mundo;

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
    public enum AccionActual { Ninguna, Descansar, Comer, Banio, Trabajar }
    public AccionActual accionActual = AccionActual.Ninguna;
    private bool estaEsperando = false;
    public GameObject pausa;

    [Header("Objeto en cabeza")]
    public SpriteRenderer objetoCabeza;
    public Sprite comer;
    public Sprite banio;
    public Sprite descansar;

    void Awake()
    {
        agente = GetComponent<NavMeshAgent>();
        mundo = GameObject.FindWithTag("Mundo").GetComponent<Mundo>();
    }

    void Update()
    {
        if (objetoCabeza.sprite == null) { objetoCabeza.gameObject.SetActive(false); } else { objetoCabeza.gameObject.SetActive(true); }
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
            //ganasIrBanio += 0.1f;
        }
        else if (accionActual == AccionActual.Comer)
        {
            ganasComer = 0f;
            //cansancio += 0.1f;
        }
        else if (accionActual == AccionActual.Banio)
        {
            ganasIrBanio = 0f;
            //ganasComer += 0.1f;
        }

        accionActual = AccionActual.Ninguna;
        estaEsperando = false;
    }

    public void IrADescansar()
    {
        if (!estaEsperando)
        {
            objetoCabeza.sprite = descansar;
            agente.SetDestination(mundo.GetLugarDescanso().position);
            accionActual = AccionActual.Descansar;
        }
    }

    public void IrAComer()
    {
        if (!estaEsperando)
        {
            objetoCabeza.sprite = comer;
            agente.SetDestination(mundo.GetLugarComer().position);
            accionActual = AccionActual.Comer;
        }
    }
    public void IrABanio()
    {
        if (!estaEsperando)
        {
            objetoCabeza.sprite = banio;
            agente.SetDestination(mundo.GetLugarBaño().position);
            accionActual = AccionActual.Banio;
        }
    }

    public float IncrementarCansancio()
    {
        if (pausa.gameObject.activeInHierarchy || estaEsperando) return cansancio;
        cansancio += incremento;
        if (cansancio > 10f) cansancio = 10f;
        return cansancio;
    }

    public float IncrementarGanasComer()
    {
        if (pausa.gameObject.activeInHierarchy || estaEsperando) return ganasComer;
        ganasComer += incremento;
        if (ganasComer > 10f) ganasComer = 10f;
        return ganasComer;
    }
    public float IncrementarGanasBanio()
    {
        if (pausa.gameObject.activeInHierarchy || estaEsperando) return ganasIrBanio;
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

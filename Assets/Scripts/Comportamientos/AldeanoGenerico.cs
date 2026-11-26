using System.Collections; // Necesario para usar corrutinas (IEnumerator)
using System.Drawing;
using Unity.VisualScripting;
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
    public float tiempoEsperaComer = 4.125f; //tiempo de la animación
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

    [Header("Animación")]
    public Animator animator;

    private Transform destinoActual; 

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

        if (accionActual == AccionActual.Descansar)
        {
            yield return StartCoroutine(Alinear(destinoActual.rotation)); 
            animator.SetTrigger("Sentarse");
            yield return new WaitForSeconds(tiempoEspera);
            animator.SetTrigger("Levantarse");
            yield return new WaitForSeconds(1.292f);
        }
        else if (accionActual == AccionActual.Comer)
        {
            animator.SetTrigger("Comer");
            yield return new WaitForSeconds(tiempoEspera);
        }
        else if (accionActual == AccionActual.Banio)
        {
            Vector3 posIni = transform.position;
            agente.enabled = false; //desactivar el navmesh para que no lo recoloque automaticamente
            transform.position = new Vector3(posIni.x , posIni.y - 1000, posIni.z);
            yield return new WaitForSeconds(tiempoEspera);
            transform.position = posIni;
            agente.enabled = true; 
        }
        else
        {
            yield return null; 
        }


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
            destinoActual = mundo.GetLugarDescanso(transform.position); 
            agente.SetDestination(destinoActual.position);
            accionActual = AccionActual.Descansar;
        }
    }

    public void IrAComer()
    {
        if (!estaEsperando)
        {
            objetoCabeza.sprite = comer;
            agente.SetDestination(mundo.GetLugarComer(transform.position).position);
            accionActual = AccionActual.Comer;
        }
    }
    public void IrABanio()
    {
        if (!estaEsperando)
        {
            objetoCabeza.sprite = banio;
            agente.SetDestination(mundo.GetLugarBaño(transform.position).position);
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

    IEnumerator Alinear(Quaternion rotObjetivo)
    {
        float duracionGiro = 1f; 
        float tiempo = 0f;
        Quaternion rotacionInicial = transform.rotation;
        Vector3 eulerObjetivo = rotObjetivo.eulerAngles;
        Quaternion objetivoPlano = Quaternion.Euler(0f, eulerObjetivo.y + 180, 0f);

        while (tiempo < duracionGiro)
        {
            transform.rotation = Quaternion.Slerp(rotacionInicial, objetivoPlano, tiempo / duracionGiro);
            tiempo += Time.deltaTime;
            yield return null;
        }

        transform.rotation = objetivoPlano;

    }
}
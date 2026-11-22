using HeyHeyThere.SceneTransitionsFree;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CinePelea : MonoBehaviour
{
    [Serializable]
    public class LineaDialogo
    {
        public string quien;
        [TextArea(2, 4)] public string texto;
        public bool provisional;//no esta en el gdd
    }

    [Serializable]
    public class PuntoDialogo
    {
        public string id;//plano
        public float tiempo;//segundo en el que se para
        public List<LineaDialogo> lineas = new List<LineaDialogo>();
    }

    [SerializeField] private Animator animator;
    [SerializeField] private int escenaAlTerminar = 1;//pueblo

    public GameObject pausa;
    public GameObject skip;

    [Header("Dialogos")]
    public List<PuntoDialogo> puntosDialogo = new List<PuntoDialogo>();
    public UnityEvent<string> alPararParaDialogo = new UnityEvent<string>();
    public float esperaSiNadieAtiende = 2f;//si no hay dialogos conectados sigue sola

    public event Action<PuntoDialogo> DialogoEmpieza;

    [Header("Hora")]
    public bool fijarHora = true;
    [Range(0f, 24f)] public float hora = 13.5f;
    public Transform sol;
    public Material cieloDia;
    public Material cieloNoche;

    private int pulsaciones = 0;
    private bool pausado;
    private bool terminada;
    private float timer = 0;
    private CanvasGroup skipGroup;

    private int siguienteDialogo = 0;
    private bool enDialogo;
    private bool nadieAtiende;
    private float esperaDialogo;

    [Header("Audio")]
    public AudioSource audioSource;
    public bool EnDialogo => enDialogo;
    public PuntoDialogo DialogoActual { get; private set; }

    void Reset() => animator = GetComponent<Animator>();

    void Awake()
    {
        if (animator == null) animator = GetComponent<Animator>();
        if (skip != null)
        {
            skipGroup = skip.GetComponent<CanvasGroup>();
            skip.SetActive(false);
        }
        if (pausa != null) pausa.SetActive(false);
        puntosDialogo.Sort((a, b) => a.tiempo.CompareTo(b.tiempo));
        AplicarHora();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;
        audioSource.Play();
    }

    void AplicarHora()
    {
        if (!fijarHora) return;
        if (sol != null) sol.localEulerAngles = new Vector3(15f * hora, 0f, 0f);
        SetAlpha(cieloDia, 1f);
        SetAlpha(cieloNoche, 0f);
    }

    static void SetAlpha(Material mat, float alpha)
    {
        if (mat == null) return;
        Color c = mat.color;
        c.a = alpha;
        mat.color = c;
    }

    float DuracionClip()
    {
        //info.length no vale con speed 0
        if (animator == null || animator.runtimeAnimatorController == null) return 0f;
        AnimatorClipInfo[] clips = animator.GetCurrentAnimatorClipInfo(0);
        return clips.Length > 0 && clips[0].clip != null ? clips[0].clip.length : 0f;
    }

    public float TiempoActual() => animator == null ? 0f : animator.GetCurrentAnimatorStateInfo(0).normalizedTime * DuracionClip();

    void Update()
    {
        if (terminada) return;

        if (enDialogo)
        {
            //en el dialogo las teclas son del dialogue manager
            if (nadieAtiende)
            {
                esperaDialogo -= Time.deltaTime;
                if (esperaDialogo <= 0f) ContinuarTrasDialogo();
            }
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pausado) QuitarPausa();
            else Pausar();
        }

        if (!pausado)
        {
            ActualizarSkip();
            ComprobarDialogo();
        }
    }

    void ComprobarDialogo()
    {
        if (terminada || siguienteDialogo >= puntosDialogo.Count) return;
        PuntoDialogo p = puntosDialogo[siguienteDialogo];
        if (TiempoActual() < p.tiempo) return;
        siguienteDialogo++;
        EmpezarDialogo(p);
    }

    void EmpezarDialogo(PuntoDialogo p)
    {
        enDialogo = true;
        DialogoActual = p;
        animator.speed = 0f;
        float duracion = DuracionClip();
        if (duracion > 0f)
            animator.Play(animator.GetCurrentAnimatorStateInfo(0).fullPathHash, 0, p.tiempo / duracion);
        pulsaciones = 0;
        if (skip != null) skip.SetActive(false);

        bool hayOyentes = DialogoEmpieza != null || alPararParaDialogo.GetPersistentEventCount() > 0;
        DialogoEmpieza?.Invoke(p);
        alPararParaDialogo.Invoke(p.id);
        nadieAtiende = !hayOyentes;
        esperaDialogo = esperaSiNadieAtiende;
        audioSource.Pause();
        if (nadieAtiende)
            Debug.Log("dialogo " + p.id + " sin conectar");
    }

    //llamar cuando acabe el dialogo
    public void ContinuarTrasDialogo()
    {
        if (!enDialogo) return;
        enDialogo = false;
        nadieAtiende = false;
        DialogoActual = null;
        if (!pausado && !terminada) animator.speed = 1f;
        audioSource.UnPause();
    }

    void ActualizarSkip()
    {
        // se ha puesto un boton de saltar pporque con los dialogos se puede saltar sin querer con el espacio
        /*
        if (pulsaciones == 2 && skipGroup != null)
        {
            timer -= Time.deltaTime;
            skipGroup.alpha = Mathf.Clamp01(timer / 3f);
            if (timer <= 0f)
            {
                pulsaciones = 0;
                skip.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            pulsaciones++;
            if (pulsaciones == 2)
            {
                if (skip != null)
                {
                    if (skipGroup != null) skipGroup.alpha = 1f;
                    skip.SetActive(true);
                }
                timer = 2f;
            }
            else if (pulsaciones >= 3)
            {
                Terminar();
            }
        }
        */
    }

    IEnumerator Start()
    {
        yield return null;

        yield return new WaitUntil(() =>
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f || terminada);

        if (!terminada) Terminar();
    }

    public void Terminar()
    {
        if (terminada) return;
        terminada = true;
        enDialogo = false;
        animator.speed = 0f;
        AudioListener.pause = false;
        if (skip != null) skip.SetActive(false);
        StartCoroutine(CambiarEscena());
    }

    IEnumerator CambiarEscena()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        SceneTransition.ChangeScene(escenaAlTerminar, "fade");
    }

    public void Pausar()
    {
        if (enDialogo) return;
        pausado = true;
        if (pausa != null) pausa.SetActive(true);
        animator.speed = 0f;
        AudioListener.pause = true;
        Cursor.visible = true;
        audioSource.Pause();
    }

    public void QuitarPausa()
    {
        pausado = false;
        if (pausa != null) pausa.SetActive(false);
        if (!enDialogo) animator.speed = 1f;
        AudioListener.pause = false;
        Cursor.visible = false;
        audioSource.UnPause();
    }

    void OnDestroy()
    {
        AudioListener.pause = false;
    }
}

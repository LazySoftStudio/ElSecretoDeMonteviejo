using HeyHeyThere.SceneTransitionsFree;
using System.Collections;
using UnityEngine;

public class CineInicial : MonoBehaviour
{
    [SerializeField] private Animator animator;
    public GameObject pausa;
    public GameObject skip;
    private int pulsaciones = 0;
    private bool pausado;
    private bool terminada;
    private float timer = 0;
    private CanvasGroup skipGroup;
    public int sceneToChange = 1;
    public AudioSource audioSource;
    public float delay = 0.7f;
    void Reset() => animator = GetComponent<Animator>();
    

    void Awake()
    {
        skipGroup = skip.GetComponent<CanvasGroup>();
        audioSource.PlayDelayed(delay);//retraso para que se ajuste con la animacion
    }

    void Update()
    {
        if (terminada) return;   // una vez terminada, ya no se puede pausar

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pausado = !pausado;
            if (pausado)
            {
                audioSource.Pause();
            }
            else
            {
                audioSource.UnPause();
            }
            pausa.SetActive(pausado);
            animator.speed = pausado ? 0f : 1f;
        }

        if (pulsaciones == 1)
        {
            timer -= Time.deltaTime;
            skipGroup.alpha = Mathf.Clamp01(timer/3);
            if (timer <= 0f)
            {
                pulsaciones = 0;
                skip.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            pulsaciones++;

            if (pulsaciones == 1)
            {
                // Primera pulsación: mostrar skip y empezar la cuenta atrás
                skipGroup.alpha = 1f;
                skip.SetActive(true);
                timer = 3f;
            }
            else if (pulsaciones == 2)
            {
                // Segunda pulsación: saltar la animación
                terminada = true;
                audioSource.Stop();

                // 1. Obtener el estado actual y adelantarlo al 100% (1f)
                AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                animator.Play(stateInfo.fullPathHash, 0, 1f);
                animator.Update(0f); // Fuerza a Unity a colocar los objetos en su posición final ya mismo
                animator.speed = 0f; // Congela la animación en ese último fotograma

                Debug.Log("Animación terminada");

                if (sceneToChange == 1) // inicial cinematica
                {
                    SavePartida.Singleton.AnimacionInicialHecha();
                }
                else if (sceneToChange != -1)
                {
                    SavePartida.Singleton.AnimacionBosqueHecha();
                }

                if (sceneToChange != -1)
                {
                    SceneTransition.ChangeScene(sceneToChange, "fade");
                }
                else
                {
                    StartCoroutine(CineFinal()); // final
                }
            }
        }
    }

    IEnumerator Start()
    {
        yield return null;   // espera un frame para que el Animator evalúe el estado inicial

        yield return new WaitUntil(() =>
        animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        audioSource.Stop();
        terminada = true;
        animator.speed = 0f;
        Debug.Log("Animación terminada");
        if (sceneToChange == 1)//inicial cinematica
        {
            SavePartida.Singleton.AnimacionInicialHecha();
        }
        else
        {
            if (sceneToChange != -1)
                SavePartida.Singleton.AnimacionBosqueHecha();
        } 
        yield return new WaitForSecondsRealtime(0.5f);
        if (sceneToChange != -1) {
            SceneTransition.ChangeScene(sceneToChange, "fade");   // al pueblo
        }
        else
        {
            StartCoroutine(CineFinal());
        }
    }

    IEnumerator CineFinal()
    {
        // Espera 1 frame para que Unity "suelte" la pulsación de Espacio anterior
        yield return null;

        pulsaciones = 1;
        timer = 3f;
        skipGroup.alpha = 1f;
        skip.SetActive(true);

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        SceneTransition.ChangeScene(4, "fade");
    }


    public void QuitarPausa()
    {
        pausado = false;
        audioSource.UnPause();
        pausa.SetActive(pausado);
        animator.speed = 1f;
    }


}

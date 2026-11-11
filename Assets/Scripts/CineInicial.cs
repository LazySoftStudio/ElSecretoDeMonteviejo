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
    void Reset() => animator = GetComponent<Animator>();
    

    void Awake()
    {
        skipGroup = skip.GetComponent<CanvasGroup>();
    }

    void Update()
    {
        if (terminada) return;   // una vez terminada, ya no se puede pausar

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pausado = !pausado;
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
                animator.speed = 0f;
                Debug.Log("Animación terminada");
                SavePartida.Singleton.AnimacionInicialHecha();
                SceneTransition.ChangeScene(1, "fade");
            }
        }
    }

    IEnumerator Start()
    {
        yield return null;   // espera un frame para que el Animator evalúe el estado inicial

        yield return new WaitUntil(() =>
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);

        terminada = true;
        animator.speed = 0f;
        Debug.Log("Animación terminada");
        SavePartida.Singleton.AnimacionInicialHecha();
        yield return new WaitForSecondsRealtime(0.5f);
        SceneTransition.ChangeScene(1, "fade");   // al pueblo
    }

    public void QuitarPausa()
    {
        pausado = false;
        pausa.SetActive(pausado);
        animator.speed = pausado ? 0f : 1f;
    }


}

using HeyHeyThere.SceneTransitionsFree;
using System.Collections;
using UnityEngine;

public class CineInicial : MonoBehaviour
{
    [SerializeField] private Animator animator;
    public GameObject pausa;

    private bool pausado;
    private bool terminada;

    void Reset() => animator = GetComponent<Animator>();

    void Update()
    {
        if (terminada) return;   // una vez terminada, ya no se puede pausar

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pausado = !pausado;
            pausa.SetActive(pausado);
            animator.speed = pausado ? 0f : 1f;
        }
    }

    IEnumerator Start()
    {
        yield return null;   // espera un frame para que el Animator evalúe el estado inicial

        yield return new WaitUntil(() =>
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);

        terminada = true;
        animator.speed = 1f;
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

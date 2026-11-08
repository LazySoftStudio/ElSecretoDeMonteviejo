using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class BotonHoverNativo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public CanvasGroup canvasGroupTocaParaVer;
    private Coroutine corrutinaParpadeo;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (canvasGroupTocaParaVer != null)
        {
            if (corrutinaParpadeo != null) StopCoroutine(corrutinaParpadeo);
            corrutinaParpadeo = StartCoroutine(BucleParpadeo());
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (canvasGroupTocaParaVer != null)
        {
            if (corrutinaParpadeo != null) StopCoroutine(corrutinaParpadeo);
            canvasGroupTocaParaVer.alpha = 1f; // Restablecer opacidad
        }
    }

    IEnumerator BucleParpadeo()
    {
        float duracion = 0.6f;
        while (true)
        {
            // Desvanecer (Fade Out)
            yield return StartCoroutine(FadeAlpha(1f, 0.2f, duracion));
            // Aparecer (Fade In)
            yield return StartCoroutine(FadeAlpha(0.2f, 1f, duracion));
        }
    }

    IEnumerator FadeAlpha(float inicio, float fin, float tiempo)
    {
        float elap = 0f;
        while (elap < tiempo)
        {
            elap += Time.deltaTime;
            canvasGroupTocaParaVer.alpha = Mathf.Lerp(inicio, fin, elap / tiempo);
            yield return null;
        }
        canvasGroupTocaParaVer.alpha = fin;
    }
}
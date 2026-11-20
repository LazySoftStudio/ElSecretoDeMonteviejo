using UnityEngine;
using UnityEngine.UI;

public class Creditos : MonoBehaviour
{
    public ScrollRect scrollRect;
    public Button saltarAnim;
    public float velScroll = 50f;

    private bool isAutoScroll = false;
    private RectTransform contentRect;
    private RectTransform viewportRect;

    void Awake()
    {
        contentRect = scrollRect.content;
        viewportRect = scrollRect.viewport;

        if (saltarAnim != null)
            saltarAnim.onClick.AddListener(SkipCredits);
    }

    void OnEnable() // al activar el panel se pone la animacion 
    {
        animacionCreditos();
    }

    public void animacionCreditos()
    {
        scrollRect.verticalNormalizedPosition = 1f; // empieza arriba del todo

        // Bloquear el scroll manual mientras sube solo
        scrollRect.vertical = false;
        isAutoScroll = true;

        if (saltarAnim != null)  saltarAnim.gameObject.SetActive(true);
    }

    void Update()
    {
        if (!isAutoScroll) return;

        
        float maxScroll = contentRect.rect.height - viewportRect.rect.height;// calcular cuánto debe subir el contenido

        // Movemos el scroll hacia abajo (de 1 hacia 0)
        float step = (velScroll * Time.deltaTime) / maxScroll;
        scrollRect.verticalNormalizedPosition -= step;

        // Si llega al final (0 o menos), terminamos la animación
        if (scrollRect.verticalNormalizedPosition <= 0f)
        {
            scrollRect.verticalNormalizedPosition = 0f;
            FinishAutoScroll();
        }
    }

    public void SkipCredits()
    {
        // Lleva los créditos directamente al final y libera el scroll
        scrollRect.verticalNormalizedPosition = 0f;
        FinishAutoScroll();
    }

    private void FinishAutoScroll()
    {
        isAutoScroll = false;

        // Permitimos que el jugador suba y baje manualmente con la rueda o arrastrando
        scrollRect.vertical = true;

        if (saltarAnim != null) saltarAnim.gameObject.SetActive(false);
    }
}



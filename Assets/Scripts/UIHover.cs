using UnityEngine;
using UnityEngine.EventSystems;

public class UIHover : MonoBehaviour
{
    [SerializeField] private float liftAmount = 4f;   
    [SerializeField] private float speed = 12f;       

    public RectTransform rect;
    private Vector2 basePos;
    private Vector2 targetPos;

    private void Awake()
    {
        
        basePos = rect.anchoredPosition;
        targetPos = basePos;
    }

    private void OnEnable()
    {
        if (rect != null)
        {
            rect.anchoredPosition = basePos;
            targetPos = basePos;
        }
    }

    public void OnPointerEnter()
    {
        targetPos = basePos + Vector2.up * liftAmount;
        Debug.Log("Enter: " + name);
    }

    public void OnPointerExit()
    {
        targetPos = basePos;
    }

    private void Update()
    {
        rect.anchoredPosition = Vector2.Lerp(
            rect.anchoredPosition, targetPos, Time.unscaledDeltaTime * speed);
    }
}

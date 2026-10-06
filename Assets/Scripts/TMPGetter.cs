using UnityEngine;
using TMPro;
public class TMPGetter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] texts;

    public TextMeshProUGUI[] GetTexts()
    {
        return texts;
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventarioUI : MonoBehaviour
{
    public Button[] botones;
    public TextMeshProUGUI[] textos;







    private void OnGUI()
    {
        if (!botones[0]) return;
        if (!textos[0]) return;
        int mostrar = Inventory.Singleton.NItems();
        for (int i = 0; i < botones.Length; i++)
        {
            if (mostrar > 0)
            {
                botones[i].gameObject.SetActive(true);
                textos[i].gameObject.SetActive(true);
                mostrar--;
            }
            else
            {
                botones[i].gameObject.SetActive(false);
                textos[i].gameObject.SetActive(false);
            }
        }
    }
}

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventarioUI : MonoBehaviour
{
    public Button[] botones;
    public TextMeshProUGUI[] textos;


    private void OnGUI()
    {
        if (Inventory.Singleton == null || botones.Length == 0 || textos.Length == 0) return;

        List<ItemSO> actuales = Inventory.Singleton.GetInventory();

        int mostrar = Inventory.Singleton.NItems();

        for (int i = 0; i < botones.Length; i++)
        {
            if (i < mostrar)
            {
                //actializar datos
                textos[i].text = actuales[i].item.nombreItem;
                Image imagenDelBoton = botones[i].GetComponent<Image>();
                if (imagenDelBoton != null && actuales[i].item.imagen != null)
                {
                    imagenDelBoton.sprite = actuales[i].item.imagen;
                }

                botones[i].gameObject.SetActive(true);
                textos[i].gameObject.SetActive(true);
            }
            else
            {
                botones[i].gameObject.SetActive(false);
                textos[i].gameObject.SetActive(false);
            }
        }
    }
}

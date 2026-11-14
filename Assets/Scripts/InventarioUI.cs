using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventarioUI : MonoBehaviour
{
    public Button[] botones;
    public Image mostrar;
    public TextMeshProUGUI mostrarText;
    private bool mostarndoIMG = false;

    private void Awake()
    {
        foreach (Button b in botones)
        {
            b.gameObject.SetActive(false);
        }
    }


    private void OnGUI()
    {
        if (Inventory.Singleton == null || botones.Length == 0) return;

        List<ItemSO> actuales = ItemBD.Singleton.GetItems(Inventory.Singleton.GetInventoryNombres());

        int mostrar = actuales.Count;

        for (int i = 0; i < botones.Length; i++)
        {
            if (i < mostrar)
            {
                //actializar datos
                Image imagenDelBoton = botones[i].GetComponent<Image>();
                if (imagenDelBoton != null && actuales[i].item.imagen != null)
                {
                    imagenDelBoton.sprite = actuales[i].item.imagen;
                }

                botones[i].gameObject.SetActive(true);
            }
            else
            {
                botones[i].gameObject.SetActive(false);
            }
        }
    }

    public void MostrarImagen(int id)
    {
        List<ItemSO> actuales = ItemBD.Singleton.GetItems(Inventory.Singleton.GetInventoryNombres());
        ItemSO correcto = actuales[id];
        if (correcto != null)
        {
            mostrarText.text = correcto.item.nombreItem;
            mostrar.sprite = correcto.item.imagen;
            mostrar.gameObject.SetActive(true);
            mostarndoIMG = true;
        }
    }

    private void Update()
    {
        if (!mostarndoIMG) return;
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Q))
        {
            mostrar.gameObject.SetActive(false);
            mostarndoIMG = false;
        }
    }
}

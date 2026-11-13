using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Archivos: MonoBehaviour
{
    public GameObject archivo;
    public GameObject[] articulos;

    public TMP_Text botonGuardar;
    public TMP_Text nombreArticulo;
    public Color oscurecer = new Color(1, 1, 1, 0.7f); 

    private int articuloActivo = 0;

    public void Start()
    {
        ActualizarUI(); 
    }


    public void SiguienteArt()
    {
        if (articulos.Length == 0) return; 

        articulos[articuloActivo].SetActive(false);
        articuloActivo = (articuloActivo +1) % articulos.Length; 
        articulos[articuloActivo].SetActive(true);

        ActualizarUI();
    }

    public void AnteriorArt()
    {
        if (articulos.Length == 0) return; 

        articulos[articuloActivo].SetActive(false);
        articuloActivo = (articuloActivo - 1 + articulos.Length) % articulos.Length;
        articulos[articuloActivo].SetActive(true);
        
        ActualizarUI();
    }

    public void Cerrar()
    {
        archivo.SetActive(false); 
        Time.timeScale = 1f;
    }

    public void GuardarDevolver()
    {
        if (articulos.Length == 0) return;

        string nombreAct = articulos[articuloActivo].name; 
        
        if (Inventory.Singleton.HasItem(nombreAct))
        {
            Devolver();
        }
        else
        {
            Guardar(); 
        }
        ActualizarUI(); 
    }

    private void Guardar()
    {
        //ItemSO artGuardado = CrearArticulo(); 
        Inventory.Singleton.AddItem(articulos[articuloActivo].name);
    }
    private void Devolver()
    {
        Inventory.Singleton.RemoveItem(articulos[articuloActivo].name);
    }
    //se podria borrar la funcion
    private ItemSO CrearArticulo()
    {
        ItemSO nuevoItem = ScriptableObject.CreateInstance<ItemSO>();

        Image img = articulos[articuloActivo].GetComponent<Image>();
        if (img != null)
        {
            nuevoItem.item.imagen = img.sprite;
        }

        nuevoItem.item.nombreItem = articulos[articuloActivo].name;
        nuevoItem.item.type = ItemTypes.Periodico;

        return nuevoItem; 
    }

    private void ActualizarUI()
    {
        if (articulos.Length == 0 || Inventory.Singleton == null) return;

        GameObject artAct = articulos[articuloActivo];
        bool estaGuardado = Inventory.Singleton.HasItem(artAct.name);

        nombreArticulo.text = artAct.name;

        if (botonGuardar != null)
        {
            botonGuardar.text = estaGuardado ? "Devolver objeto" : "Guardar en el inventario";
        }

        Image imgComponente = artAct.GetComponent<Image>();
        if (imgComponente != null)
        {
            imgComponente.color = estaGuardado ? oscurecer : Color.white;
        }
    }
}

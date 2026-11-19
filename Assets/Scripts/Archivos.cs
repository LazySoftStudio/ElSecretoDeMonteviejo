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

    public const int MAX_ARTICULOS = 3;
    //public ConversationTemplate pensamiento; 
    // poner esta variable en global para que se comparta entre todos los archivos
    private static int nArticulos = 0;  // static para que se comparta con todos

    public AudioSource audioSource;
    public AudioClip clip;

    private int articuloActivo = 0;
    public ConversationTemplate pensamiento;

    public void Start()
    {
        nArticulos = SavePartida.Singleton.GetArchivosGuardados();
        ActualizarUI(); 
    }


    public void SiguienteArt()
    {
        if (articulos.Length == 0) return; 

        articulos[articuloActivo].SetActive(false);
        articuloActivo = (articuloActivo +1) % articulos.Length; 
        articulos[articuloActivo].SetActive(true);
        audioSource.PlayOneShot(clip);
        ActualizarUI();
    }

    public void AnteriorArt()
    {
        if (articulos.Length == 0) return; 

        articulos[articuloActivo].SetActive(false);
        articuloActivo = (articuloActivo - 1 + articulos.Length) % articulos.Length;
        articulos[articuloActivo].SetActive(true);
        audioSource.PlayOneShot(clip);
        ActualizarUI();
    }

    public void Cerrar()
    {
        archivo.SetActive(false); 
        Time.timeScale = 1f;
        GameObject player = GameObject.FindWithTag("Player");
        InteriorMOvement inm = player.GetComponent<InteriorMOvement>();
        if (inm) inm.lockMovement = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void GuardarDevolver()
    {
        if (articulos.Length == 0) return;

        string nombreAct = articulos[articuloActivo].name; 
        
        if (Inventory.Singleton.HasItem(nombreAct))
        {
            Devolver();
            nArticulos--;
            SavePartida.Singleton.SumarArchivo(-1);
        }
        else
        {
            if(nArticulos < MAX_ARTICULOS)
            {
                Guardar();
                nArticulos++;
                SavePartida.Singleton.SumarArchivo(1);
            } else
            {
                // activar aquí el dialogo de pensamiento 
                Debug.Log("Devuelve un archivo porque ya tienes 3");
                DialogueManager.Instance.DecirDialogo(pensamiento);
                Cerrar();
            }
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

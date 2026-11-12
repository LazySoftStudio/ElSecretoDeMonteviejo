using UnityEngine;

public class Archivos: MonoBehaviour
{
    public GameObject canvas; 
    public GameObject[] articulos; 

    private UIutils ui; 

    private int articuloActivo = 0;
    void Start()
    {
        ui = canvas.GetComponent<UIutils>();
    }

    void Update()
    {
        
    }

    public void siguienteArt()
    {
        ui.OpenCloseOverlayAndMouse(articulos[articuloActivo]);
        articuloActivo = (articuloActivo +1) % articulos.Length; 
        ui.OpenCloseOverlayAndMouse(articulos[articuloActivo]);
    }

    public void anteriorArt()
    {
        ui.OpenCloseOverlayAndMouse(articulos[articuloActivo]);
        articuloActivo = (articuloActivo - 1 + articulos.Length) % articulos.Length; 
        ui.OpenCloseOverlayAndMouse(articulos[articuloActivo]);
    }

}

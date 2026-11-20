using UnityEngine;

public class AbrirFosa : MonoBehaviour
{
    public GameObject cerrado;
    public GameObject abierto;

    private void Awake()
    {
        DatosPartida datos = SavePartida.Singleton.Cargar(SavePartida.Singleton.GetKey());
        if (datos == null) return;
        if (datos.animacionBosque)
        {
            cerrado.SetActive(false);
            abierto.SetActive(true);
        }
        else
        {
            cerrado.SetActive(true);
            abierto.SetActive(false);
        }
    }
}

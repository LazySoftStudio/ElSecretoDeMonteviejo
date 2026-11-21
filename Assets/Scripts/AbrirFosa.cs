using UnityEngine;

public class AbrirFosa : MonoBehaviour
{
    public GameObject cerrado;
    public GameObject abierto;

    private void Awake()
    {
        DatosPartida datos = SavePartida.Singleton.Cargar(SavePartida.Singleton.GetKey());
        if (datos == null) return;
        if (datos.animacionBosque)//fosa abierta
        {
            cerrado.SetActive(false);
            abierto.SetActive(true);
            if (!datos.reposicionarEnBosque) return;
            SavePartida.Singleton.ReposicionarBosque();
            GameObject player = GameObject.FindWithTag("Player");
            GameObject target = GameObject.FindWithTag("TargetBosque");
            if (player && target)//no son nulos
            {
                player.GetComponent<InteriorMOvement>().Teletransportar(target.transform.position, target.transform.rotation);
            }
        }
        else
        {
            cerrado.SetActive(true);
            abierto.SetActive(false);
        }
    }
}

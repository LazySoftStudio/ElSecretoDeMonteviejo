using HeyHeyThere.SceneTransitionsFree;
using UnityEngine;

public class BosqueInteract : MonoBehaviour
{
    [SerializeField] private int sceneIndex = 6;
    private void OnTriggerEnter(Collider other)
    {
        DatosPartida datos = SavePartida.Singleton.Cargar(SavePartida.Singleton.GetKey());
        if (datos.animacionBosque)
        {
            GameObject player = GameObject.FindWithTag("SalidaBosque");
            SavePartida.Singleton.Guardar(player.transform.position, player.transform.rotation, sceneIndex);
            SceneTransition.ChangeScene(sceneIndex, "diamonds");//uso del asset
        }
        else{
            GameObject player = GameObject.FindWithTag("SalidaBosque");
            SavePartida.Singleton.Guardar(player.transform.position, player.transform.rotation, 7);
            SceneTransition.ChangeScene(7, "diamonds");//uso del asset
        }
        
    }
}

using HeyHeyThere.SceneTransitionsFree;
using UnityEngine;

public class BosqueInteract : MonoBehaviour
{
    [SerializeField] private int sceneIndex = 6;
    private void OnTriggerEnter(Collider other)
    {
        if (sceneIndex == 6)// ir al bosque
        {
            GameObject player = GameObject.FindWithTag("SalidaBosque");
            SavePartida.Singleton.Guardar(player.transform.position, player.transform.rotation, 6);
            SceneTransition.ChangeScene(sceneIndex, "diamonds");//uso del asset
            return;
        }
        if (sceneIndex == -1)// estoy en el bosque y hago animacion
        {
            DatosPartida datos = SavePartida.Singleton.Cargar(SavePartida.Singleton.GetKey());
            if (!datos.animacionBosque)
            {
                gameObject.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                SceneTransition.ChangeScene(7, "diamonds");//uso del asset
            }
            else
            {
                gameObject.SetActive(false);//escondete
            }
        }
    }
}

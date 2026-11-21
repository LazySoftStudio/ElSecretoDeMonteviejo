using HeyHeyThere.SceneTransitionsFree;
using UnityEngine;

public class SalidaDelBosque : MonoBehaviour
{
    public int sceneIndex = 1;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Colision con montañas");
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Colision con montañas y player");
            GameObject player = collision.gameObject;
            SavePartida.Singleton.Guardar(player.transform.position, player.transform.rotation, sceneIndex);
            SceneTransition.ChangeScene(sceneIndex, "diamonds");//uso del asset
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Trigger con montañas y player");
            GameObject player = other.gameObject;
            SavePartida.Singleton.Guardar(player.transform.position, player.transform.rotation, sceneIndex);
            SceneTransition.ChangeScene(sceneIndex, "diamonds");//uso del asset
        }
    }
}

using HeyHeyThere.SceneTransitionsFree;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorInteract : MonoBehaviour, IInteractive
{
    [SerializeField] private int sceneIndex = 0;

    public ConversationTemplate GetDialogue()
    {
        return null;
    }

    public void Interact()
    {
        //SceneManager.LoadScene(sceneIndex);
        SceneTransition.ChangeScene(sceneIndex, "diamonds");//uso del asset
        
    }

    public void OnExit()
    {
        //nada
    }

    public void OnHover()
    {
        Debug.Log("Listo para interactuar");
    }

    
}

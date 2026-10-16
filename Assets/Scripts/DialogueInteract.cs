using UnityEngine;

public class Dialoguenteract : MonoBehaviour, IInteractive
{
    [SerializeField] private ConversationTemplate ct;

    public ConversationTemplate GetDialogue()
    {
        return ct;
    }

    public void Interact()
    {

    }

    public void OnExit()
    {
        
    }

    public void OnHover()
    {
        Debug.Log("Listo para interactuar");
    }

}

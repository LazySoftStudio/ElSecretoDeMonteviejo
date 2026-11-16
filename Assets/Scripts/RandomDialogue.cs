using UnityEngine;

public class RandomDialogue : MonoBehaviour, IInteractive
{
    public ConversationTemplate[] conversaciones;
    public ConversationTemplate GetDialogue()
    {
        return conversaciones[Random.Range(0, conversaciones.Length)];
    }

    public void Interact()
    {
        
    }

    public void OnExit()
    {
        
    }

    public void OnHover()
    {
        
    }
}

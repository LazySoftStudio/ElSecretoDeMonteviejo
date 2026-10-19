using UnityEngine;

public class ConditionalDialogue : MonoBehaviour, IInteractive
{
    [SerializeField] private string itemToCheck;
    [SerializeField] private ConversationTemplate tieneObjeto;
    [SerializeField] private ConversationTemplate noTieneObjeto;
    public ConversationTemplate GetDialogue()
    {
        if (Inventory.Singleton.HasItem(itemToCheck))
        {
            return tieneObjeto;
        }
        else
        {
            return noTieneObjeto;
        }
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

using UnityEngine;

public class CogerObjeto : MonoBehaviour, IInteractive
{
    [SerializeField] private ConversationTemplate ct;
    [SerializeField] private string itemName = "Mechero JF+E";

    private void Start()
    {
        if (Inventory.Singleton.HasItem(itemName))
        {
            gameObject.SetActive(false);
        }
    }
    public ConversationTemplate GetDialogue()
    {
        return ct;
    }

    public void Interact()
    {
        gameObject.SetActive(false);
    }

    public void OnExit()
    {

    }

    public void OnHover()
    {
        Debug.Log("Listo para interactuar");
    }
}

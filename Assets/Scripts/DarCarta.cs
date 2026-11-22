using UnityEngine;

public class DarCarta : MonoBehaviour, IInteractive
{
    [SerializeField] private string itemToCheck;
    [SerializeField] private ConversationTemplate haPasado;
    [SerializeField] private ConversationTemplate noHaPasado;
    public ConversationTemplate GetDialogue()
    {
        DatosPartida datos = SavePartida.Singleton.Cargar(SavePartida.Singleton.GetKey());
        if (datos.animacionBosque && !Inventory.Singleton.HasItem(itemToCheck))
        {
            return haPasado;   
        }
        else
        {
            return noHaPasado;
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

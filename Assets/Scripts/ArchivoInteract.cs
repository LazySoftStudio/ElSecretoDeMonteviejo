using UnityEngine;

public class ArchivoInteract : MonoBehaviour, IInteractive
{
    public GameObject panelArchivos; 
    public ConversationTemplate GetDialogue()
    {
        return null;
    }

    public void Interact()
    {
        panelArchivos.SetActive(true); 
    }

    public void OnExit()
    {
        //panelArchivos.SetActive(false);
    }

    public void OnHover()
    {
        Debug.Log("Ver archivos");
    }
}

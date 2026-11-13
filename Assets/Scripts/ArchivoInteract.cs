using UnityEngine;

public class ArchivoInteract : MonoBehaviour, IInteractive
{
    public GameObject canvas;
    private UIutils ui;
    public GameObject panelArchivos; 
    public ConversationTemplate GetDialogue()
    {
        return null;
    }

    public void Start()
    {
        ui = canvas.GetComponent<UIutils>();
    }

    public void Interact()
    {
        panelArchivos.SetActive(true);
        Time.timeScale = 0;//para juego
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        //ui.OpenCloseOverlayAndMouse(panelArchivos); 
    }

    public void OnExit()
    {
        panelArchivos.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
    }

    public void OnHover()
    {
        Debug.Log("Ver archivos");
    }
}

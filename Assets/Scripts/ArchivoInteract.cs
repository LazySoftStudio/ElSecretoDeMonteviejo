using UnityEngine;
using UnityEngine.Audio;

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
        InteriorMOvement player = GameObject.FindWithTag("Player").GetComponent<InteriorMOvement>();
        player.lockMovement = true;
        panelArchivos.SetActive(true);
        Time.timeScale = 0;//para juego
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        //ui.OpenCloseOverlayAndMouse(panelArchivos); 
    }

    public void OnExit()
    {
        InteriorMOvement player = GameObject.FindWithTag("Player").GetComponent<InteriorMOvement>();
        player.lockMovement = false;
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

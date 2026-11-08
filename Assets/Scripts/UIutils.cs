using HeyHeyThere.SceneTransitionsFree;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIutils : MonoBehaviour
{
    public bool checkUpdate = false;
    public GameObject inventario;
    public GameObject antonio;
    public GameObject pausa;
    public void ChangeScene(int sceneIndex) 
    {
        SceneTransition.ChangeScene(sceneIndex, "diamonds");//uso del asset
    }

    public void OpenCloseOverlay(GameObject overlay)
    {
        overlay.SetActive(!overlay.active);
    }


    public void OpenCloseOverlayAndMouse(GameObject overlay)
    {
        overlay.SetActive(!overlay.active);
        FirstPersonMovement fps = antonio.GetComponent<FirstPersonMovement>();
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else 
        {
            if (fps) fps.lockMovement = false;
            if (!antonio.GetComponent<FirstPersonMovement>()) antonio.GetComponent<InteriorMOvement>().lockMovement = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f;
        }
    }

    public void GuardarYSalir()
    {
        GameObject player = GameObject.FindWithTag("Player");
        SavePartida.Singleton.QuickSave(player.transform.position, player.transform.rotation, SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
        ChangeScene(4);//seleccion de partidas
    }

    private void Update()
    {
        if (!checkUpdate) return;
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (Input.GetKeyDown(KeyCode.Q) && pausa.active)
            {
                return;
            }
            if (Input.GetKeyDown(KeyCode.Escape) && inventario.active)
            {
                return;
            }

            FirstPersonMovement fps = antonio.GetComponent<FirstPersonMovement>();
            if (Time.timeScale == 0)
            {
                if(fps) fps.lockMovement = false;
                if (!antonio.GetComponent<FirstPersonMovement>()) antonio.GetComponent<InteriorMOvement>().lockMovement = false;
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    inventario.SetActive(false);
                }
                else
                {
                    pausa.SetActive(false);
                }
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Time.timeScale = 1f;
            }
            else
            {
                if (fps) fps.lockMovement = false;
                if (!antonio.GetComponent<FirstPersonMovement>()) antonio.GetComponent<InteriorMOvement>().lockMovement = true;
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    inventario.SetActive(true);
                }
                else
                {
                    pausa.SetActive(true);
                }
                Time.timeScale = 0;//para juego
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            
        }
    }

}

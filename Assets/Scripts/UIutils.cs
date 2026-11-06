using HeyHeyThere.SceneTransitionsFree;
using UnityEngine;

public class UIutils : MonoBehaviour
{
    public bool checkUpdate = false;
    public GameObject inventario;
    public GameObject antonio;
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
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else 
        {
            antonio.GetComponent<FirstPersonMovement>().lockMovement = false;
            if (!antonio.GetComponent<FirstPersonMovement>()) antonio.GetComponent<InteriorMOvement>().lockMovement = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f;
        }
    }

    private void Update()
    {
        if (!checkUpdate) return;
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (Time.timeScale == 0)
            {
                antonio.GetComponent<FirstPersonMovement>().lockMovement = false;
                if (!antonio.GetComponent<FirstPersonMovement>()) antonio.GetComponent<InteriorMOvement>().lockMovement = false;
                inventario.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Time.timeScale = 1f;
            }
            else
            {
                antonio.GetComponent<FirstPersonMovement>().lockMovement = true;
                if (!antonio.GetComponent<FirstPersonMovement>()) antonio.GetComponent<InteriorMOvement>().lockMovement = true;
                inventario.SetActive(true);
                Time.timeScale = 0;//para juego
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            
        }
    }

}

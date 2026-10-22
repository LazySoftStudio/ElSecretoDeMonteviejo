using HeyHeyThere.SceneTransitionsFree;
using UnityEngine;

public class UIutils : MonoBehaviour
{
    public void ChangeScene(int sceneIndex) 
    {
        SceneTransition.ChangeScene(sceneIndex, "diamonds");//uso del asset
    }

    public void OpenCloseOverlay(GameObject overlay)
    {
        overlay.SetActive(!overlay.active);
    }

}

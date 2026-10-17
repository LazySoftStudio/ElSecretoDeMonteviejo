using UnityEngine;
using UnityEngine.EventSystems;

namespace HeyHeyThere.SceneTransitionsFree
{
    /// <summary>
    /// The demo's EventSystem: with the Input System's UI module when that is the active input (looked
    /// up by name, so the scripts compile without the package), else the Input Manager's.
    /// </summary>
    static class DemoInput
    {
        public static void Ensure()
        {
            if (Object.FindAnyObjectByType<EventSystem>() != null)
                return;
            var go = new GameObject("EventSystem", typeof(EventSystem));
#if ENABLE_INPUT_SYSTEM
            var module = System.Type.GetType("UnityEngine.InputSystem.UI.InputSystemUIInputModule, Unity.InputSystem");
            if (module != null)
            {
                go.AddComponent(module);
                return;
            }
#endif
            go.AddComponent<StandaloneInputModule>();
        }
    }
}

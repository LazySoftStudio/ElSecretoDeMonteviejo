using UnityEngine;

public class SpinInteract : MonoBehaviour, IInteractive
{
    [SerializeField] private float spinSpeed = 45f;
    private bool spin = false;
    private Vector3 spinRot;

    [SerializeField] private ConversationTemplate ct;

    public ConversationTemplate GetDialogue()
    {
        return ct;
    }

    public void Interact()
    {
        spinRot = new Vector3(0, spinSpeed, 0);
        spin = !spin;
    }

    public void OnExit()
    {
        spin = false;
    }

    public void OnHover()
    {
        Debug.Log("Listo para interactuar");
    }

    private void Update()
    {
        if (spin)
        {
            transform.Rotate(spinRot);
        }
    }

   
}

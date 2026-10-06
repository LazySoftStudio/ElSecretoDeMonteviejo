using UnityEngine;

public class BasicInteract : MonoBehaviour
{
    public Camera cam;
    public float dist = 3.0f;
    public Color hoverColor = Color.green;

    private IInteractive current;
    private Outline r;

    void Update()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, dist);

        IInteractive closest = null;
        Collider closestCol = null;
        float bestDist = float.MaxValue;

        foreach (var col in hits)
        {
            if (col.TryGetComponent(out IInteractive i))
            {
                float d = Vector3.Distance(transform.position, col.transform.position);

                if (d < bestDist)
                {
                    bestDist = d;
                    closest = i;
                    closestCol = col;
                }
            }
        }

        if (closest != null)
        {
            if (current != closest)
            {
                Clear();
                current = closest;
                current.OnHover();

                r = closestCol.GetComponent<Outline>();  
                if (r)
                {
                    r.OutlineColor = hoverColor;
                    r.OutlineMode = Outline.Mode.OutlineVisible;
                    r.OutlineWidth = 10;
                    r.enabled = true;
                }
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                current.Interact();
                if (current.GetDialogue() != null && DialogueManager.Instance.ct == null)
                {
                    DialogueManager.Instance.DecirDialogo(current.GetDialogue());
                }
            }
        }
        else
        {
            Clear();
        }
    }

    private void Clear()
    {
        if (current != null)
        {
            current.OnExit();
        }
        if (r)
        {
            r.enabled = false;
        }
        current = null;
        r = null;
    }

    
}

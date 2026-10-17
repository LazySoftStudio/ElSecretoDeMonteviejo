using UnityEngine;

public class PlayerVisible : MonoBehaviour
{
    public LayerMask obstacleMask;
    public Camera cam;
    private Outline outline;

    void Start()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;
    }

    void Update()
    {
        Vector3 dir = cam.transform.position - transform.position;
        float dist = dir.magnitude;

        if (Physics.Raycast(transform.position, dir.normalized, out RaycastHit hit, dist, obstacleMask))
        {
            outline.enabled = true;
        }
        else
        {
            outline.enabled = false;
        }
    }
}

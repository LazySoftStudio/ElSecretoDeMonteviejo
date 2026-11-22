using UnityEngine;

public class SUbirPlanoCuerpo : MonoBehaviour
{
    public float tiempo = 5f;
    private bool pausado = false;
    private Vector3 pos;
    public GameObject pausa;

    private void Start()
    {
        pos = transform.position;
    }


    private void Update()
    {
        pausado = pausa.gameObject.activeInHierarchy;
        if (pausado) return;
        tiempo -= Time.deltaTime;

        if (tiempo <= 0)
        {
            transform.position = pos + Vector3.up*5;
        }
    }
}

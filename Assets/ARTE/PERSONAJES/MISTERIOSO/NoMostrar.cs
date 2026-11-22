using UnityEngine;

public class NoMostrar : MonoBehaviour
{
    private void Update()
    {
        //muestra solo si tiene la carta
        gameObject.SetActive(Inventory.Singleton.HasItem("CartaCerrada"));
    }
}

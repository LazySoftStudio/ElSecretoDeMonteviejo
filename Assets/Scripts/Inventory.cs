using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Singleton;
    private List<string> inventory;
    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
            inventory = new  List<string>();
            
            DontDestroyOnLoad(Singleton);
        }
        else
        {
            Destroy(gameObject);
            Debug.Log("S destruyo un inventario");
        }
    }
 

    
    public void AddItem(string newItem)
    {
        if (!HasItem(newItem))
        {
            inventory.Add(newItem);
            Debug.Log("Inventory: se añadio el objeto - " + newItem);
        } 
    }

    public void RemoveItem(string item) 
    {
        inventory.Remove(item);
        Debug.Log("Inventory: se elimino el objeto - " + item);
    }

    public bool HasItem(string itemName)
    {
        Debug.Log("Inventory: existe el objeto - " + itemName);
        return inventory.Contains(itemName);
    }

    public int NItems(){ return inventory.Count; }

    public List<string> GetInventory()
    {
        return inventory;
    }

    public List<string> GetInventoryNombres()
    {
        return inventory;
    }

    public void Restore(List<string> d)
    {
        inventory = d;
    }
}

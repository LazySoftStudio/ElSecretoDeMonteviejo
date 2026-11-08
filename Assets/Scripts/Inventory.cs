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
 

    
    public void AddItem(ItemSO newItem)
    {
        if (!HasItem(newItem.item.nombreItem))
        {
            inventory.Add(newItem.item.nombreItem);
            Debug.Log("Inventory: se añadio el objeto - " + newItem.item.nombreItem);
        } 
    }
    public void RemoveItem(ItemSO item) 
    {
        inventory.Remove(item.item.nombreItem);
        Debug.Log("Inventory: se elimino el objeto - " + item.item.nombreItem);
    }
    public void RemoveItem(string itemName)
    {
        inventory.Remove(itemName);
        Debug.Log("Inventory: se elimino el objeto - " + itemName);
    }

    public bool HasItem(string itemName)
    {
        Debug.Log("Inventory: existe el objeto - " + itemName +" _ "+ inventory.Contains(itemName).ToString());
        return inventory.Contains(itemName);
       
    }

    public int NItems()
        { return inventory.Count; }

    public List<string> GetInventory()
    {
        return inventory;
    }

    public void Restore(List<string> d)
    {
        inventory = d;
    }

}

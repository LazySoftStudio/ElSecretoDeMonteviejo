using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Singleton;
    private Dictionary<string, ItemSO> inventory;
    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
            inventory = new Dictionary<string, ItemSO>();
            DontDestroyOnLoad(Singleton);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void AddItem(ItemSO newItem)
    {
        if (!HasItem(newItem.name))
        {
            inventory.Add(newItem.item.nombreItem, newItem);
            Debug.Log("Inventory: se añadio el objeto - " + newItem.name);
        } 
    }
    public void RemoveItem(ItemSO item) 
    {
        inventory.Remove(item.item.nombreItem);
        Debug.Log("Inventory: se elimino el objeto - " + item.name);
    }
    public void RemoveItem(string itemName)
    {
        inventory.Remove(itemName);
        Debug.Log("Inventory: se elimino el objeto - " + itemName);
    }

    public bool HasItem(string itemName)
    {
        Debug.Log("Inventory: existe el objeto - " + itemName +" _ "+ inventory.ContainsKey(itemName).ToString());
        return inventory.ContainsKey(itemName);
       
    }



}

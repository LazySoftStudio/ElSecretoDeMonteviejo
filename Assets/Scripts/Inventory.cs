using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Singleton;
    private List<ItemSO> inventory;
    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
            inventory = new  List<ItemSO>();
            
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
            inventory.Add(newItem);
            Debug.Log("Inventory: se añadio el objeto - " + newItem.item.nombreItem);
        } 
    }

    public void RemoveItem(ItemSO item) 
    {
        inventory.Remove(item);
        Debug.Log("Inventory: se elimino el objeto - " + item.item.nombreItem);
    }
    public void RemoveItem(string itemName)
    {
        ItemSO itemAEliminar = inventory.Find(x => x.item.nombreItem == itemName);
        if (itemAEliminar != null)
        {
            inventory.Remove(itemAEliminar);
            Debug.Log("Inventory: se eliminó el objeto - " + itemName);
        }
    }

    public bool HasItem(string itemName)
    {
        Debug.Log("Inventory: existe el objeto - " + itemName);
        return inventory.Exists(x => x.item.nombreItem == itemName);
    }

    public int NItems(){ return inventory.Count; }

    public List<ItemSO> GetInventory()
    {
        return inventory;
    }

    public List<string> GetInventoryNombres()
    {
        List<string> nombres = new List<string>();
        foreach (ItemSO itemSO in inventory)
        {
            nombres.Add(itemSO.item.nombreItem);
        }
        return nombres;
    }

    public void Restore(List<ItemSO> d)
    {
        inventory = d;
    }
    public void Restore(List<string> nombresGuardados)
    {
        inventory.Clear();
        if (nombresGuardados == null) return;

        foreach (string nombre in nombresGuardados)
        {
            // Busca el ItemSO original en el catálogo
            ItemSO itemEncontrado = inventory.Find(x => x.item.nombreItem == nombre);
            if (itemEncontrado != null)
            {
                inventory.Add(itemEncontrado);
            }
            else
            {
                Debug.LogWarning("No se encontró en el catálogo el objeto guardado: " + nombre);
            }
        }
    }
}

using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemBD : MonoBehaviour
{
    [SerializeField] private List<ItemSO> inventoryItem;
    [SerializeField] private List<string> inventoryName;
    public static ItemBD Singleton;

// ignorar lo de abajo, es solo para asegurar que este bien
#if UNITY_EDITOR
    private void OnValidate()
    {
        if (inventoryName.Count != inventoryItem.Count)
            Debug.LogError($"{name}: inventoryName ({inventoryName.Count}) e inventoryItem ({inventoryItem.Count}) no tienen el mismo tamaño", this);
    }
#endif
// fon de lo de ignorar
    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
            DontDestroyOnLoad(Singleton);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Consigue un objeto
    public ItemSO GetItem(string name)
    {
        int index = inventoryName.IndexOf(name);
        if (index < 0)
        {
            return null;
        }
        return inventoryItem[index];
    }

    public List<ItemSO> GetItems(List<string> itemsNames)
    {
        List<ItemSO> aDevolver = new List<ItemSO>(itemsNames.Count);

        for (int i = 0; i < itemsNames.Count; i++)
        {
            ItemSO item = GetItem(itemsNames[i]);
            if (item != null)
                aDevolver.Add(item);
        }
        return aDevolver;
    }

}

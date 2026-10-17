using UnityEngine;

public enum ItemTypes
{
    Objeto,
    Periodico,
    Nombre
}

[System.Serializable]
public struct Item
{
    public Sprite imagen;
    public string nombreItem;
    public ItemTypes type;
}
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemSO : ScriptableObject
{
    public Item item;
}

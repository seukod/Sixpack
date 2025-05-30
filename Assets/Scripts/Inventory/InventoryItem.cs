using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class InventoryItem : ScriptableObject
{
    [Header("Item Properties")]
    public string itemName = "New Item";
    public Sprite icon = null;
    [TextArea(3, 10)]
    public string description = "";
    public int maxStackSize = 1;

    [HideInInspector]
    public int currentStackSize = 1;

    public bool IsStackable()
    {
        return maxStackSize > 1;
    }

    // Create a clone of this item for instancing
    public InventoryItem CreateInstance()
    {
        InventoryItem instance = Instantiate(this);
        instance.currentStackSize = 1;
        return instance;
    }
}


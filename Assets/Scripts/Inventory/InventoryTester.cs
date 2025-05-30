using UnityEngine;

public class InventoryTester : MonoBehaviour
{
    [SerializeField] private InventorySystem inventorySystem;
    [SerializeField] private InventoryItem[] testItems;
    
    private void Start()
    {
        // Verify references
        if (inventorySystem == null)
        {
            Debug.LogError("InventorySystem reference is missing!");
            return;
        }
        
        if (testItems == null || testItems.Length == 0)
        {
            Debug.LogWarning("No test items assigned!");
        }
    }
    
    public void AddRandomItem()
    {
        if (testItems == null || testItems.Length == 0)
            return;
            
        // Select a random item from the test items array
        int randomIndex = Random.Range(0, testItems.Length);
        InventoryItem itemToAdd = testItems[randomIndex];
        
        // Create a new instance to avoid modifying the original scriptable object
        InventoryItem itemInstance = itemToAdd.CreateInstance();
        
        // Randomize stack size for stackable items
        if (itemInstance.IsStackable())
        {
            itemInstance.currentStackSize = Random.Range(1, itemInstance.maxStackSize + 1);
        }
        
        // Add the item to the inventory
        bool added = inventorySystem.AddItem(itemInstance);
        
        if (added)
        {
            Debug.Log($"Added {itemInstance.itemName} x{itemInstance.currentStackSize} to inventory");
        }
        else
        {
            Debug.Log("Failed to add item - inventory might be full");
        }
    }
    
    public void ClearInventory()
    {
        inventorySystem.ClearInventory();
        Debug.Log("Inventory cleared");
    }
}


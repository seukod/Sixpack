using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    [Header("Inventory Properties")]
    [SerializeField] private int gridWidth = 4;
    [SerializeField] private int gridHeight = 4;
    
    [Header("References")]
    [SerializeField] private Transform gridContainer;
    [SerializeField] private InventorySlot slotPrefab;
    
    // List to store all inventory slots
    private List<InventorySlot> inventorySlots = new List<InventorySlot>();
    
    // Selected slot for item manipulation
    private InventorySlot selectedSlot;
    
    // Properties
    public int TotalSlots => gridWidth * gridHeight;
    public List<InventorySlot> Slots => inventorySlots;
    public InventorySlot SelectedSlot => selectedSlot;

    private void Awake()
    {
        // Make sure the grid container reference is set
        if (gridContainer == null)
            gridContainer = transform;
            
        // Initialize the inventory grid if not already initialized
        if (inventorySlots.Count == 0)
            InitializeInventoryGrid();
    }
    
    // Create the inventory grid with slots
    private void InitializeInventoryGrid()
    {
        // Clear any existing slots
        foreach (Transform child in gridContainer)
        {
            Destroy(child.gameObject);
        }
        inventorySlots.Clear();
        
        // Check if the grid container has a GridLayoutGroup component
        GridLayoutGroup gridLayout = gridContainer.GetComponent<GridLayoutGroup>();
        if (gridLayout != null)
        {
            // Set the grid layout properties if needed
            gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            gridLayout.constraintCount = gridWidth;
        }
        
        // Create the slots
        for (int i = 0; i < gridWidth * gridHeight; i++)
        {
            CreateSlot();
        }
    }
    
    // Create a single inventory slot
    private InventorySlot CreateSlot()
    {
        InventorySlot newSlot = Instantiate(slotPrefab, gridContainer);
        newSlot.name = $"Slot_{inventorySlots.Count}";
        
        // Subscribe to slot events
        newSlot.OnSlotClicked += HandleSlotClicked;
        
        // Add to our list
        inventorySlots.Add(newSlot);
        
        return newSlot;
    }
    
    // Handle when a slot is clicked
    private void HandleSlotClicked(InventorySlot clickedSlot)
    {
        // If no slot is selected, select this one
        if (selectedSlot == null)
        {
            // Only select if it has an item
            if (clickedSlot.Item != null)
            {
                selectedSlot = clickedSlot;
                // Here you could highlight the selected slot or show item details
                Debug.Log($"Selected item: {clickedSlot.Item.itemName}");
            }
        }
        else
        {
            // If the same slot is clicked again, deselect it
            if (selectedSlot == clickedSlot)
            {
                selectedSlot = null;
                Debug.Log("Slot deselected");
            }
            else
            {
                // Here you could implement item swapping, splitting, etc.
                // For now, just move the item from selected to clicked slot if possible
                MoveItemBetweenSlots(selectedSlot, clickedSlot);
                selectedSlot = null;
            }
        }
    }
    
    // Move an item between two slots
    private bool MoveItemBetweenSlots(InventorySlot fromSlot, InventorySlot toSlot)
    {
        if (fromSlot == null || fromSlot.Item == null)
            return false;
            
        // Try to add the item to the destination slot
        InventoryItem itemToMove = fromSlot.RemoveItem();
        
        if (toSlot.AddItem(itemToMove))
        {
            Debug.Log($"Moved {itemToMove.itemName} to new slot");
            return true;
        }
        else
        {
            // If we couldn't add it to the destination, put it back
            fromSlot.AddItem(itemToMove);
            Debug.Log("Couldn't move item - slot incompatible or full");
            return false;
        }
    }
    
    // Add an item to the inventory (first available slot)
    public bool AddItem(InventoryItem item)
    {
        if (item == null)
            return false;
            
        // Try to find existing stack of same item type that can fit more
        if (item.IsStackable())
        {
            foreach (InventorySlot slot in inventorySlots)
            {
                if (slot.Item != null && 
                    slot.Item.itemName == item.itemName && 
                    slot.Item.currentStackSize < slot.Item.maxStackSize)
                {
                    bool success = slot.AddItem(item);
                    if (item.currentStackSize <= 0)
                    {
                        // Item was fully added to the stack
                        return true;
                    }
                    // If we added some but not all, continue to look for more slots
                }
            }
        }
        
        // Find first empty slot
        InventorySlot emptySlot = FindFirstEmptySlot();
        
        if (emptySlot != null)
        {
            return emptySlot.AddItem(item);
        }
        
        // No available slot found
        Debug.Log("Inventory is full!");
        return false;
    }
    
    // Find the first empty slot in the inventory
    public InventorySlot FindFirstEmptySlot()
    {
        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot.Item == null)
            {
                return slot;
            }
        }
        
        return null; // No empty slots found
    }
    
    // Remove an item from the inventory by reference
    public bool RemoveItem(InventoryItem item)
    {
        if (item == null)
            return false;
            
        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot.Item == item)
            {
                slot.RemoveItem();
                return true;
            }
        }
        
        return false; // Item not found
    }
    
    // Clear the entire inventory
    public void ClearInventory()
    {
        foreach (InventorySlot slot in inventorySlots)
        {
            slot.ClearSlot();
        }
        
        selectedSlot = null;
    }
    
    // Check if inventory is full
    public bool IsInventoryFull()
    {
        return FindFirstEmptySlot() == null;
    }
    
    // Count total items in inventory
    public int CountItems()
    {
        int count = 0;
        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot.Item != null)
            {
                count++;
            }
        }
        
        return count;
    }
    
    // Find items by name
    public List<InventorySlot> FindItemsByName(string itemName)
    {
        List<InventorySlot> foundSlots = new List<InventorySlot>();
        
        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot.Item != null && slot.Item.itemName == itemName)
            {
                foundSlots.Add(slot);
            }
        }
        
        return foundSlots;
    }
}


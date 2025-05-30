using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image itemIcon;
    [SerializeField] private Image slotBackground;
    [SerializeField] private TextMeshProUGUI stackSizeText;
    
    [Header("Slot Properties")]
    [SerializeField] private Color emptySlotColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
    [SerializeField] private Color occupiedSlotColor = Color.white;

    // Item currently in this slot
    private InventoryItem _item;
    public InventoryItem Item => _item;

    // Events
    public event Action<InventorySlot> OnItemAdded;
    public event Action<InventorySlot> OnItemRemoved;
    public event Action<InventorySlot> OnSlotClicked;

    private void Awake()
    {
        // Make sure the UI references are set if not assigned in inspector
        if (itemIcon == null)
            itemIcon = transform.Find("ItemIcon")?.GetComponent<Image>();
        
        if (slotBackground == null)
            slotBackground = GetComponent<Image>();
        
        if (stackSizeText == null)
            stackSizeText = transform.Find("StackSizeText")?.GetComponent<TextMeshProUGUI>();

        // Initialize the slot as empty
        ClearSlot();
    }

    private void Start()
    {
        // Add click event listener
        var button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(() => OnSlotClicked?.Invoke(this));
        }
    }

    // Add an item to this slot
    public bool AddItem(InventoryItem newItem)
    {
        if (newItem == null)
            return false;

        // If slot is empty, just add the item
        if (_item == null)
        {
            _item = newItem;
            UpdateUI();
            OnItemAdded?.Invoke(this);
            return true;
        }
        
        // If slot already has the same item and it's stackable, increase stack
        if (_item.itemName == newItem.itemName && _item.IsStackable())
        {
            // Check if we can add to this stack
            if (_item.currentStackSize < _item.maxStackSize)
            {
                // Calculate how many can be added to this stack
                int spaceInStack = _item.maxStackSize - _item.currentStackSize;
                int amountToAdd = Mathf.Min(spaceInStack, newItem.currentStackSize);
                
                _item.currentStackSize += amountToAdd;
                newItem.currentStackSize -= amountToAdd;
                
                UpdateUI();
                OnItemAdded?.Invoke(this);
                
                // Return true if we absorbed the entire item
                return newItem.currentStackSize == 0;
            }
        }
        
        // Can't add to this slot
        return false;
    }

    // Remove the item from this slot
    public InventoryItem RemoveItem()
    {
        if (_item == null)
            return null;
            
        InventoryItem itemToReturn = _item;
        _item = null;
        
        UpdateUI();
        OnItemRemoved?.Invoke(this);
        
        return itemToReturn;
    }

    // Remove a specific quantity from this slot
    public InventoryItem RemoveQuantity(int amount)
    {
        if (_item == null || amount <= 0)
            return null;
            
        // Create a new instance to return with the specified amount
        InventoryItem splitItem = _item.CreateInstance();
        
        // Calculate actual amount to remove
        int actualAmount = Mathf.Min(amount, _item.currentStackSize);
        splitItem.currentStackSize = actualAmount;
        
        // Reduce the stack size
        _item.currentStackSize -= actualAmount;
        
        // If we removed all items, clear the slot
        if (_item.currentStackSize <= 0)
        {
            _item = null;
        }
        
        UpdateUI();
        OnItemRemoved?.Invoke(this);
        
        return splitItem;
    }

    // Clear the slot (remove item completely)
    public void ClearSlot()
    {
        _item = null;
        UpdateUI();
    }

    // Update the UI elements to reflect the current item
    private void UpdateUI()
    {
        if (_item != null)
        {
            // Show item icon
            itemIcon.sprite = _item.icon;
            itemIcon.enabled = true;
            
            // Update stack size text if applicable
            if (stackSizeText != null)
            {
                if (_item.IsStackable() && _item.currentStackSize > 1)
                {
                    stackSizeText.text = _item.currentStackSize.ToString();
                    stackSizeText.gameObject.SetActive(true);
                }
                else
                {
                    stackSizeText.gameObject.SetActive(false);
                }
            }
            
            // Update slot background color
            if (slotBackground != null)
            {
                slotBackground.color = occupiedSlotColor;
            }
        }
        else
        {
            // Slot is empty, hide item visuals
            itemIcon.sprite = null;
            itemIcon.enabled = false;
            
            if (stackSizeText != null)
            {
                stackSizeText.gameObject.SetActive(false);
            }
            
            // Update slot background color
            if (slotBackground != null)
            {
                slotBackground.color = emptySlotColor;
            }
        }
    }
}


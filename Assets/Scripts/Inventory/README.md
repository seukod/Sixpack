# Grid Inventory System

A modular 4x4 grid-based inventory system for Unity games. This system provides a flexible framework for managing items in a grid layout, with support for stacking items and basic item interactions.

![Grid Inventory Example](https://via.placeholder.com/400x300.png?text=Grid+Inventory+System)

## Features

- 4x4 grid layout (easily expandable)
- Item stacking
- Drag and drop item movement
- Flexible ScriptableObject-based item definitions
- UI feedback for item selection and interaction
- Modular architecture for easy expansion

## Setup Instructions

### Prerequisites

1. Unity 2020.3 or newer (recommended)
2. TextMeshPro package installed

### Installation

1. Clone or download this repository
2. Import the scripts and resources into your Unity project
3. Follow the setup instructions below

### Step-by-Step Setup

#### 1. Create Project Structure

Ensure you have the following folder structure in your Assets folder:

```
Assets/
├── Scripts/
│   └── Inventory/
├── Prefabs/
│   └── Inventory/
├── Sprites/
│   └── Items/
├── ScriptableObjects/
│   └── Items/
└── Scenes/
    └── Inventory/
```

#### 2. Set Up the InventorySlot Prefab

Follow the detailed instructions in `Assets/Prefabs/Inventory/InventorySlot_Setup_Instructions.txt` to create the InventorySlot prefab. Key steps include:

1. Create a UI Image GameObject
2. Add child elements for item icon and stack size
3. Add Button component for interaction
4. Add the InventorySlot script and set references
5. Save as a prefab in the Prefabs/Inventory folder

#### 3. Create Test Items

1. Right-click in the Project window > Create > Inventory > Item
2. Set item properties:
   - Name
   - Description
   - Max stack size
   - Icon (create or import a simple sprite)
3. Create at least one test item

#### 4. Set Up the Inventory Scene

Follow the detailed guide in `Assets/Scenes/Inventory/InventoryScene_Setup_Guide.txt`. Key steps include:

1. Create a new scene
2. Set up a Canvas with proper scaling
3. Create a grid container with GridLayoutGroup
4. Add the InventorySystem script to an empty GameObject
5. Configure the InventorySystem references
6. Add test buttons and the InventoryTester script

## Quick Start Guide

After completing the setup:

1. Open the InventoryScene
2. Press Play in the Unity Editor
3. Click the "Add Random Item" button to add items to the inventory
4. Click on items to select them
5. Click on empty slots or other items to move the selected item
6. Use "Clear Inventory" to reset the inventory

## Scripts Overview

- **InventoryItem.cs**: ScriptableObject defining item properties
- **InventorySlot.cs**: Manages individual inventory slots and item interactions
- **InventorySystem.cs**: Manages the entire inventory grid and item placement
- **InventoryTester.cs**: Provides testing functionality for the inventory system

## Extending the System

The inventory system is designed to be modular and extensible. Here are some ways to enhance it:

1. **Add Drag and Drop**: Implement IBeginDragHandler, IDragHandler, and IEndDragHandler interfaces
2. **Add Item Categories**: Extend the InventoryItem class with type/category properties
3. **Add Item Use Functionality**: Implement item use methods and events
4. **Save/Load System**: Add serialization to save inventory state
5. **Visual Enhancements**: Add animations, tooltips, and better visual feedback

## Troubleshooting

### Common Issues

1. **Items not appearing in slots**:
   - Check that item icons are assigned
   - Verify the ItemIcon Image component is properly referenced
   - Check console for errors

2. **Slot interactions not working**:
   - Ensure the EventSystem is present in the scene
   - Check that Button components are working
   - Verify that raycast targets are enabled on UI elements

3. **Items not stacking properly**:
   - Check that items have the same itemName
   - Verify maxStackSize is set correctly
   - Debug stackSize values during runtime

4. **Compiler errors**:
   - Ensure all scripts are in the correct namespace
   - Check for missing TextMeshPro package if related errors appear
   - Verify proper C# syntax and Unity version compatibility

### Debug Tips

- Add debug logs to track item movement and interactions
- Use the Unity Inspector to check component values during runtime
- Test with simple, distinctive item sprites to easily identify items

## License

This inventory system is provided under the MIT License. Feel free to use and modify it for your projects.

## Credits

Developed as part of the Grid Inventory System tutorial for Unity.

---

For additional questions or issues, please refer to the documentation or create an issue in the repository.


using Inventory.Core;
using Inventory.Data;
using UnityEngine;
namespace Inventory.Managers
{
    /// <summary>
    /// Responsibility:
    /// Performs every inventory operation.
    ///
    /// InventoryManager is the ONLY class allowed
    /// to modify inventories.
    /// </summary>
    public static class InventoryManager
    {
        public static bool Move(
    InventoryComponent fromInventory,
    int fromSlot,
    InventoryComponent toInventory,
    int toSlot)
        {
            Debug.Log("Move() called");
            // Validate inventories.
            if (fromInventory == null || toInventory == null)
                return false;

            // Validate slot indices.
            if (fromSlot < 0 || fromSlot >= fromInventory.Slots.Length)
                return false;

            if (toSlot < 0 || toSlot >= toInventory.Slots.Length)
                return false;

            InventorySlot source = fromInventory.Slots[fromSlot];
            InventorySlot destination = toInventory.Slots[toSlot];

            // Nothing to move.
            if (source.IsEmpty)
                return false;

            bool inventoryChanged = false;

            // Destination is empty -> Move.
            if (destination.IsEmpty)
            {
                destination.Set(source.ItemInstance, source.Quantity);
                source.Clear();

                inventoryChanged = true;
            }
            // Destination contains an item.
            else if (source.ItemInstance.CanStackWith(destination.ItemInstance))
            {
                int maxStack = source.ItemInstance.ItemData.MaxStackSize;
                int totalItems = source.Quantity + destination.Quantity;

                // Entire stack fits.
                if (totalItems <= maxStack)
                {
                    destination.Quantity = totalItems;
                    source.Clear();
                }
                // Destination fills up, source keeps the remainder.
                else
                {
                    destination.Quantity = maxStack;
                    source.Quantity = totalItems - maxStack;
                }

                inventoryChanged = true;
            }
            // Swap.
            else
            {
                ItemInstance tempItem = destination.ItemInstance;
                int tempQuantity = destination.Quantity;

                destination.Set(source.ItemInstance, source.Quantity);
                source.Set(tempItem, tempQuantity);

                inventoryChanged = true;
            }

            if (inventoryChanged)
            {
                fromInventory.NotifyInventoryChanged();

                // Avoid firing twice when moving within the same inventory.
                if (fromInventory != toInventory)
                {
                    toInventory.NotifyInventoryChanged();
                }
            }

            return inventoryChanged;
        }
    }
}
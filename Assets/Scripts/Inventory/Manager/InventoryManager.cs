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
        /// <summary>
        /// Moves the entire stack.
        /// </summary>
        public static bool Move(
            InventoryComponent fromInventory,
            int fromSlot,
            InventoryComponent toInventory,
            int toSlot)
        {
            // Validate inventories.
            if (fromInventory == null || toInventory == null)
                return false;

            if (fromSlot < 0 || fromSlot >= fromInventory.Slots.Length)
                return false;

            InventorySlot source = fromInventory.Slots[fromSlot];

            return Move(
                fromInventory,
                fromSlot,
                toInventory,
                toSlot,
                source.Quantity);
        }

        /// <summary>
        /// Moves only the specified quantity.
        /// </summary>
        public static bool Move(
            InventoryComponent fromInventory,
            int fromSlot,
            InventoryComponent toInventory,
            int toSlot,
            int quantity)
        {
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

            // Clamp quantity.
            quantity = Mathf.Clamp(quantity, 1, source.Quantity);

            bool inventoryChanged = false;

            // -------------------------------------------------
            // Destination is empty.
            // -------------------------------------------------
            if (destination.IsEmpty)
            {
                destination.Set(source.ItemInstance, quantity);

                source.Quantity -= quantity;

                if (source.Quantity == 0)
                {
                    source.Clear();
                }

                inventoryChanged = true;
            }

            // -------------------------------------------------
            // Merge.
            // -------------------------------------------------
            else if (source.ItemInstance.CanStackWith(destination.ItemInstance))
            {
                int maxStack = source.ItemInstance.ItemData.MaxStackSize;

                int spaceAvailable =
                    maxStack - destination.Quantity;

                if (spaceAvailable > 0)
                {
                    int amountToMove =
                        Mathf.Min(quantity, spaceAvailable);

                    destination.Quantity += amountToMove;

                    source.Quantity -= amountToMove;

                    if (source.Quantity == 0)
                    {
                        source.Clear();
                    }

                    inventoryChanged = true;
                }
            }

            // -------------------------------------------------
            // Swap.
            // -------------------------------------------------
            else
            {
                // Partial stacks cannot be swapped.
                if (quantity != source.Quantity)
                    return false;

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
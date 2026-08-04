using Inventory.Core;
using Inventory.Data;

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

            // Destination is empty -> Move.
            if (destination.IsEmpty)
            {
                destination.Set(source.ItemInstance, source.Quantity);
                source.Clear();

                return true;
            }

            // Destination contains an item.
            if (source.ItemInstance.CanStackWith(destination.ItemInstance))
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

                return true;
            }

            //swaps if no other option

            ItemInstance tempItem = destination.ItemInstance;
            int tempQuantity = destination.Quantity;

            destination.Set(source.ItemInstance, source.Quantity);

            source.Set(tempItem, tempQuantity);

            return true;

        }
    }
}
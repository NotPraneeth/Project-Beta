namespace Inventory.Data
{
    /// <summary>
    /// Responsibility:
    /// Represents one inventory slot.
    ///
    /// A slot stores one ItemInstance and the quantity
    /// of that instance.
    /// </summary>
    public sealed class InventorySlot
    {
        public ItemInstance ItemInstance { get; set; }

        public int Quantity { get; set; }

        public bool IsEmpty => ItemInstance == null;

        public InventorySlot()
        {
            ItemInstance = null;
            Quantity = 0;
        }

        public void Clear()
        {
            ItemInstance = null;
            Quantity = 0;
        }

        public void Set(ItemInstance itemInstance, int quantity)
        {
            ItemInstance = itemInstance;
            Quantity = quantity;
        }
    }
}
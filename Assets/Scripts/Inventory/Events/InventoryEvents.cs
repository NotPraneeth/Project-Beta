using System;

namespace Inventory.Events
{
    /// <summary>
    /// Responsibility:
    /// Publishes inventory-related events.
    /// </summary>
    public sealed class InventoryEvents
    {
        /// <summary>
        /// Raised whenever the inventory contents change.
        /// </summary>
        public event Action InventoryChanged;

        public void RaiseInventoryChanged()
        {
            InventoryChanged?.Invoke();
        }
    }
}
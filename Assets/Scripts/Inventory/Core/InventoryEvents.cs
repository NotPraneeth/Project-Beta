using System;

namespace Inventory.Core
{
    /// <summary>
    /// Responsibility:
    /// Contains events raised by an Inventory.
    ///
    /// InventoryManager performs modifications.
    /// Inventory raises notifications.
    /// </summary>
    public sealed class InventoryEvents
    {
        /// <summary>
        /// Raised whenever any slot changes.
        /// </summary>
        public event Action InventoryChanged;

        internal void RaiseInventoryChanged()
        {
            InventoryChanged?.Invoke();
        }
    }
}
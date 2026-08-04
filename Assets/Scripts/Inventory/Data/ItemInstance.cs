using UnityEngine;

namespace Inventory.Data
{
    /// <summary>
    /// Responsibility:
    /// Represents one runtime instance of an item.
    ///
    /// ItemInstance stores runtime properties that may differ
    /// between two copies of the same ItemData.
    ///
    /// It intentionally does NOT store quantity or inventory
    /// location.
    /// </summary>
    public sealed class ItemInstance
    {
        /// <summary>
        /// Immutable definition of this item.
        /// </summary>
        public ItemData ItemData { get; }

        /// <summary>
        /// Runtime quality of this item.
        /// </summary>
        public ItemQuality Quality { get; }

        public ItemInstance(ItemData itemData, ItemQuality quality)
        {
            ItemData = itemData;
            Quality = quality;
        }

        public bool CanStackWith(ItemInstance other)
        {
            if (other == null)
                return false;

            return ItemData == other.ItemData &&
                   Quality == other.Quality;
        }
    }
}
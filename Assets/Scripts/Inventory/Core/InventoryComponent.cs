using Inventory.Data;
using Inventory.UI;
using UnityEngine;
using Inventory.Events;

namespace Inventory.Core
{
    /// <summary>
    /// Responsibility:
    /// Represents an inventory attached to a GameObject.
    ///
    /// InventoryComponent owns a collection of inventory slots.
    /// It intentionally contains no gameplay logic.
    /// All modifications are performed through InventoryManager.
    /// </summary>
    public sealed class InventoryComponent : MonoBehaviour
    {
        [SerializeField]
        private int slotCount = 10;

        // Temporary testing data.
        // Remove this once pickups and crafting are implemented.
        [SerializeField]
        private StaticItemData[] testItems;

        public InventorySlot[] Slots { get; private set; }
        public InventoryEvents Events { get; } = new();

        private void Awake()
        {
            Slots = new InventorySlot[slotCount];

            for (int i = 0; i < slotCount; i++)
            {
                Slots[i] = new InventorySlot();
            }
        }

        private void Start()
        {
            PopulateTestInventory();
        }

        /// <summary>
        /// Populates the inventory with test items.
        /// Used only during development.
        /// </summary>
        private void PopulateTestInventory()
        {
            if (testItems == null || testItems.Length == 0)
                return;


            // Clear all slots first.
            foreach (InventorySlot slot in Slots)
            {
                slot.Clear();
            }

            int count = Mathf.Min(testItems.Length, Slots.Length);

            for (int i = 0; i < count; i++)
            {
                ItemInstance item = new ItemInstance(
                    testItems[i],
                    ItemQuality.None);

                Slots[i].Set(
                    item,
                    Random.Range(1, testItems[i].MaxStackSize + 1));
                Debug.Log($"Added {item.ItemData.DisplayName} to slot {i}");
            }
            NotifyInventoryChanged();
        }
        public void NotifyInventoryChanged()
        {
            Events.RaiseInventoryChanged();
        }
    }
}
using System.Collections.Generic;
using UnityEngine;

using Inventory.Core;

namespace Inventory.UI
{
    /// <summary>
    /// Displays an InventoryComponent.
    /// Can be reused for the player, shelves,
    /// vehicles, chests, etc.
    /// </summary>
    public sealed class InventoryUI : MonoBehaviour
    {
        [SerializeField]
        private InventoryComponent inventory;

        public InventoryComponent Inventory => inventory;

        [SerializeField]
        private InventorySlotUI slotPrefab;

        [SerializeField]
        private Transform slotParent;

        [SerializeField]
        private DragController dragController;

        private readonly List<InventorySlotUI> slots = new();

        private void Start()
        {
            BuildSlots();

            Subscribe();

            Refresh();
        }

        private void OnDestroy()
        {
            Unsubscribe();
        }

        /// <summary>
        /// Creates the UI slots for the current inventory.
        /// </summary>
        private void BuildSlots()
        {
            foreach (InventorySlotUI slot in slots)
            {
                Destroy(slot.gameObject);
            }

            slots.Clear();

            if (inventory == null)
                return;

            for (int i = 0; i < inventory.Slots.Length; i++)
            {
                InventorySlotUI slot =
                    Instantiate(slotPrefab, slotParent);

                slot.Initialize(this, i);

                slots.Add(slot);
            }
        }

        private void Subscribe()
        {
            if (inventory != null)
            {
                inventory.Events.InventoryChanged += Refresh;
            }
        }

        private void Unsubscribe()
        {
            if (inventory != null)
            {
                inventory.Events.InventoryChanged -= Refresh;
            }
        }

        /// <summary>
        /// Displays a different inventory.
        /// </summary>
        public void SetInventory(
            InventoryComponent newInventory)
        {
            if (inventory == newInventory)
                return;

            Unsubscribe();

            inventory = newInventory;

            BuildSlots();

            Subscribe();

            Refresh();
        }

        /// <summary>
        /// Refreshes every UI slot.
        /// </summary>
        public void Refresh()
        {
            if (inventory == null)
                return;

            for (int i = 0; i < inventory.Slots.Length; i++)
            {
                slots[i].Refresh(inventory.Slots[i]);
            }
        }

        /// <summary>
        /// Begins dragging an item.
        /// </summary>
        public void BeginDrag(int slotIndex)
        {
            dragController.BeginDrag(
                inventory,
                slotIndex);
        }
    }
}
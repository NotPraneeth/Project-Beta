using System.Collections.Generic;
using UnityEngine;
using Inventory.Core;

namespace Inventory.UI
{
    /// <summary>
    /// Responsibility:
    /// Displays an InventoryComponent.
    ///
    /// InventoryUI never edits the inventory directly.
    /// It simply displays it and forwards user actions
    /// to InventoryManager.
    /// </summary>
    public sealed class InventoryUI : MonoBehaviour
    {
        [SerializeField] private InventoryComponent inventory;

        [SerializeField] private InventorySlotUI slotPrefab;

        [SerializeField] private Transform slotParent;

        private readonly List<InventorySlotUI> slots = new();

        private int selectedSlot = -1;

        private void Start()
        {
            Debug.Log("InventoryUI Start");

            for (int i = 0; i < inventory.Slots.Length; i++)
            {
                InventorySlotUI slot =
                    Instantiate(slotPrefab, slotParent);

                slot.Initialize(this, i);

                slots.Add(slot);
            }

            Debug.Log("Calling Refresh");

            Refresh();
        }

        /// <summary>
        /// Refreshes every slot in the inventory UI.
        /// </summary>
        public void Refresh()
        {
            for (int i = 0; i < inventory.Slots.Length; i++)
            {
                slots[i].Refresh(inventory.Slots[i]);
            }
        }

        /// <summary>
        /// Handles slot selection.
        /// Two clicks perform a move.
        /// </summary>
        public void OnSlotClicked(int slotIndex)
        {
            if (selectedSlot == -1)
            {
                selectedSlot = slotIndex;
                return;
            }

            Inventory.Managers.InventoryManager.Move(
                inventory,
                selectedSlot,
                inventory,
                slotIndex);

            selectedSlot = -1;

            Refresh();
        }
    }
}
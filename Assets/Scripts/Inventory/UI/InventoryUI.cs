using System.Collections.Generic;
using UnityEngine;

using Inventory.Core;

namespace Inventory.UI
{
    /// <summary>
    /// Displays an InventoryComponent.
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
            for (int i = 0; i < inventory.Slots.Length; i++)
            {
                InventorySlotUI slot = Instantiate(slotPrefab, slotParent);

                slot.Initialize(this, i);

                slots.Add(slot);
            }

            inventory.Events.InventoryChanged += Refresh;

            Refresh();
        }

        private void OnDestroy()
        {
            if (inventory != null)
            {
                inventory.Events.InventoryChanged -= Refresh;
            }
        }

        public void Refresh()
        {
            for (int i = 0; i < inventory.Slots.Length; i++)
            {
                slots[i].Refresh(inventory.Slots[i]);
            }
        }

        public void BeginDrag(int slotIndex)
        {
            dragController.BeginDrag(inventory, slotIndex);
        }
    }
}
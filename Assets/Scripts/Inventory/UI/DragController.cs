using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

using Inventory.Core;
using Inventory.Managers;

namespace Inventory.UI
{
    public sealed class DragController : MonoBehaviour
    {
        [SerializeField]
        private Image icon;

        [SerializeField]
        private TMP_Text quantityText;

        private InventoryComponent sourceInventory;
        private int sourceSlot;

        private bool isDragging;

        private void Awake()
        {
            icon.enabled = false;
            quantityText.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (!isDragging)
                return;

            transform.position = Mouse.current.position.ReadValue();

            if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                TryDrop();
            }
        }

        public void BeginDrag(
            InventoryComponent inventory,
            int slotIndex)
        {
            Inventory.Data.InventorySlot slot = inventory.Slots[slotIndex];

            if (slot.IsEmpty)
                return;

            sourceInventory = inventory;
            sourceSlot = slotIndex;

            icon.sprite = slot.ItemInstance.ItemData.Icon;
            icon.enabled = true;

            quantityText.text = slot.Quantity.ToString();
            quantityText.gameObject.SetActive(slot.Quantity > 1);

            isDragging = true;
        }

        private void TryDrop()
        {
            PointerEventData pointer =
                new PointerEventData(EventSystem.current);

            pointer.position = Mouse.current.position.ReadValue();

            List<RaycastResult> hits = new();

            EventSystem.current.RaycastAll(pointer, hits);

            Debug.Log($"Raycast Hits: {hits.Count}");

            foreach (RaycastResult hit in hits)
            {
                Debug.Log(hit.gameObject.name);

                InventorySlotUI slot =
                    hit.gameObject.GetComponentInParent<InventorySlotUI>();

                if (slot == null)
                    continue;

                Debug.Log($"Found Slot {slot.SlotIndex}");

                InventoryUI inventoryUI =
                    slot.GetComponentInParent<InventoryUI>();

                InventoryManager.Move(
                    sourceInventory,
                    sourceSlot,
                    inventoryUI.Inventory,
                    slot.SlotIndex);

                break;
            }

            EndDrag();
        }

        private void EndDrag()
        {
            isDragging = false;

            sourceInventory = null;
            sourceSlot = -1;

            icon.enabled = false;
            quantityText.gameObject.SetActive(false);
        }
    }
}
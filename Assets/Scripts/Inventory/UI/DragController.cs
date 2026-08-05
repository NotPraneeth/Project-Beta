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
        private int quantityToMove;

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

            float scroll = Mouse.current.scroll.ReadValue().y;

            if (scroll > 0)
            {
                IncreaseQuantity();
            }
            else if (scroll < 0)
            {
                DecreaseQuantity();
            }

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

            quantityToMove = slot.Quantity;

            UpdateDragUI();

            isDragging = true;
        }

        private void TryDrop()
        {
            PointerEventData pointer =
                new PointerEventData(EventSystem.current);

            pointer.position = Mouse.current.position.ReadValue();

            List<RaycastResult> hits = new();

            EventSystem.current.RaycastAll(pointer, hits);

            foreach (RaycastResult hit in hits)
            {
                InventorySlotUI slot =
                    hit.gameObject.GetComponentInParent<InventorySlotUI>();

                if (slot == null)
                    continue;

                InventoryUI inventoryUI =
                    slot.GetComponentInParent<InventoryUI>();

                Debug.Log($"Slot: {slot.name}");
                Debug.Log($"InventoryUI: {inventoryUI}");

                if (inventoryUI != null)
                {
                    Debug.Log($"Inventory: {inventoryUI.Inventory}");
                }

                InventoryManager.Move(
                    sourceInventory,
                    sourceSlot,
                    inventoryUI.Inventory,
                    slot.SlotIndex,
                    quantityToMove);

                break;
            }

            EndDrag();
        }

        private void EndDrag()
        {
            isDragging = false;

            sourceInventory = null;
            sourceSlot = -1;
            quantityToMove = 0;

            icon.enabled = false;
            quantityText.gameObject.SetActive(false);
        }

        private void UpdateDragUI()
        {
            quantityText.text = quantityToMove.ToString();
            quantityText.gameObject.SetActive(quantityToMove > 1);
        }

        private void IncreaseQuantity()
        {
            Inventory.Data.InventorySlot slot =
                sourceInventory.Slots[sourceSlot];

            if (quantityToMove >= slot.Quantity)
                return;

            quantityToMove++;

            UpdateDragUI();
        }

        private void DecreaseQuantity()
        {
            if (quantityToMove <= 1)
                return;

            quantityToMove--;

            UpdateDragUI();
        }
    }
}
//using System;
//using System.Collections.Generic;
//using UnityEngine;

///// <summary>
///// Represents a fixed-size inventory that stores items in slots.
///// </summary>
//public class Inventory
//{
//    //============================================================
//    // Events
//    //============================================================

//    /// <summary>
//    /// Invoked whenever the inventory contents change.
//    /// </summary>
//    public event Action OnInventoryChanged;

//    //============================================================
//    // Fields & Properties
//    //============================================================

//    private readonly List<InventorySlot> slots;

//    public IReadOnlyList<InventorySlot> Slots => slots;

//    public int Capacity => slots.Count;

//    //============================================================
//    // Constructor
//    //============================================================

//    public Inventory(int capacity)
//    {
//        capacity = Mathf.Max(1, capacity);

//        slots = new List<InventorySlot>(capacity);

//        for (int i = 0; i < capacity; i++)
//        {
//            slots.Add(new InventorySlot());
//        }
//    }

//    //============================================================
//    // Public Methods - Item Management
//    //============================================================

//    /// <summary>
//    /// Adds an item to the inventory.
//    /// Returns the quantity that could not be added.
//    /// </summary>
//    public int AddItem(ItemData item, int quantity)
//    {
//        if (item == null)
//            return quantity;

//        if (quantity <= 0)
//            return quantity;

//        int originalQuantity = quantity;

//        while (quantity > 0)
//        {
//            int stackIndex = FindStackableSlot(item);

//            if (stackIndex != -1)
//            {
//                InventorySlot slot = slots[stackIndex];

//                int amountToAdd = Mathf.Min(slot.RemainingSpace, quantity);

//                slot.AddQuantity(amountToAdd);
//                quantity -= amountToAdd;

//                continue;
//            }

//            int emptyIndex = FindEmptySlot();

//            if (emptyIndex == -1)
//                break;

//            int amount = item.Stackable
//                ? Mathf.Min(item.MaxStack, quantity)
//                : 1;

//            slots[emptyIndex].Set(item, amount);

//            quantity -= amount;
//        }

//        if (quantity != originalQuantity)
//            NotifyInventoryChanged();

//        return quantity;
//    }

//    /// <summary>
//    /// Removes an item from the inventory.
//    /// Returns the quantity that could not be removed.
//    /// </summary>
//    public int RemoveItem(ItemData item, int quantity)
//    {
//        if (item == null)
//            return quantity;

//        if (quantity <= 0)
//            return quantity;

//        int originalQuantity = quantity;

//        while (quantity > 0)
//        {
//            int slotIndex = FindItemSlot(item);

//            if (slotIndex == -1)
//                break;

//            InventorySlot slot = slots[slotIndex];

//            int amountToRemove = Mathf.Min(slot.Quantity, quantity);

//            slot.RemoveQuantity(amountToRemove);

//            quantity -= amountToRemove;
//        }

//        if (quantity != originalQuantity)
//            NotifyInventoryChanged();

//        return quantity;
//    }

//    //============================================================
//    // Public Methods - Slot Management
//    //============================================================

//    /// <summary>
//    /// Moves an item between two slots.
//    /// Handles moving, merging and swapping automatically.
//    /// </summary>
//    public bool MoveItem(int sourceIndex, int destinationIndex)
//    {
//        if (!IsValidSlot(sourceIndex) || !IsValidSlot(destinationIndex))
//            return false;

//        if (sourceIndex == destinationIndex)
//            return false;

//        InventorySlot source = slots[sourceIndex];
//        InventorySlot destination = slots[destinationIndex];

//        if (source.IsEmpty)
//            return false;

//        bool changed;

//        if (destination.IsEmpty)
//            changed = MoveToEmptySlot(source, destination);
//        else if (source.Item == destination.Item)
//            changed = MergeSlots(source, destination);
//        else
//            changed = SwapSlots(source, destination);

//        if (changed)
//            NotifyInventoryChanged();

//        return changed;
//    }

//    //============================================================
//    // Public Methods - Utility
//    //============================================================

//    /// <summary>
//    /// Checks whether the inventory contains at least the specified quantity
//    /// of the given item.
//    /// </summary>
//    public bool Contains(ItemData item, int quantity)
//    {
//        int total = 0;

//        foreach (InventorySlot slot in slots)
//        {
//            if (slot.IsEmpty)
//                continue;

//            if (slot.Item != item)
//                continue;

//            total += slot.Quantity;

//            if (total >= quantity)
//                return true;
//        }

//        return false;
//    }

//    /// <summary>
//    /// Returns the total quantity of the specified item in the inventory.
//    /// </summary>
//    public int GetItemCount(ItemData item)
//    {
//        int total = 0;

//        foreach (InventorySlot slot in slots)
//        {
//            if (!slot.IsEmpty && slot.Item == item)
//                total += slot.Quantity;
//        }

//        return total;
//    }

//    /// <summary>
//    /// Removes every item from the inventory.
//    /// </summary>
//    public void Clear()
//    {
//        bool changed = false;

//        foreach (InventorySlot slot in slots)
//        {
//            if (slot.IsEmpty)
//                continue;

//            slot.Clear();
//            changed = true;
//        }

//        if (changed)
//            NotifyInventoryChanged();
//    }

//    //============================================================
//    // Public Methods - Inventory Transfer
//    //============================================================

//    /// <summary>
//    /// Transfers the contents of a slot from this inventory to another inventory.
//    /// The destination inventory will automatically merge with existing stacks
//    /// or use empty slots when available.
//    ///
//    /// Returns the number of items successfully transferred.
//    /// </summary>
//    /// <param name="destination">
//    /// The inventory that will receive the item(s).
//    /// </param>
//    /// <param name="sourceSlotIndex">
//    /// The slot index in this inventory to transfer.
//    /// </param>
//    public int TransferSlotTo(Inventory destination, int sourceSlotIndex)
//    {
//        if (destination == null)
//            return 0;

//        if (destination == this)
//            return 0;

//        if (!IsValidSlot(sourceSlotIndex))
//            return 0;

//        InventorySlot source = slots[sourceSlotIndex];

//        if (source.IsEmpty)
//            return 0;

//        int originalQuantity = source.Quantity;

//        int leftover = destination.AddItem(source.Item, originalQuantity);

//        int transferred = originalQuantity - leftover;

//        source.RemoveQuantity(transferred);

//        if (transferred > 0)
//            NotifyInventoryChanged();

//        return transferred;
//    }

//    //============================================================
//    // Private Helpers - Validation
//    //============================================================

//    /// <summary>
//    /// Checks whether a slot index is valid.
//    /// </summary>
//    private bool IsValidSlot(int index)
//    {
//        return index >= 0 && index < slots.Count;
//    }

//    //============================================================
//    // Private Helpers - Searching
//    //============================================================

//    /// <summary>
//    /// Finds the first empty slot.
//    /// Returns -1 if none exist.
//    /// </summary>
//    private int FindEmptySlot()
//    {
//        for (int i = 0; i < slots.Count; i++)
//        {
//            if (slots[i].IsEmpty)
//                return i;
//        }

//        return -1;
//    }

//    /// <summary>
//    /// Finds the first stack that can accept more of the specified item.
//    /// Returns -1 if no valid stack exists.
//    /// </summary>
//    private int FindStackableSlot(ItemData item)
//    {
//        for (int i = 0; i < slots.Count; i++)
//        {
//            if (slots[i].CanStack(item))
//                return i;
//        }

//        return -1;
//    }

//    /// <summary>
//    /// Finds the first slot containing the specified item.
//    /// Returns -1 if the item does not exist.
//    /// </summary>
//    private int FindItemSlot(ItemData item)
//    {
//        for (int i = 0; i < slots.Count; i++)
//        {
//            if (!slots[i].IsEmpty && slots[i].Item == item)
//                return i;
//        }

//        return -1;
//    }

//    //============================================================
//    // Private Helpers - Slot Operations
//    //============================================================

//    /// <summary>
//    /// Moves the source slot into an empty destination slot.
//    /// </summary>
//    private bool MoveToEmptySlot(InventorySlot source, InventorySlot destination)
//    {
//        destination.Set(source.Item, source.Quantity);
//        source.Clear();

//        return true;
//    }

//    /// <summary>
//    /// Swaps the contents of two slots.
//    /// </summary>
//    private bool SwapSlots(InventorySlot first, InventorySlot second)
//    {
//        ItemData tempItem = first.Item;
//        int tempQuantity = first.Quantity;

//        first.Set(second.Item, second.Quantity);
//        second.Set(tempItem, tempQuantity);

//        return true;
//    }

//    /// <summary>
//    /// Attempts to merge the source stack into the destination stack.
//    /// </summary>
//    private bool MergeSlots(InventorySlot source, InventorySlot destination)
//    {
//        if (!destination.CanStack(source.Item))
//            return false;

//        int amountToMove = Mathf.Min(destination.RemainingSpace, source.Quantity);

//        destination.AddQuantity(amountToMove);
//        source.RemoveQuantity(amountToMove);

//        return amountToMove > 0;
//    }

//    //============================================================
//    // Private Helpers - Events
//    //============================================================

//    /// <summary>
//    /// Notifies listeners that the inventory contents have changed.
//    /// </summary>
//    private void NotifyInventoryChanged()
//    {
//        OnInventoryChanged?.Invoke();
//    }
//}
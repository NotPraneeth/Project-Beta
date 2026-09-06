using Inventory.Core;
using Player;
using UI;
using UnityEngine;

public sealed class StorageInteractable : MonoBehaviour, IInteractable
{
    [SerializeField]
    private InventoryComponent inventory;

    [SerializeField]
    private GameUIManager gameUIManager;

    public string InteractionPrompt => "Open Storage";

    public Transform InteractionPoint => transform;

    public void Interact(PlayerInteractor interactor)
    {
        gameUIManager.ToggleStorage(inventory);
    }
}
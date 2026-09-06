using Player;
using UnityEngine;

public interface IInteractable
{
    string InteractionPrompt { get; }

    Transform InteractionPoint { get; }

    void Interact(PlayerInteractor interactor);
}
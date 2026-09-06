using Player;
using UnityEngine;

public class InteractableCube : MonoBehaviour, IInteractable
{
    public string InteractionPrompt => "Interact";

    public Transform InteractionPoint => transform;

    public void Interact(PlayerInteractor interactor)
    {
        Debug.Log("Interact!");
    }
}
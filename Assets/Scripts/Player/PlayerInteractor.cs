using UI;
using UnityEngine;

[RequireComponent(typeof(PlayerInputHandler))]
public class PlayerInteractor : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private float interactionRadius = 0.5f;

    [SerializeField] private Transform interactionPoint;
    [SerializeField] private LayerMask interactableLayer;

    [Header("References")]
    [SerializeField] private GameUIManager gameUIManager;

    private readonly RaycastHit[] hits = new RaycastHit[10];

    private PlayerInputHandler input;

    private IInteractable currentInteractable;

    private void Awake()
    {
        input = GetComponent<PlayerInputHandler>();
    }

    private void Update()
    {
        FindInteractable();
        HandleInteraction();
    }

    private void FindInteractable()
    {
        currentInteractable = null;

        int hitCount = Physics.SphereCastNonAlloc(
            interactionPoint.position,
            interactionRadius,
            interactionPoint.forward,
            hits,
            interactionDistance,
            interactableLayer
        );

        if (hitCount == 0)
            return;

        currentInteractable = GetBestInteractable(hitCount);
    }

    private IInteractable GetBestInteractable(int hitCount)
    {
        IInteractable bestInteractable = null;
        float bestScore = float.MaxValue;

        for (int i = 0; i < hitCount; i++)
        {
            IInteractable interactable =
                hits[i].collider.GetComponent<IInteractable>();

            if (interactable == null)
                continue;

            float distance =
                Vector3.Distance(
                    interactionPoint.position,
                    hits[i].point
                );

            Vector3 direction =
                (hits[i].point - interactionPoint.position).normalized;

            float angle =
                Vector3.Angle(
                    interactionPoint.forward,
                    direction
                );

            float score = distance + (angle * 2f);

            if (score < bestScore)
            {
                bestScore = score;
                bestInteractable = interactable;
            }
        }

        return bestInteractable;
    }

    private void HandleInteraction()
    {
        if (!input.Interact.Consume())
            return;

        // If storage is already open, pressing E closes it.
        if (gameUIManager.CurrentMode == UIMode.Storage)
        {
            gameUIManager.HideStorage();
            return;
        }

        currentInteractable?.Interact(this);
    }

    private void OnDrawGizmos()
    {
        if (interactionPoint == null)
            return;

        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(
            interactionPoint.position,
            interactionRadius
        );

        Gizmos.DrawWireSphere(
            interactionPoint.position + interactionPoint.forward * interactionDistance,
            interactionRadius
        );

        Gizmos.DrawLine(
            interactionPoint.position,
            interactionPoint.position + interactionDistance * interactionPoint.forward
        );
    }
}
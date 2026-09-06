using UI;
using UnityEngine;

namespace Player
{
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

        private float lastDebugLogTime;

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

            bool shouldLog = Time.time - lastDebugLogTime >= 1f;
            if (shouldLog)
            {
                lastDebugLogTime = Time.time;

                Debug.Log($"[PlayerInteractor] sphere cast hit {hitCount} collider(s) on the interactable layer.");
                for (int i = 0; i < hitCount; i++)
                {
                    Debug.Log($"[PlayerInteractor]   hit: {hits[i].collider.gameObject.name} (layer: {LayerMask.LayerToName(hits[i].collider.gameObject.layer)})");
                }
            }

            if (hitCount == 0)
                return;

            currentInteractable = GetBestInteractable(hitCount);

            if (shouldLog)
            {
                Debug.Log($"[PlayerInteractor] best interactable this frame: {(currentInteractable == null ? "none" : ((MonoBehaviour)currentInteractable).gameObject.name)}");
            }
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

            Debug.Log($"[PlayerInteractor] E pressed. currentInteractable = {(currentInteractable == null ? "null" : ((MonoBehaviour)currentInteractable).gameObject.name)}");

            // If storage is already open, pressing E closes it.
            if (gameUIManager != null && gameUIManager.CurrentMode == UIMode.Storage)
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
}

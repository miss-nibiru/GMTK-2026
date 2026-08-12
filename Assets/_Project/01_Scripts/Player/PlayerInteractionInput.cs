using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractionInput : MonoBehaviour
{
    [SerializeField] private PlayerPuzzleController puzzleController;
    [SerializeField] private InputActionReference interactAction;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private LayerMask interactionLayers = ~0;

    public GameObject CurrentObject { get; private set; }

    private void OnEnable() => interactAction.action.Enable();
    private void OnDisable() => interactAction.action.Disable();

    private void Update()
    {
        if (!interactAction.action.WasPressedThisFrame()) return;
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        if (!Physics.Raycast(
                ray,
                out RaycastHit hit,
                interactionDistance,
                interactionLayers,
                QueryTriggerInteraction.Collide))
        {
            CurrentObject = null;

            if (puzzleController.CurrentlyHeldItem != null)
                puzzleController.ReleaseHeldItem();

            return;
        }

        BaseInteractable interactable =
            hit.collider.GetComponentInParent<BaseInteractable>();

        EvidenceInteractable evidenceInteractable =
            hit.collider.GetComponentInParent<EvidenceInteractable>();

        if (interactable == null)
        {
            CurrentObject = null;
            return;
        }

        CurrentObject = interactable.gameObject;
        interactable.Interact();
        
        if (evidenceInteractable != null && evidenceInteractable != interactable) 
            evidenceInteractable.Interact();
        
    }
}
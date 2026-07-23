using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionReference interactAction;

    [Header("Interaction")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private LayerMask interactionLayers = ~0;
    
    [Header("Raycast Debug")]
    [SerializeField] private GameObject raycastHitMarker;
    [SerializeField, Min(0f)] private float markerSurfaceOffset = 0.02f;
    
    [Header("UI")]
    [SerializeField] private GameObject interactionPrompt;

    private IInteractable _currentInteractable;
    public GameObject interactObject { get; private set; }

    private void OnEnable()
    {
        interactAction.action.Enable();
    }

    private void OnDisable()
    {
        interactAction.action.Disable();
        if (interactionPrompt != null) interactionPrompt.SetActive(false);
        
    }

    private void Update()
    {
        FindInteractable();

        if (interactAction.action.WasPressedThisFrame() && _currentInteractable != null) 
            _currentInteractable.Interact();
        
    }

    private void FindInteractable()
    {
        _currentInteractable = null;
        interactObject = null;

        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        bool didHit = Physics.Raycast(
            ray,
            out RaycastHit hit,
            interactionDistance,
            interactionLayers,
            QueryTriggerInteraction.Collide
        );

        if (didHit)
        {
            _currentInteractable = hit.collider.GetComponentInParent<IInteractable>();
            interactObject = hit.collider.gameObject;
        }

        if (raycastHitMarker != null)
        {
            raycastHitMarker.SetActive(didHit);

            if (didHit)
                raycastHitMarker.transform.position =
                    hit.point + hit.normal * markerSurfaceOffset;
        }

        if (interactionPrompt != null)
            interactionPrompt.SetActive(_currentInteractable != null);
    }
}
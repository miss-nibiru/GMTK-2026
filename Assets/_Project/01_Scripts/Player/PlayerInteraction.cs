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

    public GameObject interactObject { get; private set; }
    
    [Header("UI")]
    [SerializeField] private GameObject interactionPrompt;

    private IInteractable _currentInteractable;

    private void OnEnable()
    {
        interactAction.action.Enable();
    }

    private void OnDisable()
    {
        interactAction.action.Disable();

        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }
    }

    private void Update()
    {
        FindInteractable();

        if (interactAction.action.WasPressedThisFrame() &&
            _currentInteractable != null)
        {
            _currentInteractable.Interact();
        }
    }

    private void FindInteractable()
    {
        _currentInteractable = null;

        Ray ray = playerCamera.ViewportPointToRay(
            new Vector3(0.5f, 0.5f, 0f)
        );

        if (Physics.Raycast(
                ray,
                out RaycastHit hit,
                interactionDistance,
                interactionLayers,
                QueryTriggerInteraction.Collide))
        {
            _currentInteractable =
                hit.collider.GetComponentInParent<IInteractable>();
            interactObject = hit.collider.gameObject;
        }

        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(_currentInteractable != null);
        }
    }
}
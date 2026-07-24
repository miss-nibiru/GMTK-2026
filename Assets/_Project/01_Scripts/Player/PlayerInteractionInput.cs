using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractionInput : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionReference interactAction;
    
    [Header("Interaction")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private LayerMask interactionLayers = ~0;

    private BaseInteractable currentInteractable;

    public GameObject currentObject {get; private set;}
    private GameObject tempObject;
    
    private void OnEnable()
    {
        interactAction.action.Enable();
    }

    private void OnDisable()
    {
        interactAction.action.Disable();
    }

    private void Update()
    {
        if (!interactAction.action.WasPressedThisFrame()) return;
        
        ShootRay();
        
        currentInteractable.Interact();
    }

    private void ShootRay()
    {
        currentObject = null;
        
        Ray ray = playerCamera.ViewportPointToRay(
            new Vector3(0.5f, 0.5f, 0f)
        );

        if (!Physics.Raycast(
                ray,
                out RaycastHit hit,
                interactionDistance,
                interactionLayers,
                QueryTriggerInteraction.Collide))
        {
            currentInteractable = null;
            currentObject = null;
            return;
        }
        
        tempObject = hit.collider.gameObject;

        if (!tempObject.GetComponent<BaseInteractable>()) return;

        currentInteractable = tempObject.GetComponent<BaseInteractable>();

        if (!tempObject.GetComponent<PickUpItems>()) return;
        
        currentObject = tempObject;
    }
}

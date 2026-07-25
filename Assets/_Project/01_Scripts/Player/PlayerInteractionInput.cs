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
    private BaseInteractable previousInteractable;

    private GameObject pastObject;
    public GameObject currentObject {get; private set;}
    private GameObject newObject;
    
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
        
        currentInteractable?.Interact();
        
        if(pastObject?.GetComponent<PickUpItems>() && pastObject != currentObject)
            previousInteractable?.Interact();
    }

    private void ShootRay()
    {
        pastObject = currentObject;
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
            if(pastObject?.GetComponent<PickUpItems>())
                currentInteractable?.Interact();

            pastObject = null;
            currentInteractable = null;
            return;
        }
        
        newObject = hit.collider.gameObject;

        if (!newObject.GetComponent<BaseInteractable>()) return;
        
        previousInteractable = currentInteractable;
        currentInteractable = newObject.GetComponent<BaseInteractable>();

        if (!newObject.GetComponent<PickUpItems>()) return;
        
        currentObject = newObject;
    }
}

using UnityEngine;

public class PlayerInteractionUI : MonoBehaviour
{
    [Header("Interaction")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private LayerMask interactionLayers = ~0;
    
    [Header("UI")]
    [SerializeField] private GameObject interactionPrompt;

    private bool foundInteractable;

    private void Update()
    {
        FindInteractable();
        interactionPrompt.SetActive(foundInteractable);
    }

    private void FindInteractable()
    {
        foundInteractable = false;

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
            BaseInteractable interactable = hit.collider.GetComponent<BaseInteractable>();
            foundInteractable = interactable != null && interactable.CanInteract();
            
        }
    }
}
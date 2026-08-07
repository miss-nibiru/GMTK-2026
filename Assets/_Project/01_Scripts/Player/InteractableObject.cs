using UnityEngine;

public class InteractableObject : MonoBehaviour, IInteractable
{
    
    [SerializeField] private string objectName;

    public void Interact()
    {
        Debug.Log($"Interacted with {objectName}", this);
    }
    
    public bool CanInteract()
    {
        return true;
    }
    
}
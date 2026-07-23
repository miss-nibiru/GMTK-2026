using UnityEngine;

public class PickUpItems : MonoBehaviour, IInteractable
{
    public bool pickUpToggle { get; private set; }
    
    public void Interact()
    {
        pickUpToggle = !pickUpToggle;
    }
}

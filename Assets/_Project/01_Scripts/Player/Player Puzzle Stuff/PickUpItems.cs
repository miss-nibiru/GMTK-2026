using UnityEngine;

public class PickUpItems : BaseInteractable
{
    public bool pickUpToggle { get; private set; }
    
    public override void Interact()
    {
        pickUpToggle = !pickUpToggle;
    }
}

using UnityEngine;

public class PickUpItems : BaseInteractable
{
    public bool pickUpToggle { get; private set; }
    private PlayerPuzzleController puzzleController;
    
    public override void Interact()
    {
        puzzleController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerPuzzleController>();
        
        puzzleController.currentlyHeldItem = gameObject;
        pickUpToggle = !pickUpToggle;
    }
}

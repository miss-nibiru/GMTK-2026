using UnityEngine;

public class PickUpItems : BaseInteractable
{
    public int puzzleIndexNum;
    
    public bool pickUpToggle { get; private set; }
    private PlayerPuzzleController puzzleController;
    
    public override void Interact()
    {
        puzzleController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerPuzzleController>();
        
        pickUpToggle = !pickUpToggle;
        puzzleController.currentlyHeldItem = gameObject;
    }
}

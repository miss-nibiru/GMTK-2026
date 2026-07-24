using UnityEngine;

public class PickUpItems : BaseInteractable
{
    public int puzzleIndexNum;

    public bool pickUpToggle;
    private PlayerPuzzleController puzzleController;
    
    public override void Interact()
    {
        puzzleController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerPuzzleController>();
        
        switch (pickUpToggle)
        {
            case false:
                puzzleController.currentlyHeldItem = gameObject;
                break;
            case true:
                puzzleController.currentlyHeldItem  = null;
                break;        
        }
        pickUpToggle = !pickUpToggle;
    }
}

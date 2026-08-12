using UnityEngine;

public class PickUpItems : BaseInteractable
{
    public int puzzleIndexNum;
    public bool pickUpToggle;
    private PlayerPuzzleController _puzzleController;
    
    private void Awake()
    {
        _puzzleController = FindFirstObjectByType<PlayerPuzzleController>(); // find puzzle controller on the thing if needed
    }
    
    public override void Interact()
    {
        if (_puzzleController == null) return;

        GameObject heldItem = _puzzleController.CurrentlyHeldItem;

        if (heldItem == gameObject)
        {
            _puzzleController.ReleaseHeldItem();
            pickUpToggle = false;
            return;
        }

        if (heldItem != null)
        {
            PickUpItems previousItem = heldItem.GetComponent<PickUpItems>();

            if (previousItem != null)
                previousItem.pickUpToggle = false;

            _puzzleController.ReleaseHeldItem();
        }

        if (_puzzleController.HoldItem(gameObject))
        {
            pickUpToggle = true;
            AudioManager.Instance?.PlayPickup();
        }
        
    }
    
}

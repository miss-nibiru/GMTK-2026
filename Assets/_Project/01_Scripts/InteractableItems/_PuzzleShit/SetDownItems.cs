using UnityEngine;

public class SetDownItems : BaseInteractable
{
    [SerializeField] private PuzzleManager puzzleManager;
    [SerializeField] private PlayerPuzzleController puzzleController;

    public int puzzleIndexNum;
    
    public override void Interact()
    {
        if (!puzzleController.currentlyHeldItem) return;

        if (puzzleController.currentlyHeldItem.GetComponent<PickUpItems>().puzzleIndexNum != puzzleIndexNum) return;
        
        Destroy(puzzleController.currentlyHeldItem);
        puzzleManager.psm.currentPuzzle.UpdateProgress();
    }
}

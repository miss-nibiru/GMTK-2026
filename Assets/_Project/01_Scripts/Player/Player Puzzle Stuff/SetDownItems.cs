using UnityEngine;

public class SetDownItems : BaseInteractable
{
    [SerializeField] private PuzzleManager puzzleManager;
    [SerializeField] private PlayerPuzzleController puzzleController;
    
    public override void Interact()
    {
        if (!puzzleController.currentlyHeldItem) return;
        
        Destroy(puzzleController.currentlyHeldItem);
        puzzleManager.psm.currentPuzzle.UpdateProgress();
    }
}

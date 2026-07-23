using UnityEngine;

public class SetDownItems : BaseInteractable
{
    [SerializeField] private PuzzleManager puzzleManager;
    
    public override void Interact()
    {
        puzzleManager.psm.currentPuzzle.UpdateProgress();
    }
}

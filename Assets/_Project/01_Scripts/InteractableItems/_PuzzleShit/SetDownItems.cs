using UnityEngine;

public class SetDownItems : BaseInteractable
{
    [SerializeField] private PuzzleManager puzzleManager;
    [SerializeField] private PlayerPuzzleController puzzleController;

    public int puzzleIndexNum;

    public GameObject[] missingPieces;

    private PickUpItems currentItem;
    
    public override void Interact()
    {
        if (!puzzleController.currentlyHeldItem) return;

        currentItem = puzzleController.currentlyHeldItem.GetComponent<PickUpItems>();
        
        if (currentItem.puzzleIndexNum < 1 || currentItem.puzzleIndexNum > 6) return;

        missingPieces[currentItem.puzzleIndexNum - 1].SetActive(true);
        
        Destroy(puzzleController.currentlyHeldItem);
        puzzleManager.psm.currentPuzzle.UpdateProgress();
    }
}

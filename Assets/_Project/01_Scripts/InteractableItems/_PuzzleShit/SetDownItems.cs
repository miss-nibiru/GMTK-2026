using UnityEngine;

public class SetDownItems : BaseInteractable
{
    [SerializeField] private PuzzleManager puzzleManager;
    [SerializeField] private PlayerPuzzleController puzzleController;

    public GameObject[] missingPieces;

    public override void Interact()
    {
        GameObject heldItem = puzzleController.CurrentlyHeldItem;

        if (heldItem == null) return;

        PickUpItems item = heldItem.GetComponent<PickUpItems>();

        if (item == null) return;
        if (item.puzzleIndexNum < 1 || item.puzzleIndexNum > missingPieces.Length) return;

        missingPieces[item.puzzleIndexNum - 1].SetActive(true);

        puzzleController.ConsumeHeldItem();
        puzzleManager.Psm.currentPuzzle?.UpdateProgress();
    }
}
using UnityEngine;

public class UseOven : BaseInteractable
{
    [SerializeField] private PlayerPuzzleController puzzleController;
    [SerializeField] private GameTimeManager timeManager;
    [SerializeField] private GameObject thawedCarPrefab;
    [SerializeField] private Transform carOutputPosition;

    [SerializeField] private int thawHour = 13;
    private bool _carInside;

    public override void Interact()
    {
        if (_carInside)
        {
            TryCollectCar();
            return;
        }

        TryInsertCar();
    }

    private void TryInsertCar()
    {
        if (timeManager.CurrentHour >= thawHour) return;

        GameObject heldItem = puzzleController.CurrentlyHeldItem;

        if (heldItem == null) return;
        if (!heldItem.TryGetComponent(out FrozenCarItem frozenCar)) return;

        puzzleController.ConsumeHeldItem();
        _carInside = true;
    }

    private void TryCollectCar()
    {
        if (timeManager.CurrentHour < thawHour) return;

        Instantiate(
            thawedCarPrefab,
            carOutputPosition.position,
            carOutputPosition.rotation
        );

        _carInside = false;
    }
}
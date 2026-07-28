using UnityEngine;

public class UseOven : BaseInteractable
{
    [SerializeField] private PlayerPuzzleController puzzleController;
    [SerializeField] private GameTimeManager timeManager;
    [SerializeField] private GameObject thawedCarPrefab;
    [SerializeField] private Transform carOutputPosition;
    [SerializeField] private Animator ovenAnimator;

    [SerializeField] private int thawHour = 13;
    private bool _carInside;

    public override void Interact()
    {
        if (_carInside)
        {
            TryCollectCar();
            Debug.Log(thawedCarPrefab.name + " has been collected");
            return;
        }

        TryInsertCar();
        Debug.Log(thawedCarPrefab.name + " has been inserted");
    }

    private void TryInsertCar()
    {
        if (timeManager.CurrentHour >= thawHour) return;
        GameObject heldItem = puzzleController.CurrentlyHeldItem;

        if (heldItem == null) return;
        if (!heldItem.TryGetComponent(out FrozenCarItem frozenCar)) return;
        
        ovenAnimator.Play("OpenOven");

        puzzleController.ConsumeHeldItem();
        _carInside = true;
    }

    private void TryCollectCar()
    {
        if (timeManager.CurrentHour < thawHour) return;

        ovenAnimator.Play("OpenOven");
        
        Instantiate(
            thawedCarPrefab,
            carOutputPosition.position,
            carOutputPosition.rotation
        );

        _carInside = false;
    }
}
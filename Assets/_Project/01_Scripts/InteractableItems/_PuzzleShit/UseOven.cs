using UnityEngine;

public class UseOven : BaseInteractable
{
    [SerializeField] private PlayerPuzzleController puzzleController;
    [SerializeField] private GameTimeManager timeManager;
    [SerializeField] private GameObject thawedCarPrefab;
    [SerializeField] private Transform carOutputPosition;
    [SerializeField] private Animator ovenAnimator;
    [SerializeField] private int thawHour;
    
    [Header("Non interactable, just visual")]
    [SerializeField] private GameObject frozenCarVisual;
    [SerializeField] private GameObject thawedCarVisual;
    
    [SerializeField] private GameObject blueLedVisual;
    [SerializeField] private GameObject redLedVisual;
    
    private bool _carInside;
    private bool _hasThawed;
    private bool _hasExpired;

    private void Start()
    {
        blueLedVisual.SetActive(true);
        redLedVisual.SetActive(false);
    }
    private void Update()
    {
        if (!_hasExpired && timeManager.CurrentHour >= thawHour)
        {
            _hasExpired = true;
            blueLedVisual.SetActive(false);
            redLedVisual.SetActive(true);
        }
        
        if(!_carInside) return;
        if(_hasThawed) return;
        if (timeManager.CurrentHour < thawHour) return;
        
        frozenCarVisual.SetActive(false);
        thawedCarVisual.SetActive(true);
        
        _hasThawed = true;
        
    }

    public override void Interact()
    {
        if (_carInside)
        {
            if (timeManager.CurrentHour < thawHour)
            {
                ovenAnimator.Play("OpenOven");
                return;
            }

            TryCollectCar();
            return;
        }

        bool insertionSucceeded = TryInsertCar();

        if (insertionSucceeded)
        {
            Debug.Log(thawedCarPrefab.name + " has been inserted");
        }
    }

    private bool TryInsertCar()
    {
        
        if (_hasExpired)
            return false;
        
        if (timeManager.CurrentHour >= thawHour) 
            return false;
        
        GameObject heldItem = puzzleController.CurrentlyHeldItem;

        if (heldItem == null)
            return false;
        if (!heldItem.TryGetComponent(out FrozenCarItem frozenCar))
            return false;
        
        ovenAnimator.Play("OpenOven");

        puzzleController.ConsumeHeldItem();
        frozenCarVisual.SetActive(true);
        thawedCarVisual.SetActive(false);
        
        _carInside = true;
        _hasThawed = false;
        return true;
    }

    private void TryCollectCar()
    {
        if (!_hasThawed)
            return;

        ovenAnimator.Play("OpenOven");

        Instantiate(
            thawedCarPrefab,
            carOutputPosition.position,
            carOutputPosition.rotation
        );

        frozenCarVisual.SetActive(false);
        thawedCarVisual.SetActive(false);

        _carInside = false;
        _hasThawed = false;
    }
}
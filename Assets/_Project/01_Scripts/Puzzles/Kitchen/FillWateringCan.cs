using UnityEngine;

public class FillWateringCan : BaseInteractable
{
    [SerializeField] private PlayerPuzzleController puzzleControl;

    private WateringCanInteractions _can;
    
    public override void Interact()
    {
        if (!puzzleControl.CurrentlyHeldItem) return;
        if (!puzzleControl.CurrentlyHeldItem.GetComponent<WateringCanInteractions>()) return;

        _can = puzzleControl.CurrentlyHeldItem.GetComponent<WateringCanInteractions>();
        _can.wsm.SwitchStates(_can.wsm.fullState);
        
        AudioManager.Instance?.PlayWater();
        
    }
}

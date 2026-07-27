using UnityEngine;

public class FillWateringCan : BaseInteractable
{
    [SerializeField] private PlayerPuzzleController puzzleControl;

    private WateringCanInteractions can;
    
    public override void Interact()
    {
        if (!puzzleControl.CurrentlyHeldItem) return;
        
        if (!puzzleControl.CurrentlyHeldItem.GetComponent<WateringCanInteractions>()) return;

        can = puzzleControl.CurrentlyHeldItem.GetComponent<WateringCanInteractions>();
        can.wsm.SwitchStates(can.wsm.fullState);
        Debug.Log("Filled");
    }
}

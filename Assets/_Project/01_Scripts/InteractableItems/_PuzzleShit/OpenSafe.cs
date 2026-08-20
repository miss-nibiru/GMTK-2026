using UnityEngine;

public class OpenSafe : BaseInteractable
{
    [SerializeField] private PlayerPuzzleController puzzleControl;

    private bool toggleState;
    
    public override void Interact()
    {
        switch (toggleState)
        {
            case false:
                puzzleControl.Ppsm.SwitchStates(puzzleControl.Ppsm.SafeState);
                break;
            case true:
                puzzleControl.Ppsm.SwitchStates(puzzleControl.Ppsm.PickUpState);
                break;
        }

        toggleState = !toggleState;
    }
}

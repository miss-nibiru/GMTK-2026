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
                puzzleControl.ppsm.SwitchStates(puzzleControl.ppsm.safeState);
                break;
            case true:
                puzzleControl.ppsm.SwitchStates(puzzleControl.ppsm.pickUpState);
                break;
        }
    }
}

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
                puzzleControl.Ppsm.SwitchStates(puzzleControl.Ppsm.safeState);
                break;
            case true:
                puzzleControl.Ppsm.SwitchStates(puzzleControl.Ppsm.pickUpState);
                break;
        }

        toggleState = !toggleState;
    }
}

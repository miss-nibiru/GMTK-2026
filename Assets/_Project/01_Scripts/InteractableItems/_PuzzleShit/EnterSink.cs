using UnityEngine;

public class EnterSink : BaseInteractable
{
    [SerializeField] private PlayerPuzzleController puzzleController;

    private bool _interactToggle;
    
    public override void Interact()
    {
        switch (_interactToggle)
        {
            case false:
                puzzleController.Ppsm.SwitchStates(puzzleController.Ppsm.sinkState);
                break;
            case true:
                puzzleController.Ppsm.SwitchStates(puzzleController.Ppsm.pickUpState);
                break;
        }
        _interactToggle = !_interactToggle;
    }
}

using UnityEngine;

public class EnterSink : BaseInteractable
{
    [SerializeField] private PlayerPuzzleController puzzleController;

    private bool interactToggle;
    
    public override void Interact()
    {
        switch (interactToggle)
        {
            case false:
                puzzleController.ppsm.SwitchStates(puzzleController.ppsm.sinkState);
                break;
            case true:
                puzzleController.ppsm.SwitchStates(puzzleController.ppsm.pickUpState);
                break;
        }
        interactToggle = !interactToggle;
    }
}

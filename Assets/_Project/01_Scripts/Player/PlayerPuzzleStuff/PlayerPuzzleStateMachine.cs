using UnityEngine;

public class PlayerPuzzleStateMachine
{
    public IPlayerPuzzleStates currentState;

    public PPSPickUp pickUpState;
    
    public PlayerPuzzleStateMachine(PlayerPuzzleController puzzleController)
    {
        pickUpState = puzzleController.gameObject.GetComponent<PPSPickUp>();
    }

    public void SwitchStates(IPlayerPuzzleStates state)
    {
        currentState?.Exit();
        currentState = state;
        currentState.Enter();
    }
}

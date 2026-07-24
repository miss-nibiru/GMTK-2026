public class PlayerPuzzleStateMachine
{
    public IPlayerPuzzleStates currentState;

    public PPSPickUp pickUpState;
    public PPSTurningKnobs sinkState;
    
    public PlayerPuzzleStateMachine(PlayerPuzzleController puzzleController)
    {
        pickUpState = puzzleController.gameObject.GetComponent<PPSPickUp>();
        sinkState = puzzleController.gameObject.GetComponent<PPSTurningKnobs>();
    }

    public void SwitchStates(IPlayerPuzzleStates state)
    {
        currentState?.Exit();
        currentState = state;
        currentState.Enter();
    }
}

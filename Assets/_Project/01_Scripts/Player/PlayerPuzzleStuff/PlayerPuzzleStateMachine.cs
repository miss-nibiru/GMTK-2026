public class PlayerPuzzleStateMachine
{
    public IPlayerPuzzleStates currentState;

    public PPSPickUp pickUpState;
    public PPSTurningKnobs sinkState;
    public PPSOpenSafe safeState;
    
    public PlayerPuzzleStateMachine(PlayerPuzzleController puzzleController)
    {
        pickUpState = puzzleController.gameObject.GetComponent<PPSPickUp>();
        sinkState = puzzleController.gameObject.GetComponent<PPSTurningKnobs>();
        safeState = puzzleController.gameObject.GetComponent<PPSOpenSafe>();
    }

    public void SwitchStates(IPlayerPuzzleStates state)
    {
        currentState?.Exit();
        currentState = state;
        currentState.Enter();
    }
}

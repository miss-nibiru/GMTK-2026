public class PlayerPuzzleStateMachine
{
    public IPlayerPuzzleStates CurrentState;

    public PpsPickUp PickUpState;
    public PPSTurningKnobs SinkState;
    public PPSOpenSafe SafeState;
    
    public PlayerPuzzleStateMachine(PlayerPuzzleController puzzleController)
    {
        PickUpState = puzzleController.gameObject.GetComponent<PpsPickUp>();
        SinkState = puzzleController.gameObject.GetComponent<PPSTurningKnobs>();
        SafeState = puzzleController.gameObject.GetComponent<PPSOpenSafe>();
    }

    public void SwitchStates(IPlayerPuzzleStates state)
    {
        CurrentState?.Exit();
        CurrentState = state;
        CurrentState.Enter();
    }

    public void SetInitialState(PpsPickUp ppsmPickUpState)
    {
        CurrentState = ppsmPickUpState;
        CurrentState.Enter();
    }
}

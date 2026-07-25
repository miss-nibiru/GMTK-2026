public class PuzzleStateMachine
{
    public IPuzzleStates currentPuzzle;

    private PuzzleManager pm;
    
    public PuzzleStateMachine(PuzzleManager puzzleManager)
    {
        pm = puzzleManager;
    }

    public void SwitchStates(IPuzzleStates state)
    {
        currentPuzzle?.Exit();
        currentPuzzle = state;
        currentPuzzle?.Enter(pm);
    }
}

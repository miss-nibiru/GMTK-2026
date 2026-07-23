using UnityEngine;

public class PuzzleStateMachine
{
    public IPuzzleStates currentPuzzle;

    public PSMasterBed masterBedPuzzle;
    public PSKidsBed kidsBedPuzzle;
    public PSLivingRoom livingRoomPuzzle;

    private PuzzleManager pm;
    
    public PuzzleStateMachine(PuzzleManager puzzleManager)
    {
        pm = puzzleManager;
        masterBedPuzzle = pm.masterBedroom.GetComponent<PSMasterBed>();
        kidsBedPuzzle = pm.kidsBedroom.GetComponent<PSKidsBed>();
        livingRoomPuzzle = pm.livingRoom.GetComponent<PSLivingRoom>();
    }

    public void SwitchStates(IPuzzleStates state)
    {
        currentPuzzle?.Exit();
        currentPuzzle = state;
        currentPuzzle.Enter(pm);
    }
}

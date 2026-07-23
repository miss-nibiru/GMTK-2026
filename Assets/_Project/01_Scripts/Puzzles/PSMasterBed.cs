using UnityEngine;

public class PSMasterBed : MonoBehaviour, IPuzzleStates
{
    private int progressCheck;

    private PuzzleManager puzzleManager;

    public void Enter(PuzzleManager pm)
    {
        puzzleManager = pm;
    }

    public void Exit()
    {
        
    }
}

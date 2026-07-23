using UnityEngine;

public interface IPuzzleStates
{
    void Enter(PuzzleManager puzzleManager);
    void Exit();
}

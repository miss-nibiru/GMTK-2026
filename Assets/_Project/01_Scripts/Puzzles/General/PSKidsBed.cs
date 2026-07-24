using UnityEngine;

public class PSKidsBed : MonoBehaviour, IPuzzleStates
{
    private int progressCheck;
    
    
    public void Enter(PuzzleManager pm)
    {
        
    }

    public void Exit()
    {
        
    }

    public void UpdateProgress()
    {
        progressCheck++;

        if (progressCheck >= 6)
        {
            Debug.Log("Finished Puzzle");
        }
    }
}

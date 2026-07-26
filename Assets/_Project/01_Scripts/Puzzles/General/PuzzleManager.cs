using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    //Max!! I added a constructor to this so the state machine gets created first thing -- changed Start to Awake!
    public PuzzleStateMachine Psm { get; private set;}

    private void Awake()
    {
        Psm = new PuzzleStateMachine(this);
    }
    
}

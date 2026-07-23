using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public PuzzleStateMachine psm;

    private void Start()
    {
        psm = new PuzzleStateMachine(this);
    }
}

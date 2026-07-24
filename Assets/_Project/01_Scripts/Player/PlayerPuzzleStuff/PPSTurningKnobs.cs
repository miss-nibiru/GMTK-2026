using UnityEngine;

public class PPSTurningKnobs : MonoBehaviour, IPlayerPuzzleStates
{
    public void Enter()
    {
        Cursor.lockState = CursorLockMode.None;
        Debug.Log("enter");
    }

    public void Execute()
    {
        
    }

    public void Exit()
    {
        Debug.Log("exit");
    }
}

using UnityEngine;

public class PPSTurningKnobs : MonoBehaviour, IPlayerPuzzleStates
{
    [SerializeField] private GameObject knobScreen;
    
    public void Enter()
    {
        Cursor.lockState = CursorLockMode.None;
        knobScreen.SetActive(true);
    }

    public void Execute()
    {
        
    }

    public void Exit()
    {
        knobScreen.SetActive(false);
    }
}

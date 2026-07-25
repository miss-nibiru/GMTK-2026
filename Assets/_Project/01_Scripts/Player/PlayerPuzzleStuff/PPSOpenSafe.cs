using UnityEngine;

public class PPSOpenSafe : MonoBehaviour, IPlayerPuzzleStates
{
    [SerializeField] private GameObject safeScreen;
    
    public void Enter()
    {
        Cursor.lockState = CursorLockMode.None;
        safeScreen.SetActive(true);
    }

    public void Execute()
    {
        
    }

    public void Exit()
    {
        safeScreen.SetActive(false);
    }
}

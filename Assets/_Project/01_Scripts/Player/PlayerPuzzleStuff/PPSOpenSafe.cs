using UnityEngine;

public class PPSOpenSafe : MonoBehaviour, IPlayerPuzzleStates
{
    [SerializeField] private GameObject safeScreen;
    
    public void Enter()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        safeScreen.SetActive(true);
    }

    public void Execute()
    {
        
    }

    public void Exit()
    {
        safeScreen.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}

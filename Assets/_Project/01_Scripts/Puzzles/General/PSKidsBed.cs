using UnityEngine;

public class PSKidsBed : MonoBehaviour, IPuzzleStates
{
    [SerializeField] private GameObject key;
    
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
            Instantiate(key, transform.position, Quaternion.identity);
        }
    }
}

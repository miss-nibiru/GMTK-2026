using UnityEngine;

public class PSKidsBed : MonoBehaviour, IPuzzleStates
{
    [SerializeField] private GameObject key;
    [SerializeField] private Transform spawnPoint;
    
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
            Instantiate(key, spawnPoint.position, Quaternion.identity);
            Debug.Log(key.transform.position);
        }
    }
}

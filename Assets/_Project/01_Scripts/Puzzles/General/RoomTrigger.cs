using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    private PuzzleManager _puzzleManager;
    
    private void OnTriggerEnter(Collider other)
    {
        _puzzleManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<PuzzleManager>();
        
        _puzzleManager.psm.SwitchStates(gameObject.GetComponent<IPuzzleStates>());
        Debug.Log(_puzzleManager.psm.currentPuzzle);
    }
    
    
}

using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    private PuzzleManager puzzleManager;
    
    private void OnTriggerEnter(Collider other)
    {
        puzzleManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<PuzzleManager>();
        
        puzzleManager.psm.SwitchStates(gameObject.GetComponent<IPuzzleStates>());
        Debug.Log(puzzleManager.psm.currentPuzzle);
    }
}

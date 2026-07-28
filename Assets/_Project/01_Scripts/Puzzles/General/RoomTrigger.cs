using UnityEngine;

// i cleaned this one up a little - check before merging!!!
// like you had it before -- any collider could trigger a room puzzle
// i changed it so its a bit safer the puzzle activation!
public class RoomTrigger : MonoBehaviour

{
    [SerializeField] private PuzzleManager puzzleManager;

    private IPuzzleStates _puzzleState;

    private void Awake()
    {
        if (puzzleManager == null) puzzleManager = FindFirstObjectByType<PuzzleManager>(); // Finds the puzzlemanager if one was not assigned.
        ResolvePuzzleState();
        ValidateSetup();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.transform.CompareTag("Player")) return; // This one checks for it being only the player that can activate the puzzle

        if (puzzleManager == null || puzzleManager.Psm == null || _puzzleState == null) return;
        puzzleManager.Psm.SwitchStates(_puzzleState);
    }

    private void ResolvePuzzleState()
    {
        
        MonoBehaviour[] attachedScripts = GetComponents<MonoBehaviour>(); //goes throuhg the list of monobehaviour scripts until it finds if this script implements the puzzle state
        foreach (MonoBehaviour attachedScript in attachedScripts)
        {
            if (attachedScript is IPuzzleStates puzzleState)
            {
                _puzzleState = puzzleState;
                return;
            }
        }
    }

    /// <summary>
    /// Debugging section
    /// </summary>
    private void ValidateSetup()
    {
        Collider roomCollider = GetComponent<Collider>();

        if (roomCollider == null)
            Debug.LogError($"'{name}' needs a Collider.", this);
        else if (!roomCollider.isTrigger)
            Debug.LogWarning($"'{name}' Collider needs Is Trigger enabled.", this);

        if (puzzleManager == null)
            Debug.LogError($"'{name}' cannot find PuzzleManager.", this);

        if (_puzzleState == null)
            Debug.LogError($"'{name}' needs an IPuzzleStates component.", this);
    }
}
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public GameObject masterBedroom;
    public GameObject kidsBedroom;
    public GameObject livingRoom;

    public PuzzleStateMachine psm;

    private void Start()
    {
        psm = new PuzzleStateMachine(this);
    }
}

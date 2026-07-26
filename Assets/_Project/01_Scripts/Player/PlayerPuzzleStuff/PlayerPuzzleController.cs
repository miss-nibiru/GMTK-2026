using UnityEngine;

public class PlayerPuzzleController : MonoBehaviour
{
    public GameObject CurrentlyHeldItem { get; private set; }
    public PlayerPuzzleStateMachine Ppsm { get; private set; }

    private void Start()
    {
        Ppsm = new PlayerPuzzleStateMachine(this);
        Ppsm.SwitchStates(Ppsm.pickUpState);
    }

    private void Update()
    {
        Ppsm.currentState?.Execute();
    }

    public bool HoldItem(GameObject item)
    {
        if (item == null || CurrentlyHeldItem != null) return false;

        CurrentlyHeldItem = item;
        return true;
    }

    public GameObject ReleaseHeldItem()
    {
        GameObject releasedItem = CurrentlyHeldItem;
        CurrentlyHeldItem = null;

        return releasedItem;
    }

    public void ConsumeHeldItem()
    {
        if (CurrentlyHeldItem == null) return;

        Destroy(CurrentlyHeldItem);
        CurrentlyHeldItem = null;
    }
}
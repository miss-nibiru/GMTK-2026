using UnityEngine;

public class WateringCanInteractions : PickUpItems
{
    public WaterStateMachine wsm;

    private void Start()
    {
        wsm = new WaterStateMachine(this);
    }
}

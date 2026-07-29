using System.Collections;
using UnityEngine;

public class GrowFlower : BaseInteractable
{
    [SerializeField] private PlayerPuzzleController puzzleControl;
    private Vector3 positionIncrease = new Vector3(0, 0.1f, 0);
    private GameObject flower;

    private WateringCanInteractions can;
    
    public override void Interact()
    {
        if (!puzzleControl.CurrentlyHeldItem) return;
        
        if (!puzzleControl.CurrentlyHeldItem.GetComponent<WateringCanInteractions>()) return;
        
        can = puzzleControl.CurrentlyHeldItem.GetComponent<WateringCanInteractions>();
        if (can.wsm.currentState != can.wsm.fullState) return;
        
        flower = gameObject.transform.GetChild(1).gameObject;
        Growing();
    }

    private void Growing()
    {
        if (flower.transform.position.y < -0.7)
            StartCoroutine(growTimer());
    }

    private IEnumerator growTimer()
    {
        yield return new WaitForSeconds(0.1f);
        flower.transform.position += positionIncrease;
        Growing();
    }
}

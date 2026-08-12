using System.Collections;
using UnityEngine;

public class GrowFlower : BaseInteractable
{
    [SerializeField] private PlayerPuzzleController puzzleControl;
    private readonly Vector3 _positionIncrease = new Vector3(0, 0.1f, 0);
    private GameObject _flower;
    private bool _wateringStarted;
    private WateringCanInteractions can;
    
    public override void Interact()
    {
        if (!puzzleControl.CurrentlyHeldItem) return;
        
        if (!puzzleControl.CurrentlyHeldItem.GetComponent<WateringCanInteractions>()) return;
        
        can = puzzleControl.CurrentlyHeldItem.GetComponent<WateringCanInteractions>();
        if (can.wsm.currentState != can.wsm.fullState) return;
        
        _flower = transform.GetChild(1).gameObject;

        if (_wateringStarted || _flower.transform.position.y >= -0.7f)
            return;

        _wateringStarted = true;
        AudioManager.Instance?.PlayWater();
        Growing();
    }

    private void Growing()
    {
        if (_flower.transform.position.y < -0.7)
            StartCoroutine(growTimer());
    }

    private IEnumerator growTimer()
    {
        yield return new WaitForSeconds(0.1f);
        _flower.transform.position += _positionIncrease;
        Growing();
    }
}

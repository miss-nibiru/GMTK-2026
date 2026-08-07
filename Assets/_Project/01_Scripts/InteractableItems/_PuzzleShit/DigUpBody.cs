using System.Collections;
using UnityEngine;

public class DigUpBody : BaseInteractable
{
    [SerializeField] private PlayerPuzzleController puzzleControl;
    private readonly Vector3 _positionIncrease = new Vector3(0, 0.1f, 0);
    [SerializeField] private GameObject body;
    
    private bool _isDigging;

    public override bool CanInteract()
    {
        if (!puzzleControl.CurrentlyHeldItem) return false;
        if (!puzzleControl.CurrentlyHeldItem.GetComponent<UseShovel>()) return false;
        return true;
    }

    public override void Interact()
    {
        if (_isDigging) return;
        if (!puzzleControl.CurrentlyHeldItem) return;
        if (!puzzleControl.CurrentlyHeldItem.GetComponent<UseShovel>()) return;

        _isDigging = true;
        Digging();
    }

    private void Digging()
    {
        if (body.transform.position.y < -0.55f) 
        {
            StartCoroutine(DigTimer());
            return;
        }
    }

    private IEnumerator DigTimer()
    {
        yield return new WaitForSeconds(0.1f);
        body.transform.position += _positionIncrease;
        Digging();
    }
    
}
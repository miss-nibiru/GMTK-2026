using UnityEngine;

public class HourglassInteractable : MonoBehaviour, IInteractable
{
    private bool _isRewinding;

    public void Interact()
    {
        
        if (_isRewinding) return;
        if (!TimeLoopManager.Instance) return;
        _isRewinding = true;
        TimeLoopManager.Instance.RewindFullDay();
        
    }
}
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// this manages the house power and connects to time manager
/// starts with power of house on
/// beginblackout - power off = announces event
/// Communicates with end day beep as well so the player knows what to do
/// </summary>

public class BlackoutController : MonoBehaviour
{
    
    [SerializeField] private UnityEvent onPowerOff;

    public bool IsPowerOn { get; private set; } = true;

    public void BeginPowerOff()
    {
        if (!IsPowerOn) return;

        IsPowerOn = false;
        onPowerOff?.Invoke();
    }

}
    

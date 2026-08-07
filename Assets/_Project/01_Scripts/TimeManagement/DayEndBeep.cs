using System.Collections;
using UnityEngine;

/// <summary>
/// when the day ends, player needs to be somehow reminded that hourglass exists -- player feel
/// this script will temporarily make the hourglass glow -- later change to hourglass specific script??
/// Hourglass turns back time the whole day -- is there a way to turn back time and have everything that was done in the last few
/// minutes reseted?
/// </summary>

public class DayEndBeep : MonoBehaviour
{
    [SerializeField] private GameTimeManager timeManager;
    [SerializeField] private float thoughtDuration;
    
    [SerializeField] private GameObject hourglassGlow;

    private void OnEnable()
    {
        if (timeManager == null) return;
        timeManager.DayEnded += ShowDayEndSignal;
        if (timeManager.IsDayEnded) ShowDayEndSignal();
        
    }

    private void OnDisable()
    {
        if (timeManager != null)
        {
            timeManager.DayEnded -= ShowDayEndSignal;
        }
    }

    private void ShowDayEndSignal()
    {
        if (hourglassGlow != null) hourglassGlow.SetActive(true);
        
    }
    
}
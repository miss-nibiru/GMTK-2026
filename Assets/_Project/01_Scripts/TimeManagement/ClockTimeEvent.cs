using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// MAAAAX! this is the script to connect to your puzzles
/// Attach this script to a puzzle object that needs the timer
/// Choose the time it should activate at then
/// communicate with OnTimeReached func to anything that needs to happen then
/// </summary>
public class ClockTimeEvent : MonoBehaviour
{
    [SerializeField] private GameTimeManager timeManager;

    [Header("Trigger Time")]
    [SerializeField, Range(0, 23)] private int hour = 15;
    [SerializeField, Range(0, 59)] private int minute = 30;

    [Header("Event")]
    [SerializeField] private UnityEvent onTimeReached;

    private bool _hasTriggered;

    private void Start()
    {
        if (timeManager == null)
            timeManager = GameTimeManager.Instance;

        if (timeManager == null) return;
        timeManager.TimeChanged += CheckTime;
        CheckTime(timeManager.CurrentHour, timeManager.CurrentMinute);
    }

    private void OnDestroy()
    {
        if (timeManager != null)
            timeManager.TimeChanged -= CheckTime;
    }

    private void CheckTime(int currentHour, int currentMinute)
    {
        if (_hasTriggered) return;
        int currentTime = currentHour * 60 + currentMinute;
        int triggerTime = hour * 60 + minute;

        if (currentTime < triggerTime) return;
        _hasTriggered = true;
        onTimeReached?.Invoke();
    }
    
}
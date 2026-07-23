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
    [SerializeField, Range(0, 23)] private int hour;
    [SerializeField, Range(0, 59)] private int minute;

    [Header("Event")]
    [SerializeField] private UnityEvent onTimeReached;

    private bool _hasTriggered;
    private bool _isSubscribed;

    private void OnEnable() // first attempt
    {
        TrySubscribe();
    }
    
    /// <summary>
    /// // second attempt to subscribe in case it doesnt work the first one.
    /// After puzzles are connected, need to test if 2 subscriptions are needed
    /// </summary>

    private void Start()
    {
        TrySubscribe();
    }

    private void OnDisable()
    {
        if (_isSubscribed && timeManager != null) timeManager.TimeChanged -= CheckTime;
        _isSubscribed = false;
    }

    private void TrySubscribe()
    {
        if (_isSubscribed) return;
        if (timeManager == null) timeManager = GameTimeManager.Instance;
        if (timeManager == null) return;
        

        timeManager.TimeChanged += CheckTime;
        _isSubscribed = true;

        CheckTime(timeManager.CurrentHour, timeManager.CurrentMinute);
    }

    private void CheckTime(int currentHour, int currentMinute)
    {
        
        if (_hasTriggered) return;
        if (currentHour != hour || currentMinute != minute) return;
        
        _hasTriggered = true;
        onTimeReached?.Invoke();
        
    }
}
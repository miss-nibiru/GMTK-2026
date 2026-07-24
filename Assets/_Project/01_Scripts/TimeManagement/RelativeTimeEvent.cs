using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// triggers event/puzzle after a certain amount of in-game minutes
/// workd sof puzzle objects -- select delay in game mins and then
/// connect OnDelayReached to the resulting event
/// Ex - microwave begins running.
/// BeginCountdown() makes OnDelayReached make the thing do an action at 2pm
/// </summary>
public class RelativeTimeEvent : MonoBehaviour
{
    [SerializeField] private GameTimeManager timeManager;
    [SerializeField, Min(1)] private int delayInGameMinutes = 5;
    [SerializeField] private UnityEvent onDelayReached;

    private int _targetTimeInMinutes;
    private bool _isCounting;
    private bool _isSubscribed;

    private void OnEnable()
    {
        TrySubscribe();
    }

    private void Start()
    {
        TrySubscribe();
    }

    private void OnDisable()
    {
        if (_isSubscribed && timeManager != null) timeManager.TimeChanged -= CheckTime;
        _isSubscribed = false;
    }

    public void BeginCountdown()
    {
        TrySubscribe();

        if (timeManager == null)
        {
            return;
        }

        int currentTimeInMinutes =
            timeManager.CurrentHour * 60 +
            timeManager.CurrentMinute;

        _targetTimeInMinutes =
            currentTimeInMinutes + delayInGameMinutes;

        _isCounting = true;
    }

    public void CancelCountdown()
    {
        _isCounting = false;
    }

    private void TrySubscribe()
    {
        if (_isSubscribed)
        {
            return;
        }

        if (timeManager == null)
        {
            timeManager = GameTimeManager.Instance;
        }

        if (timeManager == null)
        {
            return;
        }

        timeManager.TimeChanged += CheckTime;
        _isSubscribed = true;
    }

    private void CheckTime(int currentHour, int currentMinute)
    {
        if (!_isCounting)
        {
            return;
        }

        int currentTimeInMinutes =
            currentHour * 60 +
            currentMinute;

        if (currentTimeInMinutes < _targetTimeInMinutes)
        {
            return;
        }

        _isCounting = false;
        onDelayReached?.Invoke();
    }
}
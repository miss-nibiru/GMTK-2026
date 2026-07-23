using TMPro;
using UnityEngine;

/// <summary>
/// clock always visible to the player
/// </summary>
public class GameClockUI : MonoBehaviour
{
    [SerializeField] private GameTimeManager timeManager;
    [SerializeField] private GameObject clockVisual;
    [SerializeField] private TMP_Text clockText;
    [SerializeField] private bool showClock = true;

    private void OnEnable()
    {
        if (timeManager != null)
        {
            timeManager.TimeChanged += UpdateClock;
            UpdateClock(timeManager.CurrentHour, timeManager.CurrentMinute);
        }

        ApplyVisibility();
    }

    private void OnDisable()
    {
        if (timeManager != null)
        {
            timeManager.TimeChanged -= UpdateClock;
        }
    }

    public void SetClockVisible(bool isVisible)
    {
        showClock = isVisible;
        ApplyVisibility();
    }

    private void ApplyVisibility()
    {
        if (!clockVisual) clockVisual.SetActive(showClock);
    }

    private void UpdateClock(int hour, int minute)
    {
        int twelveHourTime = hour % 12;
        if (twelveHourTime == 0) twelveHourTime = 12;
        
        string period = hour >= 12 ? "PM" : "AM";

        clockText.text = $"{twelveHourTime}:{minute:00} {period}";
    }
}
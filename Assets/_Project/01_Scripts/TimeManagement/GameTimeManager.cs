using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// this tracks the time in the game -- thinking from 9am to 4pm, workday
/// needs to covert real minutes into in-game hours. Other systems communicate via events when tiem changed
/// puzzles can access this - will have relative and real time available
/// stops at 4pm. Every system gets blocked at 4pm.
/// </summary>

public class GameTimeManager : MonoBehaviour
{
    public static GameTimeManager Instance { get; private set; }

    [Header("Day Settings")]
    [SerializeField, Range(0, 23)] private int startHour;
    [SerializeField, Range(1, 24)] private int endHour;
    [SerializeField, Min(0.1f)] private float irlDayMinutes;

    public event Action<int, int> TimeChanged; //event that controls when time changes -- puzzles listen to this
    public event Action DayEnded;

    public int CurrentHour => Mathf.FloorToInt(_currentTimeInMinutes / 60f);
    public int CurrentMinute => Mathf.FloorToInt(_currentTimeInMinutes % 60f);
    public bool IsDayEnded { get; private set; }
    public bool IsPaused => _pauseReasons.Count > 0;

    private readonly HashSet<string> _pauseReasons = new HashSet<string>();

    private float _currentTimeInMinutes;
    private int _lastDisplayedMinute = -1;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        StartNewDay();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void Update()
    {
        if (IsPaused || IsDayEnded)
        {
            return;
        }

        // everything is in minutes here
        float startTime = startHour * 60f;
        float endTime = endHour * 60f;
        float totalDayMinutes = endTime - startTime;
        float realDaySeconds = irlDayMinutes * 60f;

        _currentTimeInMinutes += totalDayMinutes / realDaySeconds * Time.deltaTime; // converts the workday into 

        if (_currentTimeInMinutes >= endTime)
        {
            _currentTimeInMinutes = endTime;
            IsDayEnded = true;

            NotifyTimeChanged();
            DayEnded?.Invoke();
            return;
        }

        NotifyTimeChanged();
    }

    public void PauseTime(string reason)
    {
        if (!string.IsNullOrWhiteSpace(reason))
        {
            _pauseReasons.Add(reason);
        }
    }

    public void ResumeTime(string reason)
    {
        if (!string.IsNullOrWhiteSpace(reason))
        {
            _pauseReasons.Remove(reason);
        }
    }

    private void StartNewDay()
    {
        _currentTimeInMinutes = startHour * 60f;
        IsDayEnded = false;
        _pauseReasons.Clear();
        NotifyTimeChanged();
    }

    private void NotifyTimeChanged()
    {
        int displayedMinute = Mathf.FloorToInt(_currentTimeInMinutes);

        if (displayedMinute == _lastDisplayedMinute)
        {
            return;
        }

        _lastDisplayedMinute = displayedMinute;
        TimeChanged?.Invoke(CurrentHour, CurrentMinute);
    }
}
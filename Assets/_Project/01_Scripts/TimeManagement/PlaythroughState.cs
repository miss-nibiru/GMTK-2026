using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores progress that must survive an hourglass rewind.
/// This is runtime playthrough state, not saved data.
/// </summary>
[DefaultExecutionOrder(-10000)]
public sealed class PlaythroughState : MonoBehaviour
{
    public static PlaythroughState Instance { get; private set; }

    [Header("Persistent Playthrough Progress")]
    [SerializeField] private int loopCount;
    [SerializeField] private bool caseFileViewed;

    [SerializeField] private List<string> discoveredEvidenceIds = new();
    [SerializeField] private List<string> completedPuzzleIds = new();
    [SerializeField] private List<string> unlockedProgressionIds = new();
    [SerializeField] private List<string> answerAttempts = new();

    private readonly HashSet<string> _discoveredEvidenceLookup =
        new(StringComparer.Ordinal);

    private readonly HashSet<string> _completedPuzzleLookup =
        new(StringComparer.Ordinal);

    private readonly HashSet<string> _unlockedProgressionLookup =
        new(StringComparer.Ordinal);

    public int LoopCount => loopCount;
    public bool CaseFileViewed => caseFileViewed;

    public IReadOnlyList<string> DiscoveredEvidenceIds => discoveredEvidenceIds;
    public IReadOnlyList<string> CompletedPuzzleIds => completedPuzzleIds;
    public IReadOnlyList<string> UnlockedProgressionIds => unlockedProgressionIds;
    public IReadOnlyList<string> AnswerAttempts => answerAttempts;

    public event Action NewGameStarted;
    public event Action<int> LoopAdvanced;
    public event Action<string> EvidenceDiscovered;
    public event Action<string> PuzzleCompleted;
    public event Action<string> ProgressionUnlocked;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Bootstrap()
    {
        GetOrCreate();
    }

    public static PlaythroughState GetOrCreate()
    {
        if (Instance != null)
            return Instance;

        GameObject stateObject = new("[Persistent] PlaythroughState");
        return stateObject.AddComponent<PlaythroughState>();
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        RebuildLookups();
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    public void BeginNewGame()
    {
        loopCount = 0;
        caseFileViewed = false;

        discoveredEvidenceIds.Clear();
        completedPuzzleIds.Clear();
        unlockedProgressionIds.Clear();
        answerAttempts.Clear();

        _discoveredEvidenceLookup.Clear();
        _completedPuzzleLookup.Clear();
        _unlockedProgressionLookup.Clear();

        NewGameStarted?.Invoke();
        Debug.Log("New playthrough started. Persistent progress cleared.");
    }

    public void AdvanceLoop()
    {
        loopCount++;
        LoopAdvanced?.Invoke(loopCount);
        Debug.Log($"Rewind started. Persistent loop count: {loopCount}");
    }

    public bool TryDiscoverEvidence(string evidenceId)
    {
        if (!TryNormaliseId(evidenceId, out string validId))
            return false;

        if (!_discoveredEvidenceLookup.Add(validId))
            return false;

        discoveredEvidenceIds.Add(validId);
        EvidenceDiscovered?.Invoke(validId);
        return true;
    }

    public bool HasDiscoveredEvidence(string evidenceId)
    {
        return TryNormaliseId(evidenceId, out string validId) &&
               _discoveredEvidenceLookup.Contains(validId);
    }

    public bool CompletePuzzle(string puzzleId)
    {
        if (!TryNormaliseId(puzzleId, out string validId))
            return false;

        if (!_completedPuzzleLookup.Add(validId))
            return false;

        completedPuzzleIds.Add(validId);
        PuzzleCompleted?.Invoke(validId);
        return true;
    }

    public bool HasCompletedPuzzle(string puzzleId)
    {
        return TryNormaliseId(puzzleId, out string validId) &&
               _completedPuzzleLookup.Contains(validId);
    }

    public bool UnlockProgression(string progressionId)
    {
        if (!TryNormaliseId(progressionId, out string validId))
            return false;

        if (!_unlockedProgressionLookup.Add(validId))
            return false;

        unlockedProgressionIds.Add(validId);
        ProgressionUnlocked?.Invoke(validId);
        return true;
    }

    public bool IsProgressionUnlocked(string progressionId)
    {
        return TryNormaliseId(progressionId, out string validId) &&
               _unlockedProgressionLookup.Contains(validId);
    }

    public void MarkCaseFileViewed()
    {
        caseFileViewed = true;
    }

    public void RecordAnswerAttempt(string answer)
    {
        if (!string.IsNullOrWhiteSpace(answer))
            answerAttempts.Add(answer.Trim());
    }

    private void RebuildLookups()
    {
        _discoveredEvidenceLookup.Clear();
        _completedPuzzleLookup.Clear();
        _unlockedProgressionLookup.Clear();

        foreach (string id in discoveredEvidenceIds)
            if (TryNormaliseId(id, out string validId))
                _discoveredEvidenceLookup.Add(validId);

        foreach (string id in completedPuzzleIds)
            if (TryNormaliseId(id, out string validId))
                _completedPuzzleLookup.Add(validId);

        foreach (string id in unlockedProgressionIds)
            if (TryNormaliseId(id, out string validId))
                _unlockedProgressionLookup.Add(validId);
    }

    private static bool TryNormaliseId(string value, out string validId)
    {
        validId = value?.Trim();
        return !string.IsNullOrEmpty(validId);
    }
}
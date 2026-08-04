using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Displays discovered evidence
/// Slots farther from player are positioned and scaled down to look like a carousel/circular ui
/// </summary>
public class EvidenceCarouselUI : MonoBehaviour
{
    [SerializeField] private EvidenceSlotUI[] visibleSlots;
    [SerializeField] private Button leftArrow;
    [SerializeField] private Button rightArrow;

    [Header("Automatic Layout")]
    [SerializeField] private Vector2 centerPosition;
    [SerializeField, Min(0f)] private float horizontalSpacing;
    [SerializeField, Min(0.01f)] private float centerScale;
    [SerializeField, Range(0f, 1f)]
    
    private float scaleReductionPerStep;
    [SerializeField] private float verticalRisePerStep;
    
    [Header("Carousel Movement")]
    [SerializeField, Min(0.05f)]
    private float movementDuration = 0.35f;

    [SerializeField]
    private AnimationCurve movementCurve =
        AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    private Coroutine _carouselMoveRoutine;

    private EvidenceTracker _tracker;
    private int _selectedEvidenceIndex;

    public EvidenceData SelectedEvidence => 
        _tracker == null || _tracker.DiscoveredEvidence.Count == 0 ? null
            : _tracker.DiscoveredEvidence[_selectedEvidenceIndex];

    public event Action<EvidenceData> SelectionChanged;
    public event Action<EvidenceData> EvidenceOpened;

    private void Awake()
    {
        foreach (EvidenceSlotUI slot in visibleSlots)
        {
            if (slot != null) slot.Clicked += HandleSlotClicked;
        }

        if (leftArrow != null)
        {
            leftArrow.onClick.AddListener(SelectPrevious);
        }

        if (rightArrow != null)
        {
            rightArrow.onClick.AddListener(SelectNext);
        }

        ApplyAutomaticLayout();
    }

    private void OnEnable()
    {
        TryConnectTracker();
        RefreshCarousel();
    }

    private void Start()
    {
        TryConnectTracker();
        RefreshCarousel();
    }

    public void SelectPrevious()
    {
        SelectRelative(-1);
    }

    public void SelectNext()
    {
        SelectRelative(1);
    }

    private void TryConnectTracker()
    {
        if (_tracker != null) return;
        _tracker = EvidenceTracker.Instance;
        if (_tracker != null) _tracker.EvidenceDiscovered += HandleEvidenceDiscovered;
    }

    private void SelectRelative(int direction)
    {
        if (_tracker == null ||
            _tracker.DiscoveredEvidence.Count <= 1 ||
            _carouselMoveRoutine != null)
        {
            return;
        }

        _carouselMoveRoutine =
            StartCoroutine(AnimateCarousel(direction));
    }
    
    private IEnumerator AnimateCarousel(int direction)
{
    direction = direction < 0 ? -1 : 1;

    if (leftArrow != null)
        leftArrow.interactable = false;

    if (rightArrow != null)
        rightArrow.interactable = false;

    int slotCount = visibleSlots.Length;
    int centerSlotIndex = slotCount / 2;

    Vector2[] startPositions = new Vector2[slotCount];
    Vector2[] targetPositions = new Vector2[slotCount];

    Vector3[] startScales = new Vector3[slotCount];
    Vector3[] targetScales = new Vector3[slotCount];

    for (int i = 0; i < slotCount; i++)
    {
        EvidenceSlotUI slot = visibleSlots[i];

        if (slot == null)
            continue;

        RectTransform slotTransform =
            slot.transform as RectTransform;

        if (slotTransform == null)
            continue;

        slot.BeginCarouselMove();

        startPositions[i] =
            slotTransform.anchoredPosition;

        startScales[i] =
            slotTransform.localScale;

        // Next moves the right card towards centre.
        // Previous moves the left card towards centre.
        int targetSlotIndex = i - direction;
        int targetOffset =
            targetSlotIndex - centerSlotIndex;

        int targetDistance =
            Mathf.Abs(targetOffset);

        targetPositions[i] =
            centerPosition +
            new Vector2(
                targetOffset * horizontalSpacing,
                targetDistance * verticalRisePerStep);

        float targetScale = Mathf.Max(
            0.05f,
            centerScale -
            targetDistance * scaleReductionPerStep);

        targetScales[i] =
            Vector3.one * targetScale;
    }

    float elapsed = 0f;

    while (elapsed < movementDuration)
    {
        elapsed += Time.unscaledDeltaTime;

        float progress = Mathf.Clamp01(
            elapsed / movementDuration);

        float easedProgress =
            movementCurve != null
                ? movementCurve.Evaluate(progress)
                : progress;

        for (int i = 0; i < slotCount; i++)
        {
            EvidenceSlotUI slot = visibleSlots[i];

            if (slot == null)
                continue;

            Vector2 position = Vector2.LerpUnclamped(
                startPositions[i],
                targetPositions[i],
                easedProgress);

            Vector3 scale = Vector3.LerpUnclamped(
                startScales[i],
                targetScales[i],
                easedProgress);

            slot.SetCarouselPose(position, scale);
        }

        yield return null;
    }

    _selectedEvidenceIndex = WrapIndex(
        _selectedEvidenceIndex + direction,
        _tracker.DiscoveredEvidence.Count);

    // Restore the permanent slot layout, then update their evidence.
    ApplyAutomaticLayout();
    RefreshCarousel();

    foreach (EvidenceSlotUI slot in visibleSlots)
    {
        if (slot != null)
            slot.EndCarouselMove();
    }

    _carouselMoveRoutine = null;
}

    private void HandleSlotClicked(EvidenceSlotUI clickedSlot)
    {
        if (_tracker == null || clickedSlot.EvidenceData == null) return;
        
        if (clickedSlot.EvidenceData == SelectedEvidence)
        {
            EvidenceOpened?.Invoke(SelectedEvidence);
            return;
        }
        
        IReadOnlyList<EvidenceData> evidence = _tracker.DiscoveredEvidence;

        for (int i = 0; i < evidence.Count; i++)
        {
            if (evidence[i] != clickedSlot.EvidenceData) continue;
            _selectedEvidenceIndex = i;
            RefreshCarousel();
            return;
        }
    }
    
    public bool CenterOnEvidence(EvidenceData evidence)
    {
        if (evidence == null)
            return false;

        TryConnectTracker();

        if (_tracker == null)
            return false;

        IReadOnlyList<EvidenceData> discoveredEvidence =
            _tracker.DiscoveredEvidence;

        for (int i = 0; i < discoveredEvidence.Count; i++)
        {
            EvidenceData discovered = discoveredEvidence[i];

            bool isMatchingEvidence =
                discovered == evidence ||
                string.Equals(
                    discovered.EvidenceId,
                    evidence.EvidenceId,
                    StringComparison.Ordinal);

            if (!isMatchingEvidence)
                continue;

            _selectedEvidenceIndex = i;
            RefreshCarousel();
            return true;
        }

        return false;
    }
    
    public bool PlayCenteredConfirmationShine()
    {
        if (visibleSlots == null || visibleSlots.Length == 0)
            return false;

        int centerSlotIndex = visibleSlots.Length / 2;
        EvidenceSlotUI centerSlot = visibleSlots[centerSlotIndex];

        if (centerSlot == null || centerSlot.EvidenceData == null)
            return false;

        centerSlot.PlayConfirmationShine();
        return true;
    }

    private void HandleEvidenceDiscovered(EvidenceData newEvidence)
    {
        _selectedEvidenceIndex = _tracker.DiscoveredEvidence.Count - 1;
        RefreshCarousel();
    }

    private void RefreshCarousel()
    {
        if (visibleSlots == null || visibleSlots.Length == 0) return;
        int evidenceCount = _tracker != null
                ? _tracker.DiscoveredEvidence.Count
            : 0;

        bool canRotate = evidenceCount > 1;

        if (leftArrow != null) leftArrow.interactable = canRotate;
        if (rightArrow != null) rightArrow.interactable = canRotate;
        

        if (evidenceCount == 0)
        {
            ClearAllSlots();
            SelectionChanged?.Invoke(null);
            return;
        }

        _selectedEvidenceIndex = WrapIndex(
            _selectedEvidenceIndex,
            evidenceCount);

        // how the ui is visible -- the slots get smaller and gets to the sides from centre
        int centerSlotIndex = visibleSlots.Length / 2;
        int leftVisibleCount = Mathf.Min(centerSlotIndex, (evidenceCount - 1) / 2);
        int rightVisibleCount = Mathf.Min(visibleSlots.Length - centerSlotIndex - 1, evidenceCount - 1 - leftVisibleCount);

        for (int slotIndex = 0; slotIndex < visibleSlots.Length; slotIndex++)
        {
            EvidenceSlotUI slot = visibleSlots[slotIndex];

            if (slot == null) continue;
            int offset = slotIndex - centerSlotIndex;

            bool shouldDisplay =
                evidenceCount > visibleSlots.Length ||
                offset >= -leftVisibleCount &&
                offset <= rightVisibleCount;

            if (!shouldDisplay)
            {
                slot.Clear();
                continue;
            }

            int evidenceIndex = WrapIndex(_selectedEvidenceIndex + offset, evidenceCount);
            slot.Display(_tracker.DiscoveredEvidence[evidenceIndex], offset == 0);
        }

        SelectionChanged?.Invoke(SelectedEvidence);
    }

    

    private void ApplyAutomaticLayout()
    {
        if (visibleSlots == null || visibleSlots.Length == 0) return;
        int centerSlotIndex = visibleSlots.Length / 2;

        for (int i = 0; i < visibleSlots.Length; i++)
        {
            if (visibleSlots[i] == null) continue;
            RectTransform slotTransform =
                visibleSlots[i].transform as RectTransform;

            if (slotTransform == null) continue;
            
            int offset = i - centerSlotIndex;
            int distanceFromCenter = Mathf.Abs(offset);

            slotTransform.anchoredPosition =
                centerPosition +
                new Vector2(
                    offset * horizontalSpacing,
                    distanceFromCenter * verticalRisePerStep);

            float slotScale = Mathf.Max(
                0.05f,
                centerScale -
                distanceFromCenter * scaleReductionPerStep);

            slotTransform.localScale =
                Vector3.one * slotScale;
        }
    }

    private static int WrapIndex(int index, int count)
    {
        return (index % count + count) % count;
    }
    
    private void ClearAllSlots()
    {
        
        foreach (EvidenceSlotUI slot in visibleSlots) 
            if (slot != null) slot.Clear();
        
    }
    
    private void OnDestroy()
    {
        foreach (EvidenceSlotUI slot in visibleSlots)
        {
            if (slot != null) slot.Clicked -= HandleSlotClicked;
        }

        if (leftArrow != null)
        {
            leftArrow.onClick.RemoveListener(SelectPrevious);
        }

        if (rightArrow != null)
        {
            rightArrow.onClick.RemoveListener(SelectNext);
        }

        if (_tracker != null)
        {
            _tracker.EvidenceDiscovered -= HandleEvidenceDiscovered;
        }
    }
    
/// <summary>
/// this will make the layout auto update when in unity editor
/// makes the ui easier to design
/// </summary>
#if UNITY_EDITOR
    private void OnValidate()
    {
        ApplyAutomaticLayout();
    }
#endif
}
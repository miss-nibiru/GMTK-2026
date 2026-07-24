using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        if (_tracker == null || _tracker.DiscoveredEvidence.Count <= 1)
            return;

        _selectedEvidenceIndex = WrapIndex(_selectedEvidenceIndex + direction, _tracker.DiscoveredEvidence.Count);
        RefreshCarousel();
        
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

    private void ClearAllSlots()
    {
        
        foreach (EvidenceSlotUI slot in visibleSlots) 
            if (slot != null) slot.Clear();
        
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
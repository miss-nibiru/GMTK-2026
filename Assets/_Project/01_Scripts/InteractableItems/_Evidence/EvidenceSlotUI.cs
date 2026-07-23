using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Items that are created as EvidenceData are
/// added into the carousel slots -- ONLY THESE!
/// Carousel controller will control the not reset of eveidence found
/// </summary>
[RequireComponent(typeof(Button))]
public class EvidenceSlotUI : MonoBehaviour
{
    [SerializeField] private Image evidenceIcon;
    [SerializeField] private Outline selectedOutline;
    private Button _button;
    private EvidenceData _evidenceData;

    public EvidenceData EvidenceData => _evidenceData;
    public event Action<EvidenceSlotUI> Clicked;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(HandleClick);
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveListener(HandleClick);
    }

    public void Display(EvidenceData evidenceData, bool isSelected)
    {
        _evidenceData = evidenceData;
        bool hasEvidence = evidenceData != null;

        evidenceIcon.sprite = hasEvidence ? evidenceData.EvidenceImage : null;
        evidenceIcon.enabled = hasEvidence;
        _button.interactable = hasEvidence;

        if (selectedOutline != null) selectedOutline.enabled = hasEvidence && isSelected;
        
    }

    public void Clear()
    {
        Display(null, false);
    }

    private void HandleClick()
    {
        if (_evidenceData == null) return;
        Clicked?.Invoke(this);
        
    }
}
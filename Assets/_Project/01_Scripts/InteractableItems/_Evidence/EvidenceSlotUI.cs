using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

/// <summary>
/// Items that are created as EvidenceData are
/// added into the carousel slots -- ONLY THESE!
/// Carousel controller will control the not reset of eveidence found
/// </summary>
[RequireComponent(typeof(Button))]
public class EvidenceSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Selected Hover")]
    [SerializeField, Min(1f)] private float hoverScaleMultiplier = 1.08f;
    [SerializeField, Min(0.01f)] private float hoverSpeed = 14f;
    [SerializeField] private Image evidenceIcon;
    [SerializeField] private Outline selectedOutline;
    
    [Header("Discovery Confirmation")]
    [SerializeField, Min(0.05f)]
    private float confirmationShineDuration = 0.75f;

    [SerializeField, Min(1)]
    private int confirmationShinePulses = 2;

    private Coroutine _confirmationShineRoutine;
    
    private Button _button;
    private EvidenceData _evidenceData;
    
    private Vector3 _restingScale;
    private bool _isSelected;
    private bool _pointerOver;
    private bool _hasStarted;
    
    public EvidenceData EvidenceData => _evidenceData;
    public event Action<EvidenceSlotUI> Clicked;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(HandleClick);
    }
    
    private void Start()
    {
        _restingScale = transform.localScale;
        _hasStarted = true;
    }

    private void Update()
    {
        if (!_hasStarted) return;
        Vector3 targetScale = _restingScale;

        if (_isSelected && _pointerOver && _evidenceData != null) targetScale *= hoverScaleMultiplier;
        float smoothing = 1f - Mathf.Exp(-hoverSpeed * Time.unscaledDeltaTime);

        transform.localScale = Vector3.Lerp(
            transform.localScale,
            targetScale,
            smoothing);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _pointerOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _pointerOver = false;
    }

    private void OnDisable()
    {
        _pointerOver = false;
        if (_hasStarted) transform.localScale = _restingScale;
        
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveListener(HandleClick);
    }

    public void Display(EvidenceData evidenceData, bool isSelected)
    {
        _evidenceData = evidenceData;
        bool hasEvidence = evidenceData != null;
        _isSelected = hasEvidence && isSelected;
        if (!_isSelected) _pointerOver = false;
        evidenceIcon.sprite = hasEvidence ? evidenceData.EvidenceImage : null;
        evidenceIcon.enabled = hasEvidence;

        if (selectedOutline != null)
            selectedOutline.enabled = _isSelected;
        
        if (!_button) return;
        _button.interactable = hasEvidence;
    }
    
    public void PlayConfirmationShine()
    {
        if (selectedOutline == null || _evidenceData == null)
            return;

        if (_confirmationShineRoutine != null)
            StopCoroutine(_confirmationShineRoutine);

        _confirmationShineRoutine =
            StartCoroutine(ConfirmationShineRoutine());
    }

    private IEnumerator ConfirmationShineRoutine()
    {
        bool originalEnabled = selectedOutline.enabled;
        Color originalColor = selectedOutline.effectColor;

        float maximumAlpha =
            originalColor.a > 0f ? originalColor.a : 1f;

        selectedOutline.enabled = true;

        float elapsed = 0f;

        while (elapsed < confirmationShineDuration)
        {
            elapsed += Time.unscaledDeltaTime;

            float progress =
                elapsed / confirmationShineDuration;

            float pulse =
                (Mathf.Sin(
                    progress *
                    Mathf.PI *
                    2f *
                    confirmationShinePulses) + 1f) * 0.5f;

            Color shineColor = originalColor;

            shineColor.a = Mathf.Lerp(
                maximumAlpha * 0.15f,
                maximumAlpha,
                pulse);

            selectedOutline.effectColor = shineColor;

            yield return null;
        }

        selectedOutline.effectColor = originalColor;
        selectedOutline.enabled = originalEnabled;
        _confirmationShineRoutine = null;
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
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Opens and closes the evidence ui with tab button
///
/// While open:
/// - Player gameplay controls are disabled.
/// - The cursor is visible and unlocked.
/// - The game clock continues running.
/// </summary>
public class EvidenceMenuController : MonoBehaviour
{
    [SerializeField] private GameObject evidenceUI;
    [SerializeField] private MonoBehaviour[] gameplayControls;

    private bool _isOpen;
    public bool IsOpen => _isOpen;

    private void Start()
    {
        SetEvidenceOpen(false);
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.tabKey.wasPressedThisFrame) 
            ToggleEvidence();
        
    }

    public void ToggleEvidence()
    {
        SetEvidenceOpen(!_isOpen);
    }

    public void OpenEvidence()
    {
        SetEvidenceOpen(true);
    }

    public void CloseEvidence()
    {
        SetEvidenceOpen(false);
    }

    private void SetEvidenceOpen(bool shouldOpen)
    {
        _isOpen = shouldOpen;

        if (evidenceUI != null) evidenceUI.SetActive(shouldOpen);
        

        foreach (MonoBehaviour gameplayControl in gameplayControls) 
            if (gameplayControl != null) gameplayControl.enabled = !shouldOpen;
        
        Cursor.lockState = shouldOpen ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = shouldOpen;
        
    }
}
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controls the case file and evidence menu.
///
/// The opening case file will pause the clock.
/// The Tab evidence menu will not pause the clock.
/// Clock behaviour is connected separately.
/// </summary>
public class EvidenceMenuController : MonoBehaviour
{
    private const string StartupCasePauseReason = "StartupCaseFile";
    [SerializeField] private GameObject evidenceUI;
    
    [SerializeField] private GameObject caseFileClosed;
    [SerializeField] private GameObject caseFileOpened;
    [SerializeField] private GameObject caseButtonClose;
    
    [SerializeField] private MonoBehaviour[] gameplayControls;

    private bool _isOpen;
    private bool _isCaseFileOpen;
    private bool _isStartupCaseFile;
    private bool _isEvidenceDetailsOpen;

    public bool IsOpen =>
        _isOpen || _isEvidenceDetailsOpen;

    private void Start()
    {
        ShowStartupCaseFile();
    }

    private void Update()
    {
        if (_isEvidenceDetailsOpen)
            return;
        
        if (Keyboard.current == null ||
            !Keyboard.current.tabKey.wasPressedThisFrame) 
            return;
        
        if (_isCaseFileOpen) return;
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

    private void ShowStartupCaseFile()
    {
        _isStartupCaseFile = true;
        GameTimeManager.Instance?.PauseTime(StartupCasePauseReason);
        
        
        ShowOpenedCaseFile();
    }

    public void OpenCaseFileFromEvidence()
    {
        _isStartupCaseFile = false;
        ShowOpenedCaseFile();
    }

    private void ShowOpenedCaseFile()
    {
        _isOpen = true;
        _isCaseFileOpen = true;

        caseFileOpened.SetActive(true);
        caseButtonClose.SetActive(true);

        caseFileClosed.SetActive(false);
        evidenceUI.SetActive(false);

        SetMenuOpened(true);
    }

    public void CloseCaseFile()
    {
        if (_isStartupCaseFile)
        {
            GameTimeManager.Instance?.ResumeTime(StartupCasePauseReason);
            _isStartupCaseFile = false;
        }
        
        _isOpen = false;
        _isCaseFileOpen = false;

        caseFileOpened.SetActive(false);
        caseButtonClose.SetActive(false);
        caseFileClosed.SetActive(false);
        evidenceUI.SetActive(false);

        SetMenuOpened(false);
    }

    private void SetEvidenceOpen(bool shouldOpen)
    {
        _isOpen = shouldOpen;
        _isCaseFileOpen = false;

        evidenceUI.SetActive(shouldOpen);
        caseFileClosed.SetActive(shouldOpen);

        caseFileOpened.SetActive(false);
        caseButtonClose.SetActive(false);

        SetMenuOpened(shouldOpen);
    }
    
    public void SetEvidenceDetailsOpen(bool shouldOpen)
    {
        _isEvidenceDetailsOpen = shouldOpen;

        // If the carousel was already open, closing the details
        // returns to it. Otherwise, closing returns to gameplay.
        SetMenuOpened(shouldOpen || _isOpen);
    }

    private void SetMenuOpened(bool shouldOpen)
    {
        foreach (MonoBehaviour gameplayControl in gameplayControls)
        {
            if (gameplayControl != null) gameplayControl.enabled = !shouldOpen;
            
        }

        Cursor.lockState = shouldOpen
            ? CursorLockMode.None
            : CursorLockMode.Locked;

        Cursor.visible = shouldOpen;
    }
}
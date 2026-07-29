using UnityEngine;

public class CaseFileUIController : MonoBehaviour
{
    
    [SerializeField] private GameObject caseFileClosed;
    [SerializeField] private GameObject caseFileOpened;
    [SerializeField] private GameObject caseButtonClose;
    [SerializeField] private GameObject evidenceCarousel;
    
    [Header("Player")]
    [SerializeField] private PlayerController player;
    [SerializeField] private FirstPersonCamera camera;
    [SerializeField] private PlayerInteractionInput playerInput;
    [SerializeField] private PlayerInteractionUI playerUI;
    
    
    // reference to camera and interaction and input?

    private void Start()
    {
        ShowOpenedCaseFile();
    }

    private void ShowOpenedCaseFile()
    {
        
        caseFileOpened.SetActive(true);
        caseButtonClose.SetActive(true);
        
        caseFileClosed.SetActive(false);
        evidenceCarousel.SetActive(false);
        
        SetGameplayEnabled(false);
        
    }
    
    private void SetGameplayEnabled(bool gameplayEnabled)
    {

        camera.enabled = gameplayEnabled;
        playerUI.enabled = gameplayEnabled;
        playerInput.enabled = gameplayEnabled;
        player.enabled = gameplayEnabled;

        Cursor.lockState = gameplayEnabled
            ? CursorLockMode.Locked
            : CursorLockMode.None;

        Cursor.visible = !gameplayEnabled;
    }

    public void CloseCaseFile()
    {
        //this function closes the case file and starts the game
        
        caseFileOpened.SetActive(false);
        caseButtonClose.SetActive(false);
        
        caseFileClosed.SetActive(false);
        evidenceCarousel.SetActive(false);
        
        SetGameplayEnabled(true);
        
        
    }
    
}

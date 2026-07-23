using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EvidenceDetailsUI : MonoBehaviour
{
    [SerializeField] private EvidenceCarouselUI carousel;
    [SerializeField] private GameObject detailsPanel;
    [SerializeField] private Image evidenceImage;
    [SerializeField] private TMP_Text evidenceName;
    [SerializeField] private TMP_Text evidenceDescription;
    [SerializeField] private Button closeButton;

    private void Awake()
    {
        carousel.EvidenceOpened += OpenDetails;
        closeButton.onClick.AddListener(CloseDetails);
        CloseDetails();
    }

    private void OnDisable()
    {
        CloseDetails();
    }

    private void OnDestroy()
    {
        carousel.EvidenceOpened -= OpenDetails;
        closeButton.onClick.RemoveListener(CloseDetails);
    }

    private void OpenDetails(EvidenceData evidence)
    {
        if (evidence == null) return;

        evidenceName.text = evidence.DisplayName;
        evidenceDescription.text = evidence.Description;
        evidenceImage.sprite = evidence.EvidenceImage;
        evidenceImage.enabled = evidence.EvidenceImage != null;

        detailsPanel.SetActive(true);
    }

    public void CloseDetails()
    {
        detailsPanel.SetActive(false);
    }
}
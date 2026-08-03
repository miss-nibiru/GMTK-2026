using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EvidenceDetailsUI : MonoBehaviour
{
    [Header("Existing References")]
    [SerializeField] private EvidenceCarouselUI carousel;
    [SerializeField] private GameObject detailsPanel;
    [SerializeField] private Image evidenceImage;
    [SerializeField] private TMP_Text evidenceName;
    [SerializeField] private TMP_Text evidenceDescription;
    [SerializeField] private Button closeButton;

    [Header("Page References")]
    [SerializeField] private TMP_Text pageHeading;
    [SerializeField] private TMP_Text pageIndicator;
    [SerializeField] private Button previousButton;
    [SerializeField] private Button nextButton;

    private EvidenceData _currentEvidence;
    private int _currentPageIndex;

    private void Awake()
    {
        carousel.EvidenceOpened += OpenDetails;

        closeButton.onClick.AddListener(CloseDetails);
        previousButton.onClick.AddListener(PreviousPage);
        nextButton.onClick.AddListener(NextPage);

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
        previousButton.onClick.RemoveListener(PreviousPage);
        nextButton.onClick.RemoveListener(NextPage);
    }

    private void OpenDetails(EvidenceData evidence)
    {
        if (evidence == null)
            return;

        _currentEvidence = evidence;
        _currentPageIndex = 0;

        evidenceName.text = evidence.DisplayName;
        evidenceImage.sprite = evidence.EvidenceImage;
        evidenceImage.enabled = evidence.EvidenceImage != null;

        detailsPanel.SetActive(true);
        RefreshPage();
    }

    private void PreviousPage()
    {
        if (_currentEvidence == null)
            return;

        _currentPageIndex = Mathf.Max(
            0,
            _currentPageIndex - 1);

        RefreshPage();
    }

    private void NextPage()
    {
        if (_currentEvidence == null)
            return;

        _currentPageIndex = Mathf.Min(
            _currentEvidence.PageCount - 1,
            _currentPageIndex + 1);

        RefreshPage();
    }

    private void RefreshPage()
    {
        if (_currentEvidence == null)
            return;

        evidenceDescription.text =
            _currentEvidence.GetPageBody(_currentPageIndex);

        string heading =
            _currentEvidence.GetPageHeading(_currentPageIndex);

        pageHeading.text = heading;
        pageHeading.gameObject.SetActive(
            !string.IsNullOrWhiteSpace(heading));

        int pageCount = _currentEvidence.PageCount;
        bool hasMultiplePages = pageCount > 1;

        previousButton.gameObject.SetActive(hasMultiplePages);
        nextButton.gameObject.SetActive(hasMultiplePages);
        pageIndicator.gameObject.SetActive(hasMultiplePages);

        previousButton.interactable = _currentPageIndex > 0;
        nextButton.interactable =
            _currentPageIndex < pageCount - 1;

        pageIndicator.text =
            $"{_currentPageIndex + 1} / {pageCount}";
    }

    public void CloseDetails()
    {
        _currentEvidence = null;
        _currentPageIndex = 0;
        detailsPanel.SetActive(false);
    }
}
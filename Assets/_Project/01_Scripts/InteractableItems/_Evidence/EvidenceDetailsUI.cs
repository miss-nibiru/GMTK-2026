using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EvidenceDetailsUI : MonoBehaviour
{
    [Header("Opening Source")]
    [SerializeField] private EvidenceCarouselUI carousel;

    [Header("Full Evidence View")]
    [SerializeField] private GameObject fullView;
    [SerializeField] private Image evidenceImage;
    [SerializeField] private TMP_Text evidenceTitle;
    [SerializeField] private TMP_Text evidenceDescription;

    [Header("Buttons")]
    [SerializeField] private Button closeButton;
    [SerializeField] private Button previousButton;
    [SerializeField] private Button nextButton;

    private EvidenceData _currentEvidence;
    private int _currentPageIndex;

    private void Awake()
    {
        if (carousel != null)
            carousel.EvidenceOpened += OpenDetails;

        if (closeButton != null)
            closeButton.onClick.AddListener(CloseDetails);

        if (previousButton != null)
            previousButton.onClick.AddListener(PreviousPage);

        if (nextButton != null)
            nextButton.onClick.AddListener(NextPage);

        fullView.SetActive(false);
    }

    private void OnDestroy()
    {
        if (carousel != null)
            carousel.EvidenceOpened -= OpenDetails;

        if (closeButton != null)
            closeButton.onClick.RemoveListener(CloseDetails);

        if (previousButton != null)
            previousButton.onClick.RemoveListener(PreviousPage);

        if (nextButton != null)
            nextButton.onClick.RemoveListener(NextPage);
    }

    public void OpenDetails(EvidenceData evidence)
    {
        if (evidence == null)
            return;

        _currentEvidence = evidence;
        _currentPageIndex = 0;

        evidenceTitle.text = evidence.DisplayName;
        evidenceImage.sprite = evidence.EvidenceImage;
        evidenceImage.enabled = evidence.EvidenceImage != null;

        fullView.SetActive(true);
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

        string heading =
            _currentEvidence.GetPageHeading(_currentPageIndex);

        string body =
            _currentEvidence.GetPageBody(_currentPageIndex);

        evidenceDescription.text =
            string.IsNullOrWhiteSpace(heading)
                ? body
                : $"{heading}\n\n{body}";

        int pageCount = _currentEvidence.PageCount;
        bool hasMultiplePages = pageCount > 1;

        previousButton.gameObject.SetActive(hasMultiplePages);
        nextButton.gameObject.SetActive(hasMultiplePages);

        previousButton.interactable =
            _currentPageIndex > 0;

        nextButton.interactable =
            _currentPageIndex < pageCount - 1;
    }

    public void CloseDetails()
    {
        _currentEvidence = null;
        _currentPageIndex = 0;
        fullView.SetActive(false);
    }
}
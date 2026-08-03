using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EvidenceDetailsUI : MonoBehaviour
{
    [Header("Controllers")]
    [SerializeField] private EvidenceCarouselUI carousel;
    [SerializeField] private EvidenceMenuController menuController;

    [Header("Full Evidence View")]
    [SerializeField] private GameObject fullView;
    [SerializeField] private Image evidenceImage;
    [SerializeField] private TMP_Text evidenceTitle;
    [SerializeField] private TMP_Text evidenceDescription;

    [Header("Buttons")]
    [SerializeField] private Button closeButton;
    [SerializeField] private Button previousButton;
    [SerializeField] private Button nextButton;
    
    [SerializeField] private PlayerThoughtsUI playerThoughts;

    private bool _showThoughtAfterClose;
    private EvidenceTracker _tracker;
    private EvidenceData _currentEvidence;
    private int _currentPageIndex;

    private void Awake()
    {
        _tracker = EvidenceTracker.GetOrCreate();
        _tracker.EvidenceDiscovered += HandleFirstDiscovery;

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
        if (_tracker != null)
            _tracker.EvidenceDiscovered -= HandleFirstDiscovery;

        if (carousel != null)
            carousel.EvidenceOpened -= OpenDetails;

        if (closeButton != null)
            closeButton.onClick.RemoveListener(CloseDetails);

        if (previousButton != null)
            previousButton.onClick.RemoveListener(PreviousPage);

        if (nextButton != null)
            nextButton.onClick.RemoveListener(NextPage);
    }

    private void HandleFirstDiscovery(EvidenceData evidence)
    {
        _showThoughtAfterClose = true;
        OpenDetailsInternal(evidence);
    }

    public void OpenDetails(EvidenceData evidence)
    {
        _showThoughtAfterClose = false;
        OpenDetailsInternal(evidence);
    }

    private void OpenDetailsInternal(EvidenceData evidence)
    {
        if (evidence == null)
            return;

        _currentEvidence = evidence;
        _currentPageIndex = 0;

        evidenceTitle.text = evidence.DisplayName;
        evidenceImage.sprite = evidence.EvidenceImage;
        evidenceImage.enabled = evidence.EvidenceImage != null;

        menuController.SetEvidenceDetailsOpen(true);
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
        EvidenceData closedEvidence = _currentEvidence;
        bool shouldShowThought = _showThoughtAfterClose;

        _currentEvidence = null;
        _currentPageIndex = 0;
        _showThoughtAfterClose = false;

        fullView.SetActive(false);
        menuController.SetEvidenceDetailsOpen(false);

        if (shouldShowThought &&
            closedEvidence != null &&
            playerThoughts != null)
        {
            playerThoughts.ShowThought(closedEvidence);
        }
    }
}
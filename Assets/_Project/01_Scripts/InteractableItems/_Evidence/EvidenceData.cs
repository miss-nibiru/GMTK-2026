using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// these will hold all the data and will already be connected
/// to actions and events
/// </summary>

[CreateAssetMenu(fileName = "EvidenceData", menuName = "Scriptable Objects/EvidenceData")]
public class EvidenceData : ScriptableObject
{
    [Serializable] public class EvidencePage
    {
        [SerializeField] private string heading;
        [SerializeField, TextArea(5, 14)] private string body;
        public string Heading => heading;
        public string Body => body;
    }
    

    [Header("Identity")]
    [SerializeField] private string evidenceId;
    [SerializeField] private string displayName;
    [SerializeField] private string interactionDisplayName;
    [SerializeField] private Sprite evidenceImage;
    
    [Header("Pages")]
    [SerializeField] private List<EvidencePage> pages = new();

    [Header("Evidence Carousel")]
    [SerializeField, TextArea(3, 8)] 
    private string description;
    
    [Header("First Discovery")]
    [SerializeField, TextArea(2, 6)] private string detectiveLine;
    [SerializeField] private AudioClip detectiveAudio;
    [SerializeField] private bool thoughtOnly;

    public string EvidenceId => evidenceId;
    public string DisplayName => displayName;

    public string InteractionDisplayName =>
        string.IsNullOrWhiteSpace(interactionDisplayName)
            ? displayName
            : interactionDisplayName;

    public Sprite EvidenceImage => evidenceImage;

    // Keep Description so the current UI does not break.
    public string Description => description;
    public string ShortDescription => description;

    public IReadOnlyList<EvidencePage> Pages => pages;

    public int PageCount =>
        pages != null && pages.Count > 0
            ? pages.Count
            : 1;

    public string DetectiveLine => detectiveLine;
    public bool ThoughtOnly => thoughtOnly;
    public AudioClip DetectiveAudio => detectiveAudio;
    public string GetPageHeading(int pageIndex)
    {
        if (pages == null || pages.Count == 0)
            return string.Empty;

        int validIndex = Mathf.Clamp(
            pageIndex,
            0,
            pages.Count - 1);

        return pages[validIndex].Heading;
    }

    public string GetPageBody(int pageIndex)
    {
        // Existing single-page evidence falls back to its description.
        if (pages == null || pages.Count == 0) return description;
        int validIndex = Mathf.Clamp(pageIndex, 0, pages.Count - 1);
        return pages[validIndex].Body;
        
    }
}

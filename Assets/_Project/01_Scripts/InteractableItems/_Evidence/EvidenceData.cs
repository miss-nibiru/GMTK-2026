using UnityEngine;

/// <summary>
/// these will hold all the data and will already be connected
/// to actions and events
/// </summary>

[CreateAssetMenu(fileName = "EvidenceData", menuName = "Scriptable Objects/EvidenceData")]
public class EvidenceData : ScriptableObject
{
    
    // ID is the item unique identifier in case we have multiple things that are called the same in game
    [SerializeField] private string evidenceId;
    // the name is what player sees in the UI display
    [SerializeField] private string displayName;
    [SerializeField] private Sprite evidenceImage;

    [SerializeField, TextArea(3, 8)]
    private string description;

    public string EvidenceId => evidenceId;
    public string DisplayName => displayName;
    public Sprite EvidenceImage => evidenceImage;
    public string Description => description;
    
}

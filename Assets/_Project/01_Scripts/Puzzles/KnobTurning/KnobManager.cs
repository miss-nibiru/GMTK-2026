using UnityEngine;

public class KnobManager : MonoBehaviour
{
    [SerializeField] private TurnKnob[] knobs;
    private int correctKnobs;
    
    public void CheckKnobs()
    {
        correctKnobs = 0;
        
        foreach (var knob in knobs)
        {
            if (!knob.inRightSpot) continue;

            correctKnobs++;
        }

        if (correctKnobs < knobs.Length) return;
        
        Debug.Log("solved puzzle!");
    }
}

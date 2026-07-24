using UnityEngine;

public class TurnKnob : MonoBehaviour
{
    [SerializeField] private RectTransform body;
    [SerializeField] private int[] correctPos;

    [SerializeField] private KnobManager knobManager;
    
    public bool inRightSpot { get; private set; }
    
    private int rotatePos;
    
    public void OnClick()
    {
        inRightSpot = false;
        
        body.Rotate(new Vector3(0, 0, 90), Space.Self);

        if (rotatePos < 3)
        {
            rotatePos += 1;
        }
        else
        {
            rotatePos = 0;
        }

        foreach (var pos in correctPos)
        {
            if (pos != rotatePos) continue;
            
            inRightSpot = true;
            knobManager.CheckKnobs();
            break;
        }
    }
}

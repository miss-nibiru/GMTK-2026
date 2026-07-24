using UnityEngine;

public class TurnKnob : MonoBehaviour
{
    [SerializeField] private RectTransform body;
    [SerializeField] private int[] correctPos;
    
    private int rotatePos;
    
    public void OnClick()
    {
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
            
        }
    }
}

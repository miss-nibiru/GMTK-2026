using UnityEngine;
using TMPro;

public class SafeInput : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentNum;
    [SerializeField] private int adder;

    private int num;
    
    public void OnClick()
    {
        num += adder;
        
        if (num < 0)
        {
            num = 9;
        }
        else if (num >= 10)
        {
            num = 0;
        }
        
        currentNum.text = num.ToString();
    }
}

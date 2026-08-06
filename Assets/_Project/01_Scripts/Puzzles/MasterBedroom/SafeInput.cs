using TMPro;
using UnityEngine;

public class SafeInput : MonoBehaviour
{
    [SerializeField] private SafeNumbers numbers;
    [SerializeField] private TextMeshProUGUI currentNum;
    [SerializeField] private int adder;

    public void OnClick()
    {
        
        numbers.currentNum += adder;
        if (numbers.currentNum < 0) numbers.currentNum = 9;
        else if (numbers.currentNum >= 10) numbers.currentNum = 0;
        currentNum.text = numbers.currentNum.ToString();
        
        
    }
}
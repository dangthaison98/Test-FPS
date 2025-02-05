using System.Text;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class CurrencyText : MonoBehaviour
{
    private TextMeshProUGUI text;
    
    private readonly StringBuilder tempString = new();
    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        UpdateManager.Instance.currencyAction += UpdateText;
        UpdateText();
    }

    private void UpdateText()
    {
        tempString.Clear();
        tempString.Append("$").Append((int)DataManager.GetCoin());
        text.text = tempString.ToString();
    }
}
using UnityEngine;
using TMPro;

public class UICoin : MonoBehaviour
{
    public TMP_Text coinText;

    private void Start()
    {
        UpdateCoinText();
    }

    public void UpdateCoinText()
    {
        int currentCoins = FindObjectOfType<Manager>().GetCoins();
        if (coinText != null)
        {
            coinText.text = currentCoins.ToString();
        }
        else
        {
            Debug.LogError("Coin Text Reference is missing.");
        }
    }
}
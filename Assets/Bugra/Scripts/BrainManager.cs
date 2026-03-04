using UnityEngine;
using TMPro; // TextMeshPro için gerekli kütüphane

public class CurrencyManager : MonoBehaviour
{
    // Her yerden kolayca ulaþabilmek için Singleton yapýyoruz
    public static CurrencyManager Instance;

    public int totalScrap = 0; // Toplanan parça sayýsý
    public TextMeshProUGUI scrapText; // UI'daki yazýmýz

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        // Oyun baþladýðýnda yazýyý sýfýrla
        UpdateUI();
    }

    public void AddScrap(int amount)
    {
        totalScrap += amount;
        UpdateUI();
    }

    void UpdateUI()
    {
        scrapText.text = "Parça: " + totalScrap;
    }
}
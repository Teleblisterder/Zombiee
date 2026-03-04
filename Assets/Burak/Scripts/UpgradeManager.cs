using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    [Header("Referanslar")]
    public Turret turret; 
    public GameObject upgradePanel;
    public Button openCloseBtn; 

    [Header("Upgrade Butonlarý")]
    public Button fireRateBtn;
    public Button maxAmmoBtn;
    public Button reloadSpeedBtn;

    [Header("Fiyatlar")]
    public int fireRateCost = 50;
    public int ammoCost = 75;
    public int reloadCost = 60;

    void Start()
    {
        
        fireRateBtn.onClick.AddListener(UpgradeFireRate);
        maxAmmoBtn.onClick.AddListener(UpgradeMaxAmmo);
        reloadSpeedBtn.onClick.AddListener(UpgradeReloadSpeed);
        
      
        openCloseBtn.onClick.AddListener(ToggleUpgradeMenu);

        upgradePanel.SetActive(false); 
        UpdateBtnTexts();
    }

    void Update()
    {
        if (upgradePanel.activeSelf)
        {
            CheckButtonInteractable();
        }
    }

   
    public void ToggleUpgradeMenu()
    {
        
        bool isActive = !upgradePanel.activeSelf;
        upgradePanel.SetActive(isActive);

       
        Time.timeScale = isActive ? 0f : 1f;

       
        openCloseBtn.GetComponentInChildren<TextMeshProUGUI>().text = isActive ? "Devam Et" : "Geliştirme";
    }

    void CheckButtonInteractable()
    {
        int currentScrap = CurrencyManager.Instance.totalScrap;
        fireRateBtn.interactable = currentScrap >= fireRateCost;
        maxAmmoBtn.interactable = currentScrap >= ammoCost;
        reloadSpeedBtn.interactable = currentScrap >= reloadCost;
    }

    void UpdateBtnTexts()
    {
        fireRateBtn.GetComponentInChildren<TextMeshProUGUI>().text = fireRateCost.ToString();
        maxAmmoBtn.GetComponentInChildren<TextMeshProUGUI>().text = ammoCost.ToString();
        reloadSpeedBtn.GetComponentInChildren<TextMeshProUGUI>().text = reloadCost.ToString();
    }

    
    void UpgradeFireRate()
    {
        CurrencyManager.Instance.totalScrap -= fireRateCost;
        turret.fireRate -= 0.05f; 
        fireRateCost = Mathf.RoundToInt(fireRateCost * 1.5f);
        FinishTransaction();
    }

    void UpgradeMaxAmmo()
    {
        if (CurrencyManager.Instance.totalScrap >= ammoCost)
        {
            CurrencyManager.Instance.totalScrap -= ammoCost;
            turret.maxAmmo += 15;
            turret.currentAmmo = turret.maxAmmo; 
            ammoCost = Mathf.RoundToInt(ammoCost * 1.4f);
            turret.UpdateUI(); 
            FinishTransaction();
        }
    }

    void UpgradeReloadSpeed()
    {
        if (CurrencyManager.Instance.totalScrap >= reloadCost)
        {
            CurrencyManager.Instance.totalScrap -= reloadCost;
            turret.reloadTime -= 0.5f; // Reload süresini kısalt
            if (turret.reloadTime < 0.5f) turret.reloadTime = 0.5f;
            reloadCost = Mathf.RoundToInt(reloadCost * 1.6f);
            FinishTransaction();
        }
    }

    void FinishTransaction()
    {
        UpdateBtnTexts();
        CurrencyManager.Instance.AddScrap(0);
    }
}
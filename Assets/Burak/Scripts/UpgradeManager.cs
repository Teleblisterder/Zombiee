using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;
    [Header("Referanslar")]
    public Turret turret; 
    public GameObject upgradePanel;
    public Button openCloseBtn; 

    [Header("Geliþtirme Fiyatlarý")]
    public int fireRateCost = 50;
    public int ammoCost = 75;
    public int reloadCost = 60;

    [Header("Yetenek (PowerUp) Fiyatlarý - LD Ayarlarý")]
    public int freezeCost = 100;
    public int fireBombCost = 120;
    public int grenadeCost = 150;
    public int instaKillCost = 300;

    [Header("Upgrade Butonlarý")]
    public Button fireRateBtn;
    public Button maxAmmoBtn;
    public Button reloadSpeedBtn;
    
    [Header("Yetenek Butonlarý")]
    public Button buyFreezeBtn;
    public Button buyFireBtn;
    public Button buyGrenadeBtn;
    public Button buyInstaKillBtn;

    void Start()
    {
      
        fireRateBtn.onClick.AddListener(UpgradeFireRate);
        maxAmmoBtn.onClick.AddListener(UpgradeMaxAmmo);
        reloadSpeedBtn.onClick.AddListener(UpgradeReloadSpeed);
        
       
        buyFreezeBtn.onClick.AddListener(() => BuyPowerUp("Freeze", freezeCost));
        buyFireBtn.onClick.AddListener(() => BuyPowerUp("Fire", fireBombCost));
        buyGrenadeBtn.onClick.AddListener(() => BuyPowerUp("Grenade", grenadeCost));
        buyInstaKillBtn.onClick.AddListener(() => BuyPowerUp("InstaKill", instaKillCost));

        openCloseBtn.onClick.AddListener(ToggleUpgradeMenu);
        upgradePanel.SetActive(false); 
        UpdateBtnTexts();
    }

    void BuyPowerUp(string type, int cost)
    {
        if (CurrencyManager.Instance.totalScrap >= cost)
        {
            CurrencyManager.Instance.totalScrap -= cost;
            
          
            switch (type)
            {
                case "Freeze": PowerUpManager.Instance.SelectFreeze(); break;
                case "Fire": PowerUpManager.Instance.SelectFire(); break;
                case "Grenade": PowerUpManager.Instance.SelectGrenade(); break;
                case "InstaKill": PowerUpManager.Instance.SelectInstaKill(); break;
            }
            
            FinishTransaction();
        }
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
        openCloseBtn.GetComponentInChildren<TextMeshProUGUI>().text = isActive ? "Kapat" : "Geliştirme";
    }

    void CheckButtonInteractable()
    {
        int currentScrap = CurrencyManager.Instance.totalScrap;
        fireRateBtn.interactable = currentScrap >= fireRateCost;
        maxAmmoBtn.interactable = currentScrap >= ammoCost;
        reloadSpeedBtn.interactable = currentScrap >= reloadCost;

       
        buyFreezeBtn.interactable = currentScrap >= freezeCost;
        buyFireBtn.interactable = currentScrap >= fireBombCost;
        buyGrenadeBtn.interactable = currentScrap >= grenadeCost;
        buyInstaKillBtn.interactable = currentScrap >= instaKillCost;
    }

    void UpdateBtnTexts()
    {
        fireRateBtn.GetComponentInChildren<TextMeshProUGUI>().text = fireRateCost.ToString();
        maxAmmoBtn.GetComponentInChildren<TextMeshProUGUI>().text = ammoCost.ToString();
        reloadSpeedBtn.GetComponentInChildren<TextMeshProUGUI>().text = reloadCost.ToString();
        
        buyFreezeBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Dondur: " + freezeCost;
        buyFireBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Alev: " + fireBombCost;
        buyGrenadeBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Bomba: " + grenadeCost;
        buyInstaKillBtn.GetComponentInChildren<TextMeshProUGUI>().text = "YOK ET: " + instaKillCost;
    }

    void UpgradeFireRate()
    {
        if (CurrencyManager.Instance.totalScrap >= fireRateCost)
        {
            CurrencyManager.Instance.totalScrap -= fireRateCost;
            turret.currentWeapon.fireRate -= 0.05f; 
            fireRateCost = Mathf.RoundToInt(fireRateCost * 1.5f);
            FinishTransaction();
        }
    }

    void UpgradeMaxAmmo()
    {
        if (CurrencyManager.Instance.totalScrap >= ammoCost)
        {
            CurrencyManager.Instance.totalScrap -= ammoCost;
            turret.currentWeapon.maxAmmo += 5;
            turret.currentAmmo = turret.currentWeapon.maxAmmo;
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
            turret.currentWeapon.reloadTime -= 0.5f; 
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
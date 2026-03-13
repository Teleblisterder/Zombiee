using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    [Header("Referanslar")]
<<<<<<< HEAD
    public Turret turret; 
    public GameObject upgradePanel;
    public Button openCloseBtn; 

    [Header("Upgrade Butonlarý")]
=======
    public Turret turret;
    public GameObject upgradePanel;
    public Button openCloseBtn;

    [Header("Upgrade Butonları")]
>>>>>>> origin/bugra
    public Button fireRateBtn;
    public Button maxAmmoBtn;
    public Button reloadSpeedBtn;

    [Header("Fiyatlar")]
<<<<<<< HEAD
    public int fireRateCost = 5;
    public int ammoCost = 5;
    public int reloadCost = 5;

    void Start()
    {
        
        fireRateBtn.onClick.AddListener(UpgradeFireRate);
        maxAmmoBtn.onClick.AddListener(UpgradeMaxAmmo);
        reloadSpeedBtn.onClick.AddListener(UpgradeReloadSpeed);
        
      
        openCloseBtn.onClick.AddListener(ToggleUpgradeMenu);

        upgradePanel.SetActive(false); 
=======
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
>>>>>>> origin/bugra
        UpdateBtnTexts();
    }

    void Update()
    {
        if (upgradePanel.activeSelf)
        {
            CheckButtonInteractable();
        }
    }

<<<<<<< HEAD
   
    public void ToggleUpgradeMenu()
    {
        
        bool isActive = !upgradePanel.activeSelf;
        upgradePanel.SetActive(isActive);

        
        openCloseBtn.GetComponentInChildren<TextMeshProUGUI>().text = isActive ? "Kapat" : "Geliştirme";
=======
    public void ToggleUpgradeMenu()
    {
        bool isActive = !upgradePanel.activeSelf;
        upgradePanel.SetActive(isActive);

        Time.timeScale = isActive ? 0f : 1f;

        openCloseBtn.GetComponentInChildren<TextMeshProUGUI>().text = isActive ? "Devam Et" : "Geliştirme";
>>>>>>> origin/bugra
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

<<<<<<< HEAD
    
    void UpgradeFireRate()
    {
        CurrencyManager.Instance.totalScrap -= fireRateCost;
        turret.fireRate -= 0.05f; 
        fireRateCost = Mathf.RoundToInt(fireRateCost * 1.5f);
        FinishTransaction();
=======
    void UpgradeFireRate()
    {
        if (CurrencyManager.Instance.totalScrap >= fireRateCost)
        {
            CurrencyManager.Instance.totalScrap -= fireRateCost;

            // turret.fireRate yerine turret.currentWeapon.fireRate kullanıyoruz
            turret.currentWeapon.fireRate -= 0.05f;

            fireRateCost = Mathf.RoundToInt(fireRateCost * 1.5f);
            FinishTransaction();
        }
>>>>>>> origin/bugra
    }

    void UpgradeMaxAmmo()
    {
        if (CurrencyManager.Instance.totalScrap >= ammoCost)
        {
            CurrencyManager.Instance.totalScrap -= ammoCost;
<<<<<<< HEAD
            turret.maxAmmo += 5;
            turret.currentAmmo = turret.maxAmmo; 
            ammoCost = Mathf.RoundToInt(ammoCost * 1.4f);
            turret.UpdateUI(); 
=======

            // turret.maxAmmo yerine turret.currentWeapon.maxAmmo kullanıyoruz
            turret.currentWeapon.maxAmmo += 15;
            turret.currentAmmo = turret.currentWeapon.maxAmmo;

            ammoCost = Mathf.RoundToInt(ammoCost * 1.4f);
            turret.UpdateUI();
>>>>>>> origin/bugra
            FinishTransaction();
        }
    }

    void UpgradeReloadSpeed()
    {
        if (CurrencyManager.Instance.totalScrap >= reloadCost)
        {
            CurrencyManager.Instance.totalScrap -= reloadCost;
<<<<<<< HEAD
            turret.reloadTime -= 0.5f; // Reload süresini kısalt
            if (turret.reloadTime < 0.5f) turret.reloadTime = 0.5f;
=======

            // turret.reloadTime yerine turret.currentWeapon.reloadTime kullanıyoruz
            turret.currentWeapon.reloadTime -= 0.5f;
            if (turret.currentWeapon.reloadTime < 0.5f) turret.currentWeapon.reloadTime = 0.5f;

>>>>>>> origin/bugra
            reloadCost = Mathf.RoundToInt(reloadCost * 1.6f);
            FinishTransaction();
        }
    }

    void FinishTransaction()
    {
        UpdateBtnTexts();
<<<<<<< HEAD
        CurrencyManager.Instance.AddScrap(0);
=======
        CurrencyManager.Instance.AddScrap(0); // Bu fonksiyon UI'ı tetiklemek için harika düşünülmüş
>>>>>>> origin/bugra
    }
}
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems; // UI tespiti için gerekli (Senin sistemin)

public class Turret : MonoBehaviour
{
    [Header("Silah Ayarları")]
    public WeaponData currentWeapon; // Editörden içine silah dosyasını sürükleyeceğimiz yuva (Arkadaşının sistemi)
    public Transform firePoint;
    public GameObject ts; // Taret kafası

    [Header("UI Referansları")]
    public TextMeshProUGUI ammoText;
    public Slider reloadSlider;

    // Çalışma değişkenleri
    public int currentAmmo;
    private bool isReloading = false;
    private float nextFireTime = 0f;

    private void Start()
    {
        // Arkadaşının sistemi: Oyun başlarken seçili silahın orijinalini bozmamak için klonunu oluştur
        if (currentWeapon != null)
        {
            currentWeapon = Instantiate(currentWeapon);
            currentAmmo = currentWeapon.maxAmmo;
        }
        
        UpdateUI();
        reloadSlider.gameObject.SetActive(false);
    }

    private void Update()
    {
        // SENİN SİSTEMİN: Fare UI üzerindeyse (Geliştirme butonu vb.) taret hiçbir şey yapmaz
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        // Oyun durmuşsa veya reload yapılıyorsa taret işlem yapmaz
        if (Time.timeScale == 0 || isReloading) return;

        RotateTowardsMouse();

        // Şarjör bittiyse veya R'ye basıldıysa Reload başlat (currentWeapon verilerini kullanır)
        if (currentAmmo <= 0 || (Input.GetKeyDown(KeyCode.R) && currentAmmo < currentWeapon.maxAmmo))
        {
            StartCoroutine(Reload());
            return;
        }

        // ARKADAŞININ SİSTEMİ: Silah türüne göre Input belirle (Taramalı mı yarı otomatik mi?)
        bool isShooting = currentWeapon.isAutomatic ? Input.GetMouseButton(0) : Input.GetMouseButtonDown(0);

        if (isShooting && Time.time >= nextFireTime)
        {
            if (currentAmmo > 0)
            {
                Shoot();
                nextFireTime = Time.time + currentWeapon.fireRate;
            }
            else if (Input.GetMouseButtonDown(0)) // SENİN SİSTEMİN: Mermi yoksa tık tık sesi
            {
                AudioManager.Instance.Play("EmptyClick", 0.2f);
            }
        }
    }

    void Shoot()
    {
        currentAmmo--;
        UpdateUI();

     
        AudioManager.Instance.Play(currentWeapon.shootSoundName, 0.07f);
        GameObject firedBullet = Instantiate(currentWeapon.bulletPrefab, firePoint.position, firePoint.rotation);
        
     
        Bullet bulletScript = firedBullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.damage = currentWeapon.damage;
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        AudioManager.Instance.Play("ReloadStart");

        reloadSlider.gameObject.SetActive(true);
        reloadSlider.maxValue = currentWeapon.reloadTime; // Süreyi silahtan çek
        reloadSlider.value = 0;

        float timer = 0;
        while (timer < currentWeapon.reloadTime)
        {
            timer += Time.deltaTime;
            reloadSlider.value = timer;
            yield return null;
        }

        AudioManager.Instance.Play("ReloadEnd");

        currentAmmo = currentWeapon.maxAmmo; // Mermiyi silahtan çek
        isReloading = false;
        reloadSlider.gameObject.SetActive(false);
        UpdateUI();
    }

    public void UpdateUI()
    {
        // Senin düzeltmen: display mermiyi eksiye düşürmez
        int displayAmmo = Mathf.Max(0, currentAmmo);
        if (currentWeapon != null)
            ammoText.text = displayAmmo + " / " + currentWeapon.maxAmmo;
    }

    void RotateTowardsMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - ts.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        ts.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Clamp(angle, -60f, 60f)));
    }
}
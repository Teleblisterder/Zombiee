using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class Turret : MonoBehaviour
{
    [Header("Silah Ayarları")]
    public WeaponData currentWeapon; // Editörden içine silah dosyasını sürükleyeceğimiz yuva
    public Transform firePoint;
    public GameObject ts;


    public int currentAmmo;
    private bool isReloading = false;
    private float nextFireTime = 0f;

    [Header("UI Referansları")]
    public TextMeshProUGUI ammoText;
    public Slider reloadSlider;

    private void Start()
    {
        // Oyun başlarken seçili silahın mermisini doldur
        currentWeapon = Instantiate(currentWeapon);
        currentAmmo = currentWeapon.maxAmmo;
        UpdateUI();
        reloadSlider.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Time.timeScale == 0) return;
        if (isReloading) return;

        RotateTowardsMouse();

        // Şarjör bittiyse veya R'ye basıldıysa
        if (currentAmmo <= 0 || (Input.GetKeyDown(KeyCode.R) && currentAmmo < currentWeapon.maxAmmo))
        {
            StartCoroutine(Reload());
            return;
        }

        // SİLAH TÜRÜNE GÖRE INPUT KONTROLÜ
        // isAutomatic true ise basılı tutmayı (GetMouseButton), false ise tek tıklamayı (GetMouseButtonDown) dinle
        bool isShooting = currentWeapon.isAutomatic ? Input.GetMouseButton(0) : Input.GetMouseButtonDown(0);

        if (isShooting && Time.time >= nextFireTime && currentAmmo > 0)
        {
            Shoot();
            nextFireTime = Time.time + currentWeapon.fireRate;
        }
    }

    void Shoot()
    {
        currentAmmo--;
        UpdateUI();

        // Sesi ve mermiyi silah dosyasından çekiyoruz
        AudioManager.Instance.Play(currentWeapon.shootSoundName, 0.07f);
        GameObject firedBullet = Instantiate(currentWeapon.bulletPrefab, firePoint.position, firePoint.rotation);
        firedBullet.GetComponent<Bullet>().damage = currentWeapon.damage;
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
        int displayAmmo = Mathf.Max(0, currentAmmo);
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
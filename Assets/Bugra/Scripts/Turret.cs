using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
<<<<<<< HEAD
using UnityEngine.EventSystems; // UI tespiti için gerekli

public class Turret : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.8f; 
    public GameObject ts; 

    [Header("Şarjör ve Reload Sistemi")]
    public int maxAmmo = 15;        
    public int currentAmmo;         
    public float reloadTime = 4f;  
    private bool isReloading = false;

    [Header("UI Referansları")]
    public TextMeshProUGUI ammoText; 
    public Slider reloadSlider;    
    private float nextFireTime = 0f;

    private void Start()
    {
        currentAmmo = maxAmmo;
=======

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
>>>>>>> origin/bugra
        UpdateUI();
        reloadSlider.gameObject.SetActive(false);
    }

    private void Update()
    {
<<<<<<< HEAD
       
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

      
        if (isReloading) return;

       
        RotateTowardsMouse();

       
        if (currentAmmo <= 0 || (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo))
=======
        if (Time.timeScale == 0) return;
        if (isReloading) return;

        RotateTowardsMouse();

        // Şarjör bittiyse veya R'ye basıldıysa
        if (currentAmmo <= 0 || (Input.GetKeyDown(KeyCode.R) && currentAmmo < currentWeapon.maxAmmo))
>>>>>>> origin/bugra
        {
            StartCoroutine(Reload());
            return;
        }

<<<<<<< HEAD
     
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime && currentAmmo > 0)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
=======
        // SİLAH TÜRÜNE GÖRE INPUT KONTROLÜ
        // isAutomatic true ise basılı tutmayı (GetMouseButton), false ise tek tıklamayı (GetMouseButtonDown) dinle
        bool isShooting = currentWeapon.isAutomatic ? Input.GetMouseButton(0) : Input.GetMouseButtonDown(0);

        if (isShooting && Time.time >= nextFireTime && currentAmmo > 0)
        {
            Shoot();
            nextFireTime = Time.time + currentWeapon.fireRate;
>>>>>>> origin/bugra
        }
    }

    void Shoot()
    {
<<<<<<< HEAD
        if (currentAmmo > 0)
        {
            currentAmmo--; 
            UpdateUI(); 
          
            AudioManager.Instance.Play("Shoot", 0.07f); 
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
        else
        {
            AudioManager.Instance.Play("EmptyClick", 0.2f);
            currentAmmo = 0; 
            UpdateUI();
        }
=======
        currentAmmo--;
        UpdateUI();

        // Sesi ve mermiyi silah dosyasından çekiyoruz
        AudioManager.Instance.Play(currentWeapon.shootSoundName, 0.07f);
        GameObject firedBullet = Instantiate(currentWeapon.bulletPrefab, firePoint.position, firePoint.rotation);
        firedBullet.GetComponent<Bullet>().damage = currentWeapon.damage;
>>>>>>> origin/bugra
    }

    IEnumerator Reload()
    {
        isReloading = true;
        AudioManager.Instance.Play("ReloadStart");
<<<<<<< HEAD
    
        reloadSlider.gameObject.SetActive(true);
        reloadSlider.maxValue = reloadTime;
        reloadSlider.value = 0;

        float timer = 0;
        while (timer < reloadTime)
        {
            timer += Time.deltaTime;
            reloadSlider.value = timer; 
            yield return null; 
        }

        AudioManager.Instance.Play("ReloadEnd");
        currentAmmo = maxAmmo;
=======

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
>>>>>>> origin/bugra
        isReloading = false;
        reloadSlider.gameObject.SetActive(false);
        UpdateUI();
    }

    public void UpdateUI()
    {
        int displayAmmo = Mathf.Max(0, currentAmmo);
<<<<<<< HEAD
        ammoText.text = displayAmmo + " / " + maxAmmo;
=======
        ammoText.text = displayAmmo + " / " + currentWeapon.maxAmmo;
>>>>>>> origin/bugra
    }

    void RotateTowardsMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - ts.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        ts.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Clamp(angle, -60f, 60f)));
    }
}
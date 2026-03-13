using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems; 

public class Turret : MonoBehaviour
{
    [Header("Silah Ayarları")]
    public WeaponData currentWeapon; 
    public Transform firePoint;
    public GameObject ts; 

    [Header("UI Referansları")]
    public TextMeshProUGUI ammoText;
    public Slider reloadSlider;

    
    public int currentAmmo;
    private bool isReloading = false;
    private float nextFireTime = 0f;

    private void Start()
    {
        
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
       
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

       
        if (Time.timeScale == 0 || isReloading) return;

        RotateTowardsMouse();

       
        if (currentAmmo <= 0 || (Input.GetKeyDown(KeyCode.R) && currentAmmo < currentWeapon.maxAmmo))
        {
            StartCoroutine(Reload());
            return;
        }

       
        bool isShooting = currentWeapon.isAutomatic ? Input.GetMouseButton(0) : Input.GetMouseButtonDown(0);

        if (isShooting && Time.time >= nextFireTime)
        {
            if (currentAmmo > 0)
            {
                Shoot();
                nextFireTime = Time.time + currentWeapon.fireRate;
            }
            else if (Input.GetMouseButtonDown(0)) 
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
        reloadSlider.maxValue = currentWeapon.reloadTime; 
        reloadSlider.value = 0;

        float timer = 0;
        while (timer < currentWeapon.reloadTime)
        {
            timer += Time.deltaTime;
            reloadSlider.value = timer;
            yield return null;
        }

        AudioManager.Instance.Play("ReloadEnd");

        currentAmmo = currentWeapon.maxAmmo; 
        isReloading = false;
        reloadSlider.gameObject.SetActive(false);
        UpdateUI();
    }

    public void UpdateUI()
    {
       
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
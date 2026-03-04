using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

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
        UpdateUI();
        reloadSlider.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Time.timeScale == 0) return; 
        if (isReloading) return;

        RotateTowardsMouse();

        
        if (currentAmmo <= 0 || (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo))
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        currentAmmo--;
        UpdateUI();
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }

    IEnumerator Reload()
    {
        isReloading = true;
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

        currentAmmo = maxAmmo;
        isReloading = false;
        reloadSlider.gameObject.SetActive(false);
        UpdateUI();
    }

    public void UpdateUI()
    {
        ammoText.text = currentAmmo + " / " + maxAmmo;
    }

    void RotateTowardsMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - ts.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        ts.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Clamp(angle, -60f, 60f)));
    }
}
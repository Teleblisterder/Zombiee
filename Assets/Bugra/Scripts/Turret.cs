using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
public class Turret : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate;
    public GameObject ts;

    [Header("Þarjör ve Reload Sistemi")]
    public int maxAmmo = 10;         // Maksimum mermi kapasitesi
    private int currentAmmo;         // O anki mermi sayýmýz
    public float reloadTime = 2f;    // Doldurma süresi (saniye)
    private bool isReloading = false; // Taret þu an dolduruyor mu?

    private float nextFireTime = 0f;

    private void Start()
    {

    }

    private void Update()
    {
        if (isReloading==true)
        {
            return;
        }
        RotateTowardsMouse();


        if (currentAmmo <= 0 || Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime=Time.time+fireRate;
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Þarjör dolduruluyor..."); // Diðer dev UI yapana kadar bunu konsoldan takip edebilirsin

        // Belirtilen süre kadar bekle
        yield return new WaitForSeconds(reloadTime);

        // Bekleme bitince mermiyi fulle ve kilidi aç
        currentAmmo = maxAmmo;
        isReloading = false;
        Debug.Log("Þarjör dolu!");
    }

    void RotateTowardsMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 direction = new Vector2(
            mousePosition.x - ts.transform.position.x,
            mousePosition.y - ts.transform.position.y
        );

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        ts.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Clamp(angle,-60f,60f)));
    }

    void Shoot()
    {
        currentAmmo--;
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}

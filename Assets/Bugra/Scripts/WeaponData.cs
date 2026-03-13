using UnityEngine;

// Bu satýr sayesinde editörde sað týklayýp yeni silahlar oluþturabileceðiz
[CreateAssetMenu(fileName = "Yeni Silah", menuName = "Turret vs Zombies/Silah Verisi")]
public class WeaponData : ScriptableObject
{
    [Header("Temel Ayarlar")]
    public float damage = 1f;
    public string weaponName;
    public bool isAutomatic; // Tikliyse taramalý (basýlý tut), tiksizse tabanca (tek tek bas)

    [Header("Statlar")]
    public int maxAmmo;
    public float fireRate;
    public float reloadTime;


    [Header("Referanslar")]
    public GameObject bulletPrefab; // Tabancanýn mermisi farklý, taramalýnýn farklý olabilir
    public string shootSoundName;   // Sesleri de buradan ayýrabiliriz (Örn: "PistolShoot")
}
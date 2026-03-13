using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager Instance;

  
    public enum PowerUpType { None, Freeze, Fire, Grenade, InstaKill }

    [Header("Mevcut Durum")]
    public PowerUpType selectedPowerUp = PowerUpType.None; 
    [Header("Alev Ayarlar�")]
    public GameObject fireAreaPrefab;
    public Vector3 fireSpawnPosition;

    [Header("Bomba Ayarlar�")]
    public GameObject explosionEffect;
    public float grenadeRadius = 3f;
    public float grenadeDamage = 50f;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Update()
    {
       
        if (UpgradeManager.Instance != null && UpgradeManager.Instance.upgradePanel.activeSelf)
        {
            return; 
        }

       
        if (Input.GetKeyDown(KeyCode.E) && selectedPowerUp != PowerUpType.None)
        {
            ExecutePowerUp();
        }
    }

   

    public void SelectFreeze()
    {
        selectedPowerUp = PowerUpType.Freeze;
        Debug.Log("Dondurma haz�r! Kullanmak i�in E'ye bas.");
    }

    public void SelectFire()
    {
        selectedPowerUp = PowerUpType.Fire;
        Debug.Log("Alev haz�r! Kullanmak i�in E'ye bas.");
    }

    public void SelectGrenade()
    {
        selectedPowerUp = PowerUpType.Grenade;
        Debug.Log("Bomba haz�r! Fare ile ni�an al ve E'ye bas.");
    }

    public void SelectInstaKill()
    {
        selectedPowerUp = PowerUpType.InstaKill;
        Debug.Log("Insta-Kill haz�r! Kullanmak i�in E'ye bas.");
    }


   

    private void ExecutePowerUp()
    {
        
        switch (selectedPowerUp)
        {
            case PowerUpType.Freeze:
                StartCoroutine(FreezeRoutine(5f));
                break;

            case PowerUpType.Fire:
                if (fireAreaPrefab != null) Instantiate(fireAreaPrefab, fireSpawnPosition, Quaternion.identity);
                break;

            case PowerUpType.Grenade:
              
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                ThrowGrenade(mousePos);
                break;

            case PowerUpType.InstaKill:
                ApplyInstaKill();
                break;
        }

        
        selectedPowerUp = PowerUpType.None;
        Debug.Log("G�� kullan�ld�! Yeni bir g�� se�melisin.");
    }


   

    IEnumerator FreezeRoutine(float duration)
    {
        foreach (Zombie z in Zombie.activeZombies)
        {
            if (z != null) z.ApplyFreeze(true);
        }

        yield return new WaitForSeconds(duration);

        foreach (Zombie z in Zombie.activeZombies)
        {
            if (z != null) z.ApplyFreeze(false);
        }
    }

    void ThrowGrenade(Vector2 position)
    {
        if (explosionEffect != null) Instantiate(explosionEffect, position, Quaternion.identity);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, grenadeRadius);
        foreach (Collider2D col in colliders)
        {
            Zombie z = col.GetComponent<Zombie>();
            if (z != null) z.TakeDamage(grenadeDamage);
        }
    }

    void ApplyInstaKill()
    {
        List<Zombie> doomedZombies = new List<Zombie>(Zombie.activeZombies);
        foreach (Zombie z in doomedZombies)
        {
            if (z != null) z.TakeDamage(9999f);
        }
    }
}
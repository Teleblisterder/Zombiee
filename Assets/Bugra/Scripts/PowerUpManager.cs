using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager Instance;

    public enum PowerUpType { None, Freeze, Fire, Grenade, InstaKill }

    [Header("Mevcut Durum")]
    public PowerUpType selectedPowerUp = PowerUpType.None;

    [Header("Alev Ayarlarý")]
    public GameObject fireAreaPrefab;
    public Vector3 fireSpawnPosition;

    [Header("Bomba Ayarlarý")]
    public GameObject grenadePrefab; // ARTIK EFEKT DEÐÝL, BOMBA PREFABI ÝSTÝYORUZ
    public float grenadeSpawnHeight = 10f; // Bombanýn ekranýn ne kadar üstünden düþeceði

    [Header("Ses Efektleri (SFX Ýsimleri)")]
    public string freezeSound = "FreezePower";
    public string fireSound = "FirePower";
    public string throwSound = "ThrowGrenade";
    public string instaKillSound = "NukeSiren";

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Update()
    {
        // Geliþtirme menüsü açýksa E tuþunu dinleme
        if (UpgradeManager.Instance != null && UpgradeManager.Instance.upgradePanel.activeSelf) return;

        if (Input.GetKeyDown(KeyCode.E) && selectedPowerUp != PowerUpType.None)
        {
            ExecutePowerUp();
        }

        // --- GEÇÝCÝ TEST TUÞLARI (UI baðlanana kadar denemek için) ---
        if (Input.GetKeyDown(KeyCode.Alpha1)) SelectFreeze();
        if (Input.GetKeyDown(KeyCode.Alpha2)) SelectFire();
        if (Input.GetKeyDown(KeyCode.Alpha3)) SelectGrenade();
        if (Input.GetKeyDown(KeyCode.Alpha4)) SelectInstaKill();
    }

    // --- SEÇÝM FONKSÝYONLARI ---
    public void SelectFreeze() { selectedPowerUp = PowerUpType.Freeze; Debug.Log("Dondurma hazýr!"); }
    public void SelectFire() { selectedPowerUp = PowerUpType.Fire; Debug.Log("Alev hazýr!"); }
    public void SelectGrenade() { selectedPowerUp = PowerUpType.Grenade; Debug.Log("Bomba hazýr!"); }
    public void SelectInstaKill() { selectedPowerUp = PowerUpType.InstaKill; Debug.Log("Insta-Kill hazýr!"); }

    private void ExecutePowerUp()
    {
        switch (selectedPowerUp)
        {
            case PowerUpType.Freeze:
                if (AudioManager.Instance != null) AudioManager.Instance.Play(freezeSound);
                StartCoroutine(FreezeRoutine(5f));
                break;

            case PowerUpType.Fire:
                if (AudioManager.Instance != null) AudioManager.Instance.Play(fireSound);
                if (fireAreaPrefab != null) Instantiate(fireAreaPrefab, fireSpawnPosition, Quaternion.identity);
                break;

            case PowerUpType.Grenade:
                if (AudioManager.Instance != null) AudioManager.Instance.Play(throwSound);
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                ThrowGrenade(mousePos);
                break;

            case PowerUpType.InstaKill:
                if (AudioManager.Instance != null) AudioManager.Instance.Play(instaKillSound);
                ApplyInstaKill();
                break;
        }

        selectedPowerUp = PowerUpType.None;
    }

    // --- GÜÇ MEKANÝKLERÝ ---

    IEnumerator FreezeRoutine(float duration)
    {
        foreach (Zombie z in Zombie.activeZombies) { if (z != null) z.ApplyFreeze(true); }
        yield return new WaitForSeconds(duration);
        foreach (Zombie z in Zombie.activeZombies) { if (z != null) z.ApplyFreeze(false); }
    }

    void ThrowGrenade(Vector2 targetPosition)
    {
        if (grenadePrefab != null)
        {
            // Bombayý farenin týkladýðý yerin çok yukarýsýnda (gökyüzünde) oluþturuyoruz
            Vector2 spawnPos = new Vector2(targetPosition.x, targetPosition.y + grenadeSpawnHeight);
            GameObject grenadeObj = Instantiate(grenadePrefab, spawnPos, Quaternion.identity);

            // Bombaya "þuraya düþeceksin" hedefini veriyoruz
            grenadeObj.GetComponent<Grenade>().targetPosition = targetPosition;
        }
    }

    void ApplyInstaKill()
    {
        // Koca bir deprem yarat! (0.5 saniye boyunca 0.3 þiddetinde sarsýntý)
        ShakeCamera(0.5f, 0.3f);

        List<Zombie> doomedZombies = new List<Zombie>(Zombie.activeZombies);
        foreach (Zombie z in doomedZombies)
        {
            if (z != null) z.TakeDamage(9999f, false);
        }
    }

    // --- JUICE: KAMERA SARSINTISI SÝSTEMÝ ---

    public void ShakeCamera(float duration, float magnitude)
    {
        StartCoroutine(CameraShakeRoutine(duration, magnitude));
    }

    IEnumerator CameraShakeRoutine(float duration, float magnitude)
    {
        Transform camTransform = Camera.main.transform;
        Vector3 originalPos = camTransform.localPosition;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            // Orijinal pozisyonun etrafýnda rastgele bir nokta seç
            float x = originalPos.x + Random.Range(-1f, 1f) * magnitude;
            float y = originalPos.y + Random.Range(-1f, 1f) * magnitude;

            camTransform.localPosition = new Vector3(x, y, originalPos.z);
            elapsed += Time.deltaTime;

            yield return null; // Bir sonraki frame'e geç
        }

        // Süre bitince kamerayý tam yerine geri koy
        camTransform.localPosition = originalPos;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// --- 1. DALGA VERİ YAPISI (Bu kısım eksik olduğu için hata alıyordun) ---
[System.Serializable]
public class Wave
{
    public string waveName; 
    public int normalZombieCount;
    public int fastZombieCount;
    public int tankZombieCount;
    public float spawnRate; 
}

// --- 2. ANA SPAWNER SINIFI ---
public class WaveSpawner : MonoBehaviour
{
    [Header("Zombi Prefabları")]
    public GameObject normalZombie;
    public GameObject fastZombie;
    public GameObject tankZombie;

    [Header("Dalga Ayarları")]
    public Wave[] waves; 
    public float timeBetweenWaves = 5f; 

    [Header("Spawn Noktası")]
    public float minY; 
    public float maxY; 
    public float spawnXPosition; 

    [Header("UI Referansları")]
    public Slider waveProgressBar;   
    public TextMeshProUGUI waveText; 
    public float barSmoothSpeed = 2f; 

    private int currentWaveIndex = 0;
    private bool isSpawning = false;
    private bool isVictoryTriggered = false;

    private int totalZombiesInLevel = 0;
    private int spawnedZombiesCount = 0;
    private float targetProgress = 0f;

    public static int enemiesAlive = 0;

    void Start()
    {
        // Restart/Başlangıç temizliği
        enemiesAlive = 0;
        spawnedZombiesCount = 0;
        targetProgress = 0f;
        isVictoryTriggered = false;
        currentWaveIndex = 0;

        // 1. Toplam zombiyi hesapla
        totalZombiesInLevel = 0;
        foreach (Wave w in waves)
        {
            totalZombiesInLevel += w.normalZombieCount + w.fastZombieCount + w.tankZombieCount;
        }

        // 2. UI ZORLA SIFIRLA VE AÇ
        if (waveProgressBar != null)
        {
            waveProgressBar.minValue = 0f;
            waveProgressBar.maxValue = 1f; 
            waveProgressBar.value = 0f; // Slider'ın 1'den başlamasını engeller
        }

        if (waveText != null) 
        {
            waveText.gameObject.SetActive(true); // Kapanma sorununu çözer
            waveText.text = "Bölüm: 1 / " + waves.Length;
        }

        StartCoroutine(StartNextWave());
    }

    void Update()
    {
        // 3. BARIN AKICI DOLMASI (Mathf.Lerp ile pürüzsüzleştirildi)
        if (waveProgressBar != null)
        {
            waveProgressBar.value = Mathf.Lerp(waveProgressBar.value, targetProgress, Time.deltaTime * barSmoothSpeed);
        }

        // ZAFER KONTROLÜ
        if (!isSpawning && enemiesAlive <= 0 && currentWaveIndex >= waves.Length)
        {
            if (!isVictoryTriggered)
            {
                isVictoryTriggered = true;
                if(GameFlowManager.Instance != null) GameFlowManager.Instance.ShowVictory();
            }
            return;
        }

        // YENİ DALGA KONTROLÜ
        if (!isSpawning && enemiesAlive <= 0 && currentWaveIndex < waves.Length)
        {
            StartCoroutine(StartNextWave());
        }
    }

    IEnumerator StartNextWave()
    {
        isSpawning = true;
        
        if (waveText != null) 
            waveText.text = "Bölüm: " + (currentWaveIndex + 1) + " / " + waves.Length;

        Wave currentWave = waves[currentWaveIndex];
        yield return new WaitForSeconds(timeBetweenWaves);

        List<GameObject> zombiesToSpawn = new List<GameObject>();
        for (int i = 0; i < currentWave.normalZombieCount; i++) zombiesToSpawn.Add(normalZombie);
        for (int i = 0; i < currentWave.fastZombieCount; i++) zombiesToSpawn.Add(fastZombie);
        for (int i = 0; i < currentWave.tankZombieCount; i++) zombiesToSpawn.Add(tankZombie);

        ShuffleList(zombiesToSpawn);

        foreach (GameObject zombie in zombiesToSpawn)
        {
            SpawnZombie(zombie);
            
            // PROGRESS GÜNCELLEME
            spawnedZombiesCount++;
            if(totalZombiesInLevel > 0)
                targetProgress = (float)spawnedZombiesCount / totalZombiesInLevel;

            yield return new WaitForSeconds(currentWave.spawnRate);
        }

        currentWaveIndex++;
        isSpawning = false;
    }

    void SpawnZombie(GameObject zombiePrefab)
    {
        float randomY = Random.Range(minY, maxY);
        Vector3 spawnPosition = new Vector3(spawnXPosition, randomY, 0f);
        Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);
        enemiesAlive++;
    }

    void ShuffleList(List<GameObject> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            GameObject temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
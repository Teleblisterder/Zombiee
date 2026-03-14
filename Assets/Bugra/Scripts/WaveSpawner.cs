using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class Wave
{
    public string waveName; 
    public int normalZombieCount;
    public int fastZombieCount;
    public int tankZombieCount;
    public float spawnRate; 
}

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
    public float minY = -7f; 
    public float maxY = -2f; 
    public float spawnXPosition = 17f; 

    [Header("UI Referansları")]
    public Slider waveProgressBar;   
    public TextMeshProUGUI waveText; 
    public float barSmoothSpeed = 2f; 

    private int currentWaveIndex = 0;
    private bool isSpawning = false;
    private bool isVictoryTriggered = false;
    private bool isWaitingForPhase2 = false; // Faz kilit değişkeni

    private int totalZombiesInLevel = 0;
    private int spawnedZombiesCount = 0;
    private float targetProgress = 0f;

    public static int enemiesAlive = 0;

    void Start()
    {
        enemiesAlive = 0;
        spawnedZombiesCount = 0;
        targetProgress = 0f;
        isVictoryTriggered = false;
        isWaitingForPhase2 = false;
        currentWaveIndex = 0;

        totalZombiesInLevel = 0;
        foreach (Wave w in waves)
        {
            totalZombiesInLevel += w.normalZombieCount + w.fastZombieCount + w.tankZombieCount;
        }

        if (waveProgressBar != null)
        {
            waveProgressBar.minValue = 0f;
            waveProgressBar.maxValue = 1f; 
            waveProgressBar.value = 0f; 
        }
        
        if (waveText != null) 
        {
            
            waveText.gameObject.SetActive(true); 

          
            if (waveText.transform.parent != null)
            {
                waveText.transform.parent.gameObject.SetActive(true);
            }

           
            waveText.text = "Bölüm: 1 / " + waves.Length;
        
          
            waveText.alpha = 1f; 
        }

        StartCoroutine(StartNextWave());
    }

    void Update()
    {
        if (waveProgressBar != null)
            waveProgressBar.value = Mathf.Lerp(waveProgressBar.value, targetProgress, Time.deltaTime * barSmoothSpeed);

        // 1. KONTROL: FAZ GEÇİŞİ (Sadece 4. Dalga Bittiğinde)
        if (!isSpawning && enemiesAlive <= 0 && currentWaveIndex == 4 && !isVictoryTriggered)
        {
            currentWaveIndex = 5; // <--- KRİTİK: İndexi hemen artır ki bu 'if' bir daha çalışmasın!
            isWaitingForPhase2 = true; 
            GameFlowManager.Instance.ShowVictory(false); 
            return;
        }

        // 2. KONTROL: TÜM OYUN BİTTİĞİNDE
        if (!isSpawning && enemiesAlive <= 0 && currentWaveIndex >= waves.Length && !isVictoryTriggered)
        {
            isVictoryTriggered = true;
            GameFlowManager.Instance.ShowVictory(true); 
            return;
        }

        // 3. KONTROL: NORMAL DALGA BAŞLATMA
        if (!isSpawning && enemiesAlive <= 0 && currentWaveIndex < waves.Length && !isWaitingForPhase2)
        {
            // Burada index zaten artmış olacağı için normal akış devam eder
            StartCoroutine(StartNextWave());
        }
    }


    public void FinishPhaseTransition()
    {
        isWaitingForPhase2 = false; // Kilidi aç, Update otomatik devam eder
    }

    IEnumerator StartNextWave()
    {
        isSpawning = true;
        if (waveText != null) waveText.text = "Bölüm: " + (currentWaveIndex + 1) + " / " + waves.Length;

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
            spawnedZombiesCount++;
            if(totalZombiesInLevel > 0) targetProgress = (float)spawnedZombiesCount / totalZombiesInLevel;
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
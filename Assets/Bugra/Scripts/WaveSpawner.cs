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
    [Header("Zombi Prefablarý")]
    public GameObject normalZombie;
    public GameObject fastZombie;
    public GameObject tankZombie;

    [Header("Dalga Ayarlarý")]
    public Wave[] waves; 
    public float timeBetweenWaves = 5f; 

    [Header("Spawn Noktasý (Y Ekseni Sýnýrlarý)")]
    public float minY; 
    public float maxY; 
    public float spawnXPosition; 

    [Header("UI Referanslarý")]
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
        enemiesAlive = 0;
        isVictoryTriggered = false;

       
        totalZombiesInLevel = 0;
        foreach (Wave w in waves)
        {
            totalZombiesInLevel += w.normalZombieCount + w.fastZombieCount + w.tankZombieCount;
        }

        if (waveProgressBar != null)
        {
            waveProgressBar.maxValue = 1f; 
            waveProgressBar.value = 0f;
        }

        StartCoroutine(StartNextWave());
    }

    void Update()
    {
       
        if (waveProgressBar != null)
        {
            waveProgressBar.value = Mathf.MoveTowards(waveProgressBar.value, targetProgress, Time.deltaTime * (barSmoothSpeed / 10f));
        }

        if (!isSpawning && enemiesAlive <= 0 && currentWaveIndex >= waves.Length)
        {
            if (!isVictoryTriggered)
            {
                isVictoryTriggered = true;
                GameFlowManager.Instance.ShowVictory();
            }
            return;
        }

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
            
           
            spawnedZombiesCount++;
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
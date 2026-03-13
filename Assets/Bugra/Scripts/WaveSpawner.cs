using System.Collections;
using System.Collections.Generic;
using UnityEngine;
<<<<<<< HEAD
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
=======

// Bu sýnýfýn Inspector'da görünmesi için System.Serializable ekliyoruz
[System.Serializable]
public class Wave
{
    public string waveName; // Dalganýn adý (Örn: "Dalga 1")
    public int normalZombieCount;
    public int fastZombieCount;
    public int tankZombieCount;
    public float spawnRate; // Bu dalgada zombilerin çýkma aralýðý (saniye)
>>>>>>> origin/bugra
}

public class WaveSpawner : MonoBehaviour
{
<<<<<<< HEAD
    [Header("Zombi PrefablarÃ½")]
=======
    [Header("Zombi Prefablarý")]
>>>>>>> origin/bugra
    public GameObject normalZombie;
    public GameObject fastZombie;
    public GameObject tankZombie;

<<<<<<< HEAD
    [Header("Dalga AyarlarÃ½")]
    public Wave[] waves; 
    public float timeBetweenWaves = 5f; 

    [Header("Spawn NoktasÃ½ (Y Ekseni SÃ½nÃ½rlarÃ½)")]
    public float minY; 
    public float maxY; 
    public float spawnXPosition; 

    [Header("UI ReferanslarÃ½")]
    public Slider waveProgressBar;   
    public TextMeshProUGUI waveText; 
    public float barSmoothSpeed = 2f;

    private int currentWaveIndex = 0;
    private bool isSpawning = false;
    private bool isVictoryTriggered = false;

   
    private int totalZombiesInLevel = 0;
    private int spawnedZombiesCount = 0;
    private float targetProgress = 0f;

=======
    [Header("Dalga Ayarlarý")]
    public Wave[] waves; // Tüm dalgalarý tutacaðýmýz dizi
    public float timeBetweenWaves = 5f; // Dalgalar arasý bekleme süresi

    [Header("Spawn Noktasý (Y Ekseni Sýnýrlarý)")]
    public float minY; // Zombinin çýkabileceði en alt nokta
    public float maxY; // Zombinin çýkabileceði en üst nokta
    public float spawnXPosition; // Ekranýn sað tarafýndaki X koordinatý

    private int currentWaveIndex = 0;
    private bool isSpawning = false;

    // Sahnede kalan zombileri saymak için statik deðiþken
>>>>>>> origin/bugra
    public static int enemiesAlive = 0;

    void Start()
    {
<<<<<<< HEAD
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

=======
        // Oyun baþladýðýnda ilk dalgayý çaðýr
>>>>>>> origin/bugra
        StartCoroutine(StartNextWave());
    }

    void Update()
    {
<<<<<<< HEAD
       
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
=======
        // Eðer o an spawn iþlemi bittiyse, sahnede zombi kalmadýysa ve tüm dalgalar bitmediyse yeni dalgaya geç
        if (!isSpawning && enemiesAlive == 0 && currentWaveIndex < waves.Length)
>>>>>>> origin/bugra
        {
            StartCoroutine(StartNextWave());
        }
    }

    IEnumerator StartNextWave()
    {
        isSpawning = true;
<<<<<<< HEAD
        
        if (waveText != null) 
            waveText.text = "BÃ¶lÃ¼m: " + (currentWaveIndex + 1) + " / " + waves.Length;

        Wave currentWave = waves[currentWaveIndex];

        yield return new WaitForSeconds(timeBetweenWaves);

        List<GameObject> zombiesToSpawn = new List<GameObject>();
=======
        Wave currentWave = waves[currentWaveIndex];

        // Yeni dalga baþlamadan önce oyuncuya nefes alma süresi ver
        yield return new WaitForSeconds(timeBetweenWaves);

        // Bu dalgada doðacak zombileri bir "havuza" atýyoruz
        List<GameObject> zombiesToSpawn = new List<GameObject>();

>>>>>>> origin/bugra
        for (int i = 0; i < currentWave.normalZombieCount; i++) zombiesToSpawn.Add(normalZombie);
        for (int i = 0; i < currentWave.fastZombieCount; i++) zombiesToSpawn.Add(fastZombie);
        for (int i = 0; i < currentWave.tankZombieCount; i++) zombiesToSpawn.Add(tankZombie);

<<<<<<< HEAD
        ShuffleList(zombiesToSpawn);

        foreach (GameObject zombie in zombiesToSpawn)
        {
            SpawnZombie(zombie);
            
           
            spawnedZombiesCount++;
            targetProgress = (float)spawnedZombiesCount / totalZombiesInLevel;

            yield return new WaitForSeconds(currentWave.spawnRate);
        }

=======
        // Zombiler hep ayný sýrayla (önce normaller, sonra hýzlýlar vs.) gelmesin diye listeyi karýþtýrýyoruz
        ShuffleList(zombiesToSpawn);

        // Zombileri tek tek doður
        foreach (GameObject zombie in zombiesToSpawn)
        {
            SpawnZombie(zombie);
            yield return new WaitForSeconds(currentWave.spawnRate);
        }

        // Tüm zombiler doðdu, dalga indeksini artýr
>>>>>>> origin/bugra
        currentWaveIndex++;
        isSpawning = false;
    }

    void SpawnZombie(GameObject zombiePrefab)
    {
<<<<<<< HEAD
        float randomY = Random.Range(minY, maxY);
        Vector3 spawnPosition = new Vector3(spawnXPosition, randomY, 0f);
        Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);
        enemiesAlive++;
    }

=======
        // Rastgele bir Y yüksekliði belirle
        float randomY = Random.Range(minY, maxY);
        Vector3 spawnPosition = new Vector3(spawnXPosition, randomY, 0f);

        Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);
        enemiesAlive++; // Zombi doðdu, yaþayan zombi sayacýný artýr
    }

    // Listeyi rastgele karýþtýran küçük bir yardýmcý fonksiyon
>>>>>>> origin/bugra
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
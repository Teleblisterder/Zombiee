using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Bu sýnýfýn Inspector'da görünmesi için System.Serializable ekliyoruz
[System.Serializable]
public class Wave
{
    public string waveName; // Dalganýn adý (Örn: "Dalga 1")
    public int normalZombieCount;
    public int fastZombieCount;
    public int tankZombieCount;
    public float spawnRate; // Bu dalgada zombilerin çýkma aralýðý (saniye)
}

public class WaveSpawner : MonoBehaviour
{
    [Header("Zombi Prefablarý")]
    public GameObject normalZombie;
    public GameObject fastZombie;
    public GameObject tankZombie;

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
    public static int enemiesAlive = 0;

    void Start()
    {
        // Oyun baþladýðýnda ilk dalgayý çaðýr
        StartCoroutine(StartNextWave());
    }

    void Update()
    {
        // Eðer o an spawn iþlemi bittiyse, sahnede zombi kalmadýysa ve tüm dalgalar bitmediyse yeni dalgaya geç
        if (!isSpawning && enemiesAlive == 0 && currentWaveIndex < waves.Length)
        {
            StartCoroutine(StartNextWave());
        }
    }

    IEnumerator StartNextWave()
    {
        isSpawning = true;
        Wave currentWave = waves[currentWaveIndex];

        // Yeni dalga baþlamadan önce oyuncuya nefes alma süresi ver
        yield return new WaitForSeconds(timeBetweenWaves);

        // Bu dalgada doðacak zombileri bir "havuza" atýyoruz
        List<GameObject> zombiesToSpawn = new List<GameObject>();

        for (int i = 0; i < currentWave.normalZombieCount; i++) zombiesToSpawn.Add(normalZombie);
        for (int i = 0; i < currentWave.fastZombieCount; i++) zombiesToSpawn.Add(fastZombie);
        for (int i = 0; i < currentWave.tankZombieCount; i++) zombiesToSpawn.Add(tankZombie);

        // Zombiler hep ayný sýrayla (önce normaller, sonra hýzlýlar vs.) gelmesin diye listeyi karýþtýrýyoruz
        ShuffleList(zombiesToSpawn);

        // Zombileri tek tek doður
        foreach (GameObject zombie in zombiesToSpawn)
        {
            SpawnZombie(zombie);
            yield return new WaitForSeconds(currentWave.spawnRate);
        }

        // Tüm zombiler doðdu, dalga indeksini artýr
        currentWaveIndex++;
        isSpawning = false;
    }

    void SpawnZombie(GameObject zombiePrefab)
    {
        // Rastgele bir Y yüksekliði belirle
        float randomY = Random.Range(minY, maxY);
        Vector3 spawnPosition = new Vector3(spawnXPosition, randomY, 0f);

        Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);
        enemiesAlive++; // Zombi doðdu, yaþayan zombi sayacýný artýr
    }

    // Listeyi rastgele karýþtýran küçük bir yardýmcý fonksiyon
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
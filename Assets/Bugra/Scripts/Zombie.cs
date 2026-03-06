using UnityEngine;
using UnityEditor;
using System.Collections;

public class Zombie : MonoBehaviour
{
    [Header("Zombi Statlar�")]
    public float health = 4f;

    [Header("Hareket Ayarlar�")]
    public float moveSpeed = 2f;        // Sola gitme h�z�
    public float dalgaGenligi = 0.5f;  // Ne kadar yukar�/a�a�� sapaca�� (Y�kseklik)
    public float dalgaFrekansi = 4f;    // Dalgalanma h�z� (Ne kadar h�zl� sallanaca��)

    public GameObject Brain;
    private float startY; // Zombinin do�du�u orijinal Y pozisyonu
    private SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        startY = transform.position.y;
        string[] groans = { "Groan1", "Groan2", "Groan3" };
        AudioManager.Instance.PlayRandom(groans, 0.3f);
    }

    private void Update()
    {
        Move();
    }

    void Move()
    {
        float newX = transform.position.x - (moveSpeed * Time.deltaTime);

        float newY = startY + Mathf.Sin(Time.time * dalgaFrekansi) * dalgaGenligi;

        transform.position = new Vector3(newX, newY, transform.position.z);
    }

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        StartCoroutine(HasarEfekti());
        AudioManager.Instance.Play("ZombieHit", 0.1f);
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if(Random.Range(0,5)==2)
        {
            Instantiate(Brain, transform.position,transform.rotation);
        }
        WaveSpawner.enemiesAlive--;
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            TakeDamage(1f);
            Destroy(collision.gameObject);
        }
    }
    IEnumerator HasarEfekti()
    {
        for (int i = 0; i < 2; i++)
        {
            sr.color = Color.red;    // K�rm�z� yap
            yield return new WaitForSeconds(0.1f);
            sr.color = Color.green; // Eskiye d�nd�r
            yield return new WaitForSeconds(0.1f);
        }
    }
}
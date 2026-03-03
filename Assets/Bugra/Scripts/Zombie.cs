using UnityEngine;
using UnityEditor;
using System.Collections;

public class Zombie : MonoBehaviour
{
    [Header("Zombi Statlarý")]
    public float health = 4f;

    [Header("Hareket Ayarlarý")]
    public float moveSpeed = 2f;        // Sola gitme hýzý
    public float dalgaGenligi = 0.5f;  // Ne kadar yukarý/aþaðý sapacaðý (Yükseklik)
    public float dalgaFrekansi = 4f;    // Dalgalanma hýzý (Ne kadar hýzlý sallanacaðý)

    public GameObject Brain;
    private float startY; // Zombinin doðduðu orijinal Y pozisyonu
    private SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        startY = transform.position.y;
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
            sr.color = Color.red;    // Kýrmýzý yap
            yield return new WaitForSeconds(0.1f);
            sr.color = Color.green; // Eskiye döndür
            yield return new WaitForSeconds(0.1f);
        }
    }
}
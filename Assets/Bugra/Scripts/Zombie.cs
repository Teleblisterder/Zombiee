using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    public static List<Zombie> activeZombies = new List<Zombie>();

    [Header("Zombi Statları")]
    public float health = 4f;

    [Header("Hareket Ayarları")]
    public float moveSpeed = 2f;
    public float dalgaGenligi = 0.5f;
    public float dalgaFrekansi = 4f;

    public GameObject Brain;
    private float startY;
    private float originalSpeed; // Dondurma bitince eski hıza dönmek için
    private float originaldalgaFrekansı;
    private SpriteRenderer sr;
    private bool isFlashing = false;

    private void Awake()
    {
        activeZombies.Add(this);
    }
    private void OnDestroy()
    {
        activeZombies.Remove(this);
    }
    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        startY = transform.position.y;
        originalSpeed = moveSpeed;
        originaldalgaFrekansı = dalgaFrekansi;
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
        if (!isFlashing && sr != null)
        {
            StartCoroutine(HasarEfekti());
        }
        AudioManager.Instance.Play("ZombieHit", 0.1f);
        if (health <= 0)
        {
            Die();
        }
    }
    public void ApplyFreeze(bool isFrozen)
    {
        if (sr == null) return;

        if (isFrozen)
        {
            moveSpeed = 0f;
            sr.color = Color.blue;
            dalgaFrekansi = 0;
        }
        else
        {
            dalgaFrekansi = originaldalgaFrekansı;
            moveSpeed = originalSpeed;
            sr.color = Color.white;
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
            Bullet bulletScript=collision.GetComponent<Bullet>();
            TakeDamage(bulletScript.damage);
            Destroy(collision.gameObject);
        }
    }
    IEnumerator HasarEfekti()
    {
        isFlashing = true;
        for (int i = 0; i < 2; i++)
        {
            sr.color = Color.red;    // K�rm�z� yap
            yield return new WaitForSeconds(0.1f);
            sr.color = Color.white; // Eskiye d�nd�r
            yield return new WaitForSeconds(0.1f);
        }
        isFlashing=false;
    }
}
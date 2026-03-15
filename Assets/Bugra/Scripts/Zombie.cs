using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Zombie : MonoBehaviour
{
    public static List<Zombie> activeZombies = new List<Zombie>();

    [Header("Zombi Statları")]
    public float health = 4f;
    public int damageToTurret = 10;

    public enum ZombieType { Normal, Small, Big };
    public ZombieType Ztype;
    public float knockbackForce = 0.4f;

    [Header("Hareket Ayarları")]
    public float moveSpeed = 2f;
    public float dalgaGenligi = 0.5f;
    public float dalgaFrekansi = 4f;

    [Header("Saldırı Ayarları")]
    public float attackInterval = 1.2f;
    private float nextAttackTime;
    private bool isAttacking = false;

    [Header("Referanslar")]
    public GameObject Brain;
    public GameObject head;
    public GameObject deathParticlePrefab;

    private float startY;
    private float originalSpeed;
    private float originalDalgaFrekansi;

    private SpriteRenderer sr;
    private Animator anim;
    private bool isFlashing = false;

    // YENİ: Zombinin donuk olup olmadığını takip eden kilit değişkenimiz
    public bool isFrozen = false;

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
        anim = GetComponent<Animator>();
        startY = transform.position.y;

        originalSpeed = moveSpeed;
        originalDalgaFrekansi = dalgaFrekansi;

        string[] groans = { "Groan1", "Groan2", "Groan3" };
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayRandom(groans, 0.3f);
    }

    private void Update()
    {
        if (health <= 0) return;

        // YENİ: Eğer zombi donmuşsa, aşağıdaki hiçbir şeyi yapma (Hareket etme, saldırma!)
        if (isFrozen) return;

        if (!isAttacking)
        {
            Move();
        }
        else
        {
            if (Time.time >= nextAttackTime)
            {
                Attack();
                nextAttackTime = Time.time + attackInterval;
            }
        }
    }

    void Move()
    {
        float newX = transform.position.x - (moveSpeed * Time.deltaTime);
        float newY = startY + Mathf.Sin(Time.time * dalgaFrekansi) * dalgaGenligi;
        transform.position = new Vector3(newX, newY, transform.position.z);
    }

    void Attack()
    {
        BaseHealth baseHealth = FindObjectOfType<BaseHealth>();
        if (baseHealth != null)
        {
            baseHealth.TakeDamage(damageToTurret);
        }
    }

    public void TakeDamage(float damageAmount, bool applyKnockback = true)
    {
        health -= damageAmount;

        // Büyük (Big) zombi değilse ve itilme isteniyorsa geriye it
        if (applyKnockback && Ztype != ZombieType.Big)
        {
            GetPushedBack(knockbackForce);
        }

        if (!isFlashing && sr != null)
        {
            StartCoroutine(HasarEfekti());
        }

        if (AudioManager.Instance != null)
            AudioManager.Instance.Play("ZombieHit", 0.1f);

        if (health <= 0)
        {
            Die();
        }
    }

    public void ApplyFreeze(bool freezeState)
    {
        if (sr == null) return;

        // Durumu kilit değişkenine aktar
        isFrozen = freezeState;

        if (anim != null)
            anim.speed = isFrozen ? 0f : 1f;

        if (isFrozen)
        {
            moveSpeed = 0f;
            sr.color = Color.blue;
            dalgaFrekansi = 0f;
            nextAttackTime = Time.time + attackInterval;
        }
        else
        {
            dalgaFrekansi = originalDalgaFrekansi;
            moveSpeed = originalSpeed;
            sr.color = Color.white;
        }
    }

    public void GetPushedBack(float pushAmount)
    {
        isAttacking = false;
        
        transform.position = new Vector3(transform.position.x + pushAmount, transform.position.y, transform.position.z);
    }

    void Die()
    {
        if (Random.Range(0, 5) == 2)
        {
            Instantiate(Brain, transform.position, Quaternion.identity);
        }

       
        if (deathParticlePrefab != null && head != null)
        {
            Instantiate(deathParticlePrefab, head.transform.position, Quaternion.identity);
        }

        WaveSpawner.enemiesAlive--;
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            Bullet bulletScript = collision.GetComponent<Bullet>();
            if (bulletScript != null)
                TakeDamage(bulletScript.damage);
            else
                TakeDamage(1f);

            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("Turret"))
        {
            isAttacking = true;
            Rigidbody2D rb = GetComponent<Rigidbody2D>();

            if (rb != null) rb.linearVelocity = Vector2.zero;
        }
    }

    IEnumerator HasarEfekti()
    {
        isFlashing = true;
        for (int i = 0; i < 2; i++)
        {
            if (sr != null) sr.color = Color.red;
            yield return new WaitForSeconds(0.1f);

            
            if (sr != null) sr.color = isFrozen ? Color.blue : Color.white;

            yield return new WaitForSeconds(0.1f);
        }
        isFlashing = false;
    }
}
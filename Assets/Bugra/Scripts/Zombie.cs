using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    // Arkadaşının eklediği global liste (Yetenekler için zombileri takip eder)
    public static List<Zombie> activeZombies = new List<Zombie>();

    [Header("Zombi Statları")]
    public float health = 4f;
    public int damageToTurret = 10; 

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
    private float startY;
    private SpriteRenderer sr;

    // Arkadaşının dondurma ve görsel sistemleri için gereken değişkenler
    private float originalSpeed; 
    private float originalDalgaFrekansi;
    private bool isFlashing = false;

    private void Awake()
    {
        // Zombi doğduğunda listeye ekle
        activeZombies.Add(this);
    }

    private void OnDestroy()
    {
        // Zombi yok olduğunda listeden çıkar
        activeZombies.Remove(this);
    }

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        startY = transform.position.y;
        
        // Hızları yedekle (Freeze bittiğinde dönmek için)
        originalSpeed = moveSpeed;
        originalDalgaFrekansi = dalgaFrekansi;

        // Rastgele ses
        string[] groans = { "Groan1", "Groan2", "Groan3" };
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayRandom(groans, 0.3f);
    }

    void Update()
    {
        if (health <= 0) return;

        // SENİN SİSTEMİN: Eğer saldırıyorsa yürüme, yoksa yürü
        if (!isAttacking)
        {
            Move();
        }
        else
        {
            // Tarete ulaştıysa periyodik vur
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

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        
        // Arkadaşının flashing kontrolü ile senin hasar efektin birleşti
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

    // ARKADAŞININ SİSTEMİ: Dondurma Yeteneği
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
            dalgaFrekansi = originalDalgaFrekansi;
            moveSpeed = originalSpeed;
            sr.color = Color.white;
        }
    }

    // SENİN SİSTEMİN: Geri İtme Yeteneği (Shockwave)
    public void GetPushedBack(float pushAmount)
    {
        isAttacking = false; 
        transform.position = new Vector3(transform.position.x + pushAmount, transform.position.y, transform.position.z);
        startY = transform.position.y; 
    }

    void Die()
    {
        if (Random.Range(0, 5) == 2)
        {
            // İkinizin de instantiate yöntemini birleştirdik
            Instantiate(Brain, transform.position, Quaternion.identity);
        }
        
        WaveSpawner.enemiesAlive--;
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Arkadaşının sistemini koruduk: Mermiden hasar değerini çekiyor
        if (collision.CompareTag("Bullet"))
        {
            Bullet bulletScript = collision.GetComponent<Bullet>();
            if(bulletScript != null)
                TakeDamage(bulletScript.damage);
            else
                TakeDamage(1f); // Script yoksa varsayılan 1 vur
                
            Destroy(collision.gameObject);
        }

        // Senin sistemini koruduk: Tarete çarpınca durup vurmaya başlar
        if (collision.CompareTag("Turret"))
        {
            isAttacking = true;
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            // Unity 6+ versiyonlarında 'linearVelocity' kullanılır, hata verirse 'velocity' yapın.
            if (rb != null) rb.linearVelocity = Vector2.zero; 
        }
    }

    IEnumerator HasarEfekti()
    {
        isFlashing = true;
        for (int i = 0; i < 2; i++)
        {
            sr.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            sr.color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }
        isFlashing = false;
    }
}
<<<<<<< HEAD
using UnityEngine;
using System.Collections;

public class Zombie : MonoBehaviour
{
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
    
    

=======
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
>>>>>>> origin/bugra
    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        startY = transform.position.y;
<<<<<<< HEAD

   
        string[] groans = { "Groan1", "Groan2", "Groan3" };
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayRandom(groans, 0.3f);
    }

    void Update()
    {
        if (health <= 0) return;

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
    public void GetPushedBack(float pushAmount)
    {
        isAttacking = false; 
        transform.position = new Vector3(transform.position.x + pushAmount, transform.position.y, transform.position.z);
    
       
        startY = transform.position.y; 
=======
        originalSpeed = moveSpeed;
        originaldalgaFrekansı = dalgaFrekansi;
        string[] groans = { "Groan1", "Groan2", "Groan3" };
        AudioManager.Instance.PlayRandom(groans, 0.3f);
    }

    private void Update()
    {
        Move();
>>>>>>> origin/bugra
    }

    void Move()
    {
        float newX = transform.position.x - (moveSpeed * Time.deltaTime);
<<<<<<< HEAD
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
=======

        float newY = startY + Mathf.Sin(Time.time * dalgaFrekansi) * dalgaGenligi;

        transform.position = new Vector3(newX, newY, transform.position.z);
>>>>>>> origin/bugra
    }

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
<<<<<<< HEAD
        StartCoroutine(HasarEfekti());
        
        if (AudioManager.Instance != null)
            AudioManager.Instance.Play("ZombieHit", 0.1f);

=======
        if (!isFlashing && sr != null)
        {
            StartCoroutine(HasarEfekti());
        }
        AudioManager.Instance.Play("ZombieHit", 0.1f);
>>>>>>> origin/bugra
        if (health <= 0)
        {
            Die();
        }
    }
<<<<<<< HEAD

    void Die()
    {
     
        if (Random.Range(0, 5) == 2)
        {
            Instantiate(Brain, transform.position, Quaternion.identity);
        }

       
=======
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
>>>>>>> origin/bugra
        WaveSpawner.enemiesAlive--;
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
<<<<<<< HEAD
        
        if (collision.CompareTag("Bullet"))
        {
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
      
        sr.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sr.color = Color.white;
=======
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
>>>>>>> origin/bugra
    }
}
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
    
    

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        startY = transform.position.y;

   
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
        StartCoroutine(HasarEfekti());
        
        if (AudioManager.Instance != null)
            AudioManager.Instance.Play("ZombieHit", 0.1f);

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
     
        if (Random.Range(0, 5) == 2)
        {
            Instantiate(Brain, transform.position, Quaternion.identity);
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
    }
}
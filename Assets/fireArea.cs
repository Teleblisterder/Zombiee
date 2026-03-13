using UnityEngine;

public class FireArea : MonoBehaviour
{
    public float lifetime = 7f; 
    public float damagePerSecond = 2f; 

    void Start()
    {
       
        Destroy(gameObject, lifetime);
    }

    
    void OnTriggerStay2D(Collider2D collision)
    {
       
        Zombie z = collision.GetComponent<Zombie>();
        if (z != null)
        {
           
            z.TakeDamage(damagePerSecond * Time.deltaTime);
        }
    }
}
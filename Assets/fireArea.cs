using UnityEngine;

public class FireArea : MonoBehaviour
{
    public float lifetime = 7f; // Alev ne kadar yerde kalacak
    public float damagePerSecond = 2f; // Saniyede vuracaðý hasar

    void Start()
    {
        // Süre dolunca alevi yok et
        Destroy(gameObject, lifetime);
    }

    // Ýçinde duran objelere sürekli çalýþýr
    void OnTriggerStay2D(Collider2D collision)
    {
        // "Bullet" kodunda yaptýðýmýz gibi GetComponent ile zombiyi buluyoruz
        Zombie z = collision.GetComponent<Zombie>();
        if (z != null)
        {
<<<<<<< Updated upstream
            // Time.deltaTime ile saniyeye bölerek yumuþak bir hasar veriyoruz (DoT)
            z.TakeDamage(damagePerSecond * Time.deltaTime);
=======
           
            z.TakeDamage(damagePerSecond * Time.deltaTime, false);
>>>>>>> Stashed changes
        }
    }
}
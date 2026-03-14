using UnityEngine;

public class Grenade : MonoBehaviour
{
    [HideInInspector] public Vector2 targetPosition; // Nereye düþeceði (Manager'dan gelecek)

    [Header("Bomba Ayarlarý")]
    public float fallSpeed = 10f;
    public float explosionRadius = 3f;
    public float damage = 50f;
    public GameObject explosionEffect;
    public string explosionSound = "Explosion"; // AudioManager'daki sesin adý

    void Update()
    {
        // Bombayý hedefe doðru hareket ettir
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, fallSpeed * Time.deltaTime);

        // Hedefe ulaþtýysa patla
        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            Explode();
        }
    }

    void Explode()
    {
        // 1. Görsel Efekt
        if (explosionEffect != null)
        {
            GameObject effect= Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(effect,0.6f);
        }


        // 2. Ses Efekti (Diðer dev'in sistemiyle)
        if (AudioManager.Instance != null)
            AudioManager.Instance.Play(explosionSound);

        // 3. Alan Hasarý
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (Collider2D col in colliders)
        {
            Zombie z = col.GetComponent<Zombie>();
            if (z != null) z.TakeDamage(damage);
        }

        // 4. Kamerayý Hafif Sars (Opsiyonel ama çok tatlý olur)
        if (PowerUpManager.Instance != null)
            PowerUpManager.Instance.ShakeCamera(0.2f, 0.1f);

        // Bombayý yok et
        Destroy(gameObject);
    }
}
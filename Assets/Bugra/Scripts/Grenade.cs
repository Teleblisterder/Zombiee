using UnityEngine;

public class Grenade : MonoBehaviour
{
    [HideInInspector] public Vector2 targetPosition;

    [Header("Bomba Ayarları")]
    public float fallSpeed = 12f; // Biraz hızlandırdım daha tok dursun
    public float explosionRadius = 4f;
    public float damage = 50f;
    public GameObject explosionEffect;
    public string explosionSound = "Explosion"; 

    void Update()
    {
        // Bombayı hedefe doğru hareket ettir
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, fallSpeed * Time.deltaTime);

        // Hedefe ulaştıysa patla
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
            GameObject effect = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(effect, 1f);
        }

        // 2. Ses
        if (AudioManager.Instance != null)
            AudioManager.Instance.Play(explosionSound);

        // 3. Alan Hasarı (Sadece zombilere vurduğundan emin oluyoruz)
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (Collider2D col in colliders)
        {
            if (col.CompareTag("Enemy"))
            {
                Zombie z = col.GetComponent<Zombie>();
                // Bombadan kaçış yok, knockback her zaman aktif (true)
                if (z != null) z.TakeDamage(damage, true);
            }
        }

        // 4. Kamera Sarsıntısı
        if (PowerUpManager.Instance != null)
            PowerUpManager.Instance.ShakeCamera(0.4f, 0.3f);
        Debug.Log("BOOM! Patlama gerçekleşti.");
        Destroy(gameObject);
    }
}
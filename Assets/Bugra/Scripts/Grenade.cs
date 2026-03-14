using UnityEngine;

public class Grenade : MonoBehaviour
{
    [HideInInspector] public Vector2 targetPosition; // Nereye d??ece?i (Manager'dan gelecek)

    [Header("Bomba Ayarlar?")]
    public float fallSpeed = 10f;
    public float explosionRadius = 3f;
    public float damage = 50f;
    public GameObject explosionEffect;
    public string explosionSound = "Explosion"; // AudioManager'daki sesin ad?

    void Update()
    {
        // Bombay? hedefe do?ru hareket ettir
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, fallSpeed * Time.deltaTime);

        // Hedefe ula?t?ysa patla
        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            Explode();
        }
    }

    void Explode()
    {
        // 1. G?rsel Efekt
        if (explosionEffect != null)
        {
            GameObject effect = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(effect, 0.6f);
        }


        // 2. Ses Efekti (Di?er dev'in sistemiyle)
        if (AudioManager.Instance != null)
            AudioManager.Instance.Play(explosionSound);

        // 3. Alan Hasar?
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (Collider2D col in colliders)
        {
            Zombie z = col.GetComponent<Zombie>();
            if (z != null) z.TakeDamage(damage);
        }

        // 4. Kameray? Hafif Sars (Opsiyonel ama ?ok tatl? olur)
        if (PowerUpManager.Instance != null)
            PowerUpManager.Instance.ShakeCamera(0.2f, 0.1f);

        // Bombay? yok et
        Destroy(gameObject);
    }
}
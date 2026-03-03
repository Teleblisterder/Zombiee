using UnityEngine;

public class Loot : MonoBehaviour
{
    public int value = 1; // Her bir parçanýn deðeri

    private void Start()
    {
        // Zombi öldüðünde parça yere dümdüz düþmesin, rastgele bir yöne hafifçe fýrlasýn
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            float randomX = Random.Range(-2f, 2f); // Saða veya sola
            rb.AddForce(new Vector2(randomX, 3f), ForceMode2D.Impulse); // Yukarý doðru fýrlat
        }
    }

    // Fareyle (veya mobilde parmakla) objenin üzerine týklandýðýnda çalýþýr
    private void OnMouseDown()
    {
        // Parayý ekle
        CurrencyManager.Instance.AddScrap(value);

        // Objeyi yok et (Ýstersen buraya küçük bir toplanma sesi/efekti de ekleyebilirsin)
        Destroy(gameObject);
    }
}
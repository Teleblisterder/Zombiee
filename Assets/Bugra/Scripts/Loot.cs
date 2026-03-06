using UnityEngine;

public class Loot : MonoBehaviour
{
    public int value = 1; // Her bir par�an�n de�eri

    private void Start()
    {
        // Zombi �ld���nde par�a yere d�md�z d��mesin, rastgele bir y�ne hafif�e f�rlas�n
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            float randomX = Random.Range(-2f, 2f); // Sa�a veya sola
            rb.AddForce(new Vector2(randomX, 3f), ForceMode2D.Impulse); // Yukar� do�ru f�rlat
        }
    }

    // Fareyle (veya mobilde parmakla) objenin �zerine t�kland���nda �al���r
    private void OnMouseDown()
    {
        // Paray� ekle
        CurrencyManager.Instance.AddScrap(value);
        AudioManager.Instance.Play("ScrapCollect", 0.05f);

        // Objeyi yok et (�stersen buraya k���k bir toplanma sesi/efekti de ekleyebilirsin)
        Destroy(gameObject);
    }
}
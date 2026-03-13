using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BaseHealth : MonoBehaviour
{
    [Header("Can Ayarlarý")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("UI Referanslarý")]
    public Slider healthSlider;     
    public TextMeshProUGUI healthText;

    private bool isGameOver = false;

    void Start()
    {
        currentHealth = maxHealth;
        
        // UI Baþlangýç Ayarlarý
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = maxHealth;
        }
        UpdateUI();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug.Log("Temas: " + collision.gameObject.name);

        if (collision.CompareTag("Enemy") && !isGameOver)
        {
          
        }
    }

    public void TakeDamage(int amount)
    {
        if (isGameOver) return;

        currentHealth -= amount;
        if (currentHealth <= 0) currentHealth = 0;

        // SES: Hasar alma sesi
        if (AudioManager.Instance != null)
            AudioManager.Instance.Play("TurretDamage", 0.15f);
       
        // SENİN SİSTEMİN: İlk hasarda Slow-Mo Tutorial'ı başlatır
        if (AbilityManager.Instance != null)
        {
            AbilityManager.Instance.StartTutorial();
        }

        UpdateUI();

        if (currentHealth <= 0)
        {
            GameOver();
        }
    }

    void UpdateUI()
    {
        if (healthSlider != null) healthSlider.value = currentHealth;
        if (healthText != null) healthText.text = currentHealth + " / " + maxHealth;
    }

    void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;
        
        // GameFlowManager'daki fonksiyonu çağırıyoruz
        if (GameFlowManager.Instance != null)
            GameFlowManager.Instance.ShowGameOver();
    }
}
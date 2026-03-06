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
       
        Debug.Log("Bir şey çarptı: " + collision.gameObject.name + " Tag: " + collision.tag);

        if (collision.CompareTag("Enemy") && !isGameOver)
        {
            TakeDamage(10);
        
           
            WaveSpawner.enemiesAlive--;
            Destroy(collision.gameObject);
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0) currentHealth = 0;
        AudioManager.Instance.Play("TurretDamage", 0.15f);

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
        isGameOver = true;
        // GameFlowManager'daki fonksiyonu çağırıyoruz
        GameFlowManager.Instance.ShowGameOver();
    
    }
}
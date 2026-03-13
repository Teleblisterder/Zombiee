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
        
     
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = maxHealth;
        }
        UpdateUI();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
      

        if (collision.CompareTag("Enemy") && !isGameOver)
        {
          
        }
    }

    public void TakeDamage(int amount)
    {
        if (isGameOver) return;

        currentHealth -= amount;
        if (currentHealth <= 0) currentHealth = 0;

       
        if (AudioManager.Instance != null)
            AudioManager.Instance.Play("TurretDamage", 0.15f);
       
        
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
        
      
        if (GameFlowManager.Instance != null)
            GameFlowManager.Instance.ShowGameOver();
    }
}
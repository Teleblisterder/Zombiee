using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro; // Metin kontrolü için şart

public class GameFlowManager : MonoBehaviour
{
    public static GameFlowManager Instance;

    [Header("Paneller")]
    public GameObject gameOverPanel;
    public GameObject victoryPanel;

    [Header("Victory Panel Dinamik İçerik")]
    public TextMeshProUGUI victoryTitleText;  // Paneldeki ana başlık
    public TextMeshProUGUI victoryButtonText; // Butonun üzerindeki yazı

    [Header("Butonlar")]
    public Button restartBtn;        // GameOver butonu
    public Button victoryActionBtn;  // Victory panelindeki ana buton

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        
        gameOverPanel.SetActive(false);
        victoryPanel.SetActive(false);

        // GameOver butonu her zaman sahneyi yeniden yükler
        restartBtn.onClick.AddListener(RestartGame);
    }

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    // isFinalVictory: true ise oyun biter, false ise 2. faza geçer
    public void ShowVictory(bool isFinalVictory)
    {
        victoryPanel.SetActive(true);
        Time.timeScale = 0f;

        victoryActionBtn.onClick.RemoveAllListeners();

        if (isFinalVictory)
        {
            victoryTitleText.text = "OYUN BİTTİ!";
            victoryButtonText.text = "TEKRAR DENE";
            victoryActionBtn.onClick.AddListener(RestartGame);
        }
        else
        {
            // İSTEDİĞİN KISA METİN:
            victoryTitleText.text = "2. FAZA GEÇİLİYOR...";
            victoryButtonText.text = "DEVAM ET";
            victoryActionBtn.onClick.AddListener(ContinueToPhase2);
        }
    }

    void ContinueToPhase2()
    {
        // Logları temizlemek ve sistemi uyandırmak için
        Debug.Log("Butona Basıldı, Silah Değişiyor.");
    
        Time.timeScale = 1f;
        victoryPanel.SetActive(false);
    
        Turret turret = FindObjectOfType<Turret>();
        if (turret != null) turret.EvolveToAutomatic();
    
        WaveSpawner spawner = FindObjectOfType<WaveSpawner>();
        if (spawner != null) spawner.FinishPhaseTransition();
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
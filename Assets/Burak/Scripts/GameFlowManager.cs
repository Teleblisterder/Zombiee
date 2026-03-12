using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 

public class GameFlowManager : MonoBehaviour
{
  
    public static GameFlowManager Instance;

    [Header("Paneller")]
    public GameObject gameOverPanel;
    public GameObject victoryPanel;

    [Header("Butonlar")]
    public Button restartBtn;       
    public Button victoryRestartBtn; 
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
       
        restartBtn.onClick.AddListener(RestartGame);
        victoryRestartBtn.onClick.AddListener(RestartGame);

      
        gameOverPanel.SetActive(false);
        victoryPanel.SetActive(false);
    }



    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ShowVictory()
    {
        victoryPanel.SetActive(true);
        Time.timeScale = 0f; 
    }

   
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
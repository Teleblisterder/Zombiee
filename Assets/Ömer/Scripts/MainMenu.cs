using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject creditPanel;
    public void OpenCreditPanel()
    {
        creditPanel.SetActive(true);
    }

    public void CloseCreditPanel()
    {
        creditPanel.SetActive(false);
    }

    public void NextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);

            
            }

    public void QuitApp()
    {
        Application.Quit();
    }

    
}

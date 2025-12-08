using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string gameScene;
    [SerializeField] private string creditsScene;

    
    public void StartGame()
    {
        LifeManager.currentLives = 3;
        SceneManager.LoadScene(gameScene);
    }

    public void ViewCredits()
    {
        SceneManager.LoadScene(creditsScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

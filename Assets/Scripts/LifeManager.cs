using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LifeManager : MonoBehaviour
{
    public TMP_Text livesText;

    // Esta variable se conserva entre escenas
    public static int currentLives;

    public static LifeManager instance;

    void Awake()
    {
        // Singleton para mantener una sola instancia persistente
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            // Solo inicializamos la primera vez
            if (currentLives == 0)
            {
                currentLives = 3;
            }
        }
        else
        {
            Destroy(gameObject); // Evita duplicados
        }
    }

    void Start()
    {
        UpdateLivesUI();
    }

    public void TakeDamage()
    {
        currentLives--;
        UpdateLivesUI();

        if (currentLives <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Aquí puedes cargar una escena de Game Over o reiniciar el juego
        SceneManager.LoadScene("MenuMain");
    }

    void UpdateLivesUI()
    {
        if (livesText != null)
        {
            livesText.text = "Vidas: " + currentLives;
        }
    }
}
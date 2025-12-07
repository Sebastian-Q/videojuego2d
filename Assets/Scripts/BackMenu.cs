using UnityEngine;
using UnityEngine.SceneManagement;

public class BackMenu : MonoBehaviour
{
    [SerializeField] private string backMenu;

    public void BackInitialMenu()
    {
        SceneManager.LoadScene(backMenu);
    }

}

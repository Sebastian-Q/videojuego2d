using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class pointslife : MonoBehaviour
{
    public TMP_Text livesText;
    public LifeManager life;

    void Update()
    {
        if (life != null && livesText != null)
    {
        livesText.text = LifeManager.currentLives.ToString(); 
    }

    }

}

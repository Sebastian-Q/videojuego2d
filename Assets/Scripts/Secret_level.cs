using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SecretLevel : MonoBehaviour
{
    public Animator chestAnimator;
    public string nombreNivelSecreto = "Secret_Level";
    public float esperaAnimacion = 1.5f;

    private static readonly int OpenChestHash = Animator.StringToHash("OpenChest");
    private int index;
    private bool nivelActivado = false;
    
    private bool jugadorCerca = false; 

    private KeyCode[] cheatCode = new KeyCode[]
    {
        KeyCode.W, KeyCode.S,
        KeyCode.W, KeyCode.S,
        KeyCode.A, KeyCode.D,
        KeyCode.A, KeyCode.D,
        KeyCode.Q, KeyCode.E
    };

    void Update()
    {
        if (!jugadorCerca || nivelActivado) return;

        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(cheatCode[index]))
            {
                index++;
                if (index >= cheatCode.Length)
                {
                    Debug.Log("¡Cofre abierto!");
                    StartCoroutine(AbrirCofreYViajar());
                    index = 0; 
                }
            }
            else
            {
                index = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            jugadorCerca = true;
            Debug.Log("Jugador cerca del cofre. ¡Introduce el código!");
        }
    }

    // NUEVO: Detectar cuando el jugador se aleja
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            jugadorCerca = false;
            index = 0; 
        }
    }

    IEnumerator AbrirCofreYViajar()
    {
        nivelActivado = true;
        

        PlayerPrefs.SetInt("VengoDelNivelSecreto", 1);
        PlayerPrefs.SetFloat("PosX", GameObject.FindGameObjectWithTag("Player").transform.position.x);
        PlayerPrefs.SetFloat("PosY", GameObject.FindGameObjectWithTag("Player").transform.position.y);
        
        if(chestAnimator != null) chestAnimator.SetTrigger(OpenChestHash);

        yield return new WaitForSeconds(esperaAnimacion);

        SceneManager.LoadScene(nombreNivelSecreto);
    }
}
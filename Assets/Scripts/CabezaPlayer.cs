using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CabezaPlayer : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D rb2D;
    private Vector2 movementInput;
    private Vector3 escalaOriginal;

    public int monedasParaGanar = 234; 
    public string nombreEscenaAnterior = "Level1"; 

    public float tiempoLimite = 60f;
    public TMP_Text textTimer;
    public TMP_Text textCoins;  

    private float tiempoRestante;
    private int coinsActuales = 0;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        tiempoRestante = tiempoLimite;
        escalaOriginal = transform.localScale;

        ActualizarUI();
        
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        movementInput = new Vector2(moveX, moveY).normalized;

        if (moveX != 0)
        {
            float direction = moveX > 0 ? 1 : -1;
            transform.localScale = new Vector3(
                Mathf.Abs(escalaOriginal.x) * direction,
                escalaOriginal.y,
                escalaOriginal.z
            );
        }

        if (tiempoRestante > 0)
        {
            tiempoRestante -= Time.deltaTime;
            
            if(textTimer != null) 
                textTimer.text = tiempoRestante.ToString("0");
        }
        else
        {
            ReiniciarNivel();
        }
    }

    void FixedUpdate()
    {
        rb2D.linearVelocity = movementInput * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            Destroy(collision.gameObject);
            coinsActuales++;
            ActualizarUI();

            if (coinsActuales >= monedasParaGanar)
            {
                GanarNivel();
            }
        }

        if (collision.CompareTag("KillZone") || 
            collision.CompareTag("Spikes") || 
            collision.CompareTag("StarFish") || 
            collision.CompareTag("Bomb") ||
            collision.CompareTag("Urchin")) 
        {
            ReiniciarNivel();
        }
    }

    void ActualizarUI()
    {
        if(textCoins != null)
        {
            textCoins.text = coinsActuales + " / " + monedasParaGanar;
        }
    }


    void ReiniciarNivel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void GanarNivel()
    {
        Debug.Log("¡Nivel Completado!");
        SceneManager.LoadScene(nombreEscenaAnterior);
    }
}
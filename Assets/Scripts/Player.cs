using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{

    public float speed = 5f; // Velocidad del jugador
    private Rigidbody2D rb2D; // Fisicas en Unity

    private float move; // Variable para mover al jugador
    public float jumpForce = 4f; // Fuerza del salto
    private bool isGrounded; // Valor que determina si esta en el suelo

    public Transform groundCheck; 
    public float groundRadius = 0.1f;
    public LayerMask groundLayer;
    private Animator animator;
    private int coins;
    public TMP_Text textCoins;
    private bool isAttacking = false; 
    
    public LifeManager life;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>(); // Poder acceder al componente dentro de Unity
        animator = GetComponent<Animator>(); // Acceder al componente Animator
        GameObject managerObj = GameObject.Find("Lifemanager"); // o el nombre real del objeto
        if (managerObj != null)
        {
            life = managerObj.GetComponent<LifeManager>();
        }
        //Contador de vida
    }

    void Update()
    {
        // 1. Si el jugador está atacando, ignoramos el movimiento y el salto.
        if (isAttacking)
        {
            rb2D.linearVelocity = new Vector2(0, rb2D.linearVelocity.y); // Detiene el movimiento horizontal
            return; // Salimos de Update para ignorar el resto del input
        }

        move = Input.GetAxisRaw("Horizontal"); // Input para mover al jugador
        rb2D.linearVelocity = new Vector2(move * speed, rb2D.linearVelocity.y); // Movimiento del jugador

        if (move != 0)
        {
            transform.localScale = new Vector3(move, 1, 1); // Cambia la direccion del jugador
        }

        if (Input.GetButtonDown("Jump") && isGrounded) // Input para saltar
        {
            rb2D.linearVelocity = new Vector2(rb2D.linearVelocity.x, jumpForce); // Fuerza del salto
        }

        // --- LÓGICA DE ATAQUE ---
        if (Input.GetKeyDown(KeyCode.Q) && isGrounded) // Primer ataque (Tecla Q)
        {
            isAttacking = true;
            animator.SetTrigger("Attack1"); // <-- CAMBIO: Usa el Trigger del Animator
        }
        else if (Input.GetKeyDown(KeyCode.E) && isGrounded) // Segundo ataque (Tecla E)
        {
            isAttacking = true;
            animator.SetTrigger("Attack2"); // <-- CAMBIO: Usa el Trigger del Animator
        }

        animator.SetFloat("Speed", Mathf.Abs(move)); // Controlar la animacion de correr
        animator.SetFloat("VerticalVelocity", rb2D.linearVelocity.y); // Controlar la animacion de salto
        animator.SetBool("isGrounded", isGrounded); // Controlar la animacion de estar en el suelo
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer); // Verifica si el jugador esta en el suelo
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Monedas
        if (collision.transform.CompareTag("Coin"))
        {
            Destroy(collision.gameObject); // Destruye el objeto coleccionable al tocarlo
            coins++; // Incrementa el contador de monedas
            textCoins.text = coins.ToString(); // Actualiza el texto de las monedas
        }

        //Limite de Zona
        if (collision.transform.CompareTag("KillZone"))
        {
            DieAndRestart();
        }

        //Pinchos //Estrellas de mar //Erizos
        if (collision.transform.CompareTag("Spikes") || collision.transform.CompareTag("StarFish") || collision.transform.CompareTag("Urchin")) 
        {
            DieAndRestart();
        }

        //Barril (se mantiene la lógica de knockback inicial)
        if (collision.transform.CompareTag("Barrel"))
        {
            Vector2 knockbackDir = (rb2D.position - (Vector2)collision.transform.position).normalized; // Direccion del knockback
            rb2D.linearVelocity = Vector2.zero; // Reiniciar la velocidad actual
            rb2D.AddForce(knockbackDir * 3f, ForceMode2D.Impulse); // Aplicar la fuerza de knockback

            BoxCollider2D[] colliders = collision.gameObject.GetComponents<BoxCollider2D>();

            foreach (BoxCollider2D collider in colliders)
            {
                collider.enabled = false; // Desactivar el collider del barril para evitar colisiones multiples
            }

            collision.GetComponent<Animator>().enabled = true; // Activar la animacion del barril
            Destroy(collision.gameObject, 0.5f); // Destruir el barril despues de un tiempo
        }

        //Bombas
        if (collision.transform.CompareTag("Bomb"))
        {
            // ... (lógica de knockback y animación de la bomba sin cambios)
            Vector2 knockbackDir = (rb2D.position - (Vector2)collision.transform.position).normalized;
            rb2D.linearVelocity = Vector2.zero;
            rb2D.AddForce(knockbackDir * 3f, ForceMode2D.Impulse);

            BoxCollider2D[] colliders = collision.gameObject.GetComponents<BoxCollider2D>();

            foreach (BoxCollider2D collider in colliders)
            {
                collider.enabled = false;
            }

            Animator bombAnimator = collision.GetComponent<Animator>();
            if (bombAnimator != null) bombAnimator.enabled = true;

            // Variables de la explosión
            Vector2 explosionPos = collision.transform.position;
            float delaySeconds = 1.5f;

            // REDUCIMOS EL RADIO DE DAÑO
            float newExplosionRadius = 0.5f; // Valor reducido de 2.5f a 1.5f (o el que consideres apropiado)

            StartCoroutine(DelayBombExplosionCheck(explosionPos, delaySeconds, newExplosionRadius));

            // Destroy the bomb object after its full lifetime (keep original 2.8s)
            Destroy(collision.gameObject, 2.8f);
        }
    }

    // NUEVO MÉTODO: Llamado por el evento de animación al final de un ataque
    public void AttackEnd()
    {
        isAttacking = false;
    }

    // NUEVO MÉTODO: Manejar la muerte y el reinicio.
    public void DieAndRestart()
    {
        if (life != null) {
            life.TakeDamage(); // ← primero restamos la vida
        } else {
            Debug.LogError("life es null en DieAndRestart");
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reiniciar la escena.
    }

    // MÉTODO MODIFICADO: APLICA DAÑO Y DESTRUYE OBJETOS EN EL ÁREA
    private IEnumerator DelayBombExplosionCheck(Vector2 explosionPos, float delaySeconds, float explosionRadius)
    {
        // Esperar a que aparezca la animación de explosión
        yield return new WaitForSeconds(delaySeconds);

        // --- LÓGICA DE DAÑO EN ÁREA ---

        // 1. Detectar todos los colliders dentro del radio de la explosión
        // Usamos Physics2D.OverlapCircleAll para buscar objetos en el área
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(explosionPos, explosionRadius);

        foreach (Collider2D hit in hitObjects)
        {
            // 2. Revisar si el jugador está dentro del radio
            if (hit.CompareTag("Player"))
            {
                // El jugador muere si está en el radio
                DieAndRestart();
                // Nota: Usamos return para terminar la Coroutine si el jugador muere
                // para evitar procesar más lógica de golpe innecesariamente.
                yield break;
            }
            
            // 3. Destruir Barriles si están en el radio
            if (hit.CompareTag("Barrel"))
            {
                Destroy(hit.gameObject);
                // Opcional: Podrías llamar a una animación de explosión del barril aquí
            }
        }
    }
}
using UnityEngine;

public class Starfish : MonoBehaviour
{
    // CUIDADO: Recuerda corregir el error de sintaxis en Start()
    
    [Header("Movimiento")]
    public float moveSpeed = 4f; // Velocidad de avance
    public float lifeTime = 4f; // Tiempo de vida antes de autodestruirse
    public float destroyOnCollisionDelay = 0.05f; // Pequeño retraso al chocar con pared/suelo

    private Rigidbody2D rb;
    private bool isMoving = false; 
    private float initialGravityScale;

    void Start()
    {
        // CORRECCIÓN: Usar GetComponent<Rigidbody2D>()
        rb = GetComponent<Rigidbody2D>(); 
        
        initialGravityScale = rb.gravityScale;

        // Flota durante la animación "Start"
        rb.gravityScale = 0f;

        // Inicia la animación Start
        GetComponent<Animator>()?.Play("Start");
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            // Aplica la velocidad horizontal, manteniendo la velocidad vertical (gravedad)
            rb.linearVelocity = new Vector2(transform.right.x * moveSpeed, rb.linearVelocity.y);
        }
    }

    /// <summary>
    /// Llamado por el evento de animación en el primer frame de 'Roll'.
    /// </summary>
    public void ActivateMovement()
    {
        isMoving = true;

        // Activa la gravedad (el valor que tenga en el Inspector, ej. 1)
        rb.gravityScale = initialGravityScale;

        // Autodestrucción por tiempo
        Destroy(gameObject, lifeTime);

        // Cambiamos la animación a Roll
        GetComponent<Animator>()?.Play("Roll");
    }

    // LÓGICA DE COLISIÓN SÓLIDA: Activada por el Collider de COLISIÓN (Is Trigger = false)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verifica si el objeto colisionado NO es el suelo (Tilemap con Tag="Floor").
        if (!collision.gameObject.CompareTag("Floor") && !collision.gameObject.CompareTag("StarFish")) 
        {
            // Si choca con CUALQUIER OTRA COSA SÓLIDA (pared, objeto móvil, etc.)
            if (isMoving)
            {
                // Destruir la estrella al chocar con una pared o objeto sólido no-suelo.
                Destroy(gameObject, destroyOnCollisionDelay);
            }
        }
    }
}
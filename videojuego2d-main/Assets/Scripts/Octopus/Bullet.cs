using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5f; // Velocidad de la bala
    public float lifeTime = 2f; // Tiempo de vida en segundos

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Lanza la bala hacia la derecha (dirección x positiva)
        // Usamos la propiedad .right del transform para asegurar que va en la dirección correcta.
        rb.linearVelocity = transform.right * speed; 
        
        // Destruye la bala después de 'lifeTime' segundos
        Destroy(gameObject, lifeTime);
    }

    // NUEVO: Detectar colisión con el Jugador
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verificar si la colisión fue con el Jugador usando su Tag
        if (collision.CompareTag("Player"))
        {
            // Opcional: Llamar un método en el Jugador para gestionar el daño/muerte
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                // Llama al nuevo método que crearemos en Player.cs
                player.DieAndRestart(); 
            }
        }

        // Destruir la bala al impactar con cualquier cosa, no solo el jugador
        // (puedes añadir un chequeo de Tag si solo quieres que se destruya con el Player)
        Destroy(gameObject); 
    }
}
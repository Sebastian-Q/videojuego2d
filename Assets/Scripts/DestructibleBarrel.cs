using UnityEngine;

public class DestructibleBarrel : MonoBehaviour
{
    // Puedes configurar si el barril requiere varios golpes (Health > 1) o solo uno (Health = 1)
    public int health = 1; 

    // Método llamado por el PlayerAttackHitbox al golpear el barril
    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            // Lógica de destrucción del barril (similar a la que tenías en Player.cs)
            // Puedes agregar animaciones o efectos de sonido aquí.
            
            // Destruir el barril y sus efectos (ej. 0.5f para animación de romper)
            Destroy(gameObject, 0.5f);
            
            // Opcional: Desactivar el collider para que no golpee más mientras se destruye
            Collider2D[] colliders = GetComponents<Collider2D>();
            foreach (Collider2D collider in colliders)
            {
                collider.enabled = false;
            }
            
            // Opcional: Activar animación de destrucción (si existe un Animator)
            Animator animator = GetComponent<Animator>();
            if (animator != null) animator.enabled = true;
        }
    }
}
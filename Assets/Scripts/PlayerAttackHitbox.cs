using UnityEngine;

public class PlayerAttackHitbox : MonoBehaviour
{
    // Define el daño que este ataque inflige (puedes ajustarlo en el Inspector)
    public int attackDamage = 1;

    // Se llama cuando el collider de este objeto (el hitbox) toca otro collider de tipo Trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 1. Verificar si el objeto golpeado es un Barril
        if (collision.CompareTag("Barrel"))
        {
            // 2. Intentar obtener un componente para manejar el daño en el barril.
            //    P.ej., un script llamado 'DestructibleBarrel.cs' (ver sección siguiente)
            DestructibleBarrel barrel = collision.GetComponent<DestructibleBarrel>();

            if (barrel != null)
            {
                // 3. Si tiene el componente, llamamos a un método para que reciba daño.
                //    Como los barriles en tu código actual se destruyen de inmediato,
                //    podemos hacer que el método de daño lo destruya.
                barrel.TakeDamage(attackDamage);
            }
            else
            {
                // Si no hay un script de DestructibleBarrel, simplemente lo destruimos si queremos
                // que el ataque siempre destruya el barril al primer toque.
                Destroy(collision.gameObject);
            }
        }

        // Si tuvieras enemigos, la lógica de daño iría aquí también:
        // if (collision.CompareTag("Enemy")) { ... }
    }
}
using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    public Octopus octopus; // Referencia al enemigo
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            octopus.playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        { octopus.playerInRange = false;
        }
    }
}
using UnityEngine;

public class VisionStarFish : MonoBehaviour
{
    public Starfish starfish;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            starfish.playerInRange = true;
            starfish.GetComponent<Animator>().SetBool("playerInRange", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            starfish.playerInRange = false;
            starfish.GetComponent<Animator>().SetBool("playerInRange", false);
        }
    }
}
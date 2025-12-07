using UnityEngine;

public class Urchin : MonoBehaviour
{
    private Animator animator;
    public float timeBetweenAttacks = 3f; // Tiempo de espera entre ataques
    private float nextAttackTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + timeBetweenAttacks;
            animator.SetTrigger("StandOut");
        }
    }
}

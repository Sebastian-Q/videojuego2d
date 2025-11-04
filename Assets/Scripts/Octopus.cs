using UnityEngine;

public class Octopus : MonoBehaviour
{
    public GameObject bulletPrefab; // Arrastra el prefab de la bala aquí
    public Transform firePoint;     // Objeto vacío que indica el punto de salida de la bala
    public float timeBetweenAttacks = 3f; // Tiempo de espera entre ataques

    private Animator animator;
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
            // Activa la animación de ataque
            animator.SetTrigger("Attack");
            nextAttackTime = Time.time + timeBetweenAttacks;
        }
    }

    // Método llamado por la animación de ataque en Unity
    public void Shoot()
    {
        // Obtiene la dirección actual del pulpo (1 para derecha, -1 para izquierda)
        float direction = Mathf.Sign(transform.localScale.x);

        // Calcula la rotación para la bala (0 grados = derecha, 180 grados = izquierda)
        Quaternion bulletRotation = firePoint.rotation;
        if (direction < 0) // Si está mirando a la izquierda
        {
            // Rotar la bala 180 grados en el eje Z
            bulletRotation = Quaternion.Euler(0, 0, 180); 
        }

        // Instanciar la bala con la rotación correcta
        Instantiate(bulletPrefab, firePoint.position, bulletRotation);
    }
}

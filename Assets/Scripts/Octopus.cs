using System;
using System.Collections.Generic;
using UnityEngine;

public class Octopus : MonoBehaviour
{
    public GameObject bulletPrefab; // Objeto del prefab de la bala
    public Transform firePoint;     // Objeto vacío que indica el punto de salida de la bala
    public float timeBetweenAttacks = 1f; // Tiempo de espera entre ataques

    private Animator animator;
    private float nextAttackTime;
    
    public bool playerInRange = false; // Valor que determina si entro en el rango del enemigo

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange && Time.time >= nextAttackTime)
        {
            animator.SetTrigger("Attack"); // Activa la animación de ataque
            nextAttackTime = Time.time + timeBetweenAttacks;
        }
    }

    // Método llamado por la animación de ataque en Unity
    public void Shoot()
    {
        // Obtiene la dirección actual del pulpo (1 para derecha, -1 para izquierda)
        float direction = Mathf.Sign(transform.localScale.x);

        // Saber la dirección del disparo para la bala (0 grados = derecha, 180 grados = izquierda)
        Quaternion bulletDirection = firePoint.rotation;
        if (direction < 0) // Si está mirando a la izquierda
        {
            // Rotar la bala 180 grados en el eje Z
            bulletDirection = Quaternion.Euler(0, 0, 180); 
        }

        // Instanciar la bala con la rotación correcta
        Instantiate(bulletPrefab, firePoint.position, bulletDirection);
    }
}

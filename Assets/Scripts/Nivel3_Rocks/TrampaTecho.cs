using Unity.VisualScripting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampaTecho : MonoBehaviour
{
    public Rigidbody2D rb2D;
    public float distanciaLinea;
    public LayerMask capaJugador;
    public bool estaSubiendo = false;
    public float velocidadSubida;
    public float tiempoEspera;

    private void Update()
    {
        RaycastHit2D infoJugador = Physics2D.Raycast(transform.position, Vector3.down, distanciaLinea, capaJugador);

        if (infoJugador && !estaSubiendo)
        {
            rb2D.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }
        
       if (estaSubiendo)
       {
            transform.Translate(Time.deltaTime * velocidadSubida * Vector3.up);
       }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Suelo"))
        {
            rb2D.constraints = RigidbodyConstraints2D.FreezeAll;

            if (estaSubiendo)
            {
                estaSubiendo = false;
            }
            else
            {
                StartCoroutine(EsperarEnSuelo());
            }
            
        }
        
        if(other.gameObject.TryGetComponent(out Player player))
        {
            player.DieAndRestart();
        }
    }

    private IEnumerator EsperarEnSuelo()
    {
        yield return new WaitForSeconds(tiempoEspera);
        estaSubiendo = true;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * distanciaLinea);
    }

}

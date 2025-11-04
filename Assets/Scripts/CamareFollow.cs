using UnityEngine;

public class CamareFollow : MonoBehaviour
{

    public Transform target; // El objetivo a seguir (el jugador)

    private void LateUpdate()
    {
        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z); // Seguir al jugador manteniendo la posicion Z de la camara
    }
}

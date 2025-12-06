using UnityEngine;

public class CamareFollow : MonoBehaviour
{
    public Vector2 posMin, posMax;

    public Transform target; // El objetivo a seguir (el jugador)

    private void LateUpdate()
    {
        transform.position = new Vector3(Mathf.Clamp(target.position.x,posMin.x,posMax.x), Mathf.Clamp(target.position.y,posMin.y,posMax.y), transform.position.z); // Seguir al jugador manteniendo la posicion Z de la camara
    }
}

using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform playerTransform; // Referencia al transform del jugador

    // Update is called once per frame
    void Update()
    {
        if(playerTransform.position.y > transform.position.y)
        {
            // Si la posicion del jugador es mayor que la posicion de la camara, se actualiza la posicion de la camara
            // para que siga al jugador en el eje Y
            transform.position = new Vector3(transform.position.x, playerTransform.position.y, transform.position.z);
        }
    }
}

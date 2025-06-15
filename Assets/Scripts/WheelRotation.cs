using UnityEngine;

public class WheelRotation : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 100f; // Velocidad de rotacion de la rueda

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0,0,rotationSpeed * Time.deltaTime));
    }
}

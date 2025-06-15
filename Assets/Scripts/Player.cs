using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] private float verticalForce = 400f; // Fuerza vertical que se aplicar� al jugador
    [SerializeField] private float restartDelay = 1f; // Tiempo de espera antes de reiniciar la escena
    [SerializeField] private ParticleSystem playerParticles; // Particulas que se reproducir�n al reiniciar la escena
    [SerializeField] private Color orangeColor;
    [SerializeField] private Color violetColor;
    [SerializeField] private Color cyanColor;
    [SerializeField] private Color pinkColor;
    private string currentColor;
    Rigidbody2D playerRb;
    SpriteRenderer playerSR;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Se agrega una fuerza al objeto con la funcion AddForce que requiere un parametro
        // Vector2 que indica la direccion y magnitud de la fuerza a aplicar.
        playerRb = GetComponent<Rigidbody2D>();

        //Se cambia la propiedad color del SpriteRenderer
        playerSR = GetComponent<SpriteRenderer>();

        ChangeColor();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            playerRb.linearVelocity = Vector2.zero; // Resetea la velocidad del jugador antes de aplicar la nueva fuerza
            // Al presionar la tecla espacio, se aplica una fuerza vertical al jugador
            playerRb.AddForce(new Vector2(0, verticalForce));
        }
    }
    /*
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Colision con: " + collision.gameObject.name);
        collision.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
    }
    */

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("ColorChanger"))
        {
            ChangeColor(); // Cambia el color del jugador al colisionar con un objeto de tipo ColorChanger
            Destroy(collision.gameObject); // Destruye el objeto ColorChanger al colisionar
            return;
        }

        if (collision.gameObject.CompareTag("FinishLine"))
        {
            gameObject.SetActive(false); // Desactiva el jugador al colisionar con un objeto de tipo FinishLine
            Instantiate(playerParticles, transform.position, Quaternion.identity); // Instancia las particulas en la posicion del jugador
            Invoke("LoadNextScene", restartDelay); // Carga la siguiente escena al colisionar con un objeto de tipo FinishLine
            return;
        }

        if (!collision.gameObject.CompareTag(currentColor))
        {
            gameObject.SetActive(false); // Desactiva el jugador al colisionar con un objeto de color diferente
            Instantiate(playerParticles, transform.position, Quaternion.identity); // Instancia las particulas en la posicion del jugador
            Invoke("RestartScene", restartDelay);

        }
    }

    void LoadNextScene()
    {
        int activeSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(activeSceneIndex + 1);
    }


     void RestartScene()
    {
        int activeSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(activeSceneIndex);
    }

    void ChangeColor()
    {
        int randomNumber = Random.Range(0, 4);
        switch (randomNumber)
        {
            case 0:
                playerSR.color = orangeColor;
                currentColor = "Orange";
                break;
            case 1:
                playerSR.color = violetColor;
                currentColor = "Violet";
                break;
            case 2:
                playerSR.color = cyanColor;
                currentColor = "Cyan";
                break;
            case 3:
                playerSR.color = pinkColor;
                currentColor = "Pink";
                break;
        }
    }

}

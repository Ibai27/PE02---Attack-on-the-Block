using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
    // Velocidad de movimiento del jugador
    public float velocidad = 5f;
    private Vector3 posicionInicial;

    void Start()
    {
        // Ocultar el cursor
        Cursor.visible = false;

        // Bloquear el cursor dentro de la ventana del juego
        Cursor.lockState = CursorLockMode.Confined;

        // Guardar posición inicial
        posicionInicial = transform.position;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    void Update()
    {
        // Obtener la posición del ratón en coordenadas del mundo
        Vector3 posicionRaton = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        posicionRaton.z = 0f; // Mantener en el plano 2D

        // Limitar la posición del ratón al área visible de la cámara
        float alto = Camera.main.orthographicSize;
        float ancho = alto * Camera.main.aspect;

        // Ajustar la posición X e Y dentro de los límites
        posicionRaton.x = Mathf.Clamp(posicionRaton.x, -ancho, ancho);
        posicionRaton.y = Mathf.Clamp(posicionRaton.y, -alto, alto);

        // Mover al jugador hacia la posición del ratón instantaneo
        //transform.position = posicionRaton;

        transform.position = Vector3.MoveTowards(transform.position, posicionRaton, velocidad * Time.deltaTime);

    }

    // Método público para restaurar posición
   public void ResetearPosicion()
    {
        // Restaurar posición inicial y activar jugador
        transform.position = posicionInicial;
        gameObject.SetActive(true);
        enabled = true;

        // Ocultar cursor al reiniciar
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    // Detectar colisión con un enemigo
    private void OnCollisionEnter2D(Collision2D colision)
    {
        if (colision.gameObject.CompareTag("Enemigo"))
        {
            Debug.Log("Colisión con enemigo -> GAME OVER");

            // Llamar al GameManager para activar el panel
            GameManager gm = Object.FindFirstObjectByType<GameManager>();
            if (gm != null)
                gm.FinDelJuego();
        }

    }
}

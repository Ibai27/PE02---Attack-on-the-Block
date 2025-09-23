using UnityEngine;

public class MovimientoEnemigo : MonoBehaviour
{
    // Velocidad inicial del enemigo
    public float velocidad = 5f;


    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Darle una dirección inicial aleatoria
        Vector2 direccion = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;

        // Usar la instancia "rb" y la propiedad "velocity"
        rb.linearVelocity = direccion * velocidad;
    }


    // El rebote se maneja automáticamente gracias al PhysicsMaterial2D
}

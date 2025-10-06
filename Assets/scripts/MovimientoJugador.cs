using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 5f;
    private Vector3 posicionInicial;

    [Header("Invencibilidad")]
    public bool invencible = false;
    public float tiempoInvencibilidadPowerUp = 5f;
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        // Guardar posición inicial
        posicionInicial = transform.position;

        // Ocultar cursor al iniciar la partida
        OcultarCursor();
    }

    void Update()
    {
        // Mover jugador hacia el ratón
        Vector3 posicionRaton = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        posicionRaton.z = 0f;

        float alto = Camera.main.orthographicSize;
        float ancho = alto * Camera.main.aspect;

        posicionRaton.x = Mathf.Clamp(posicionRaton.x, -ancho, ancho);
        posicionRaton.y = Mathf.Clamp(posicionRaton.y, -alto, alto);

        transform.position = Vector3.MoveTowards(transform.position, posicionRaton, velocidad * Time.deltaTime);
    }

    #region Cursor
    private void OcultarCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined; // o Locked si quieres bloquear completamente
    }

    public void MostrarCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    #endregion

    #region Invencibilidad
    public void ActivarInvencibilidad(float duracion)
    {
        if (!invencible)
            StartCoroutine(InvencibilidadTemporal(duracion));
    }

    private System.Collections.IEnumerator InvencibilidadTemporal(float duracion)
    {
        invencible = true;

        Color colorOriginal = sr.color;
        float tiempoParpadeo = 0.2f;
        float tiempoAcumulado = 0f;

        while (tiempoAcumulado < duracion)
        {
            sr.color = Color.yellow;
            yield return new WaitForSeconds(tiempoParpadeo);
            sr.color = colorOriginal;
            yield return new WaitForSeconds(tiempoParpadeo);

            tiempoAcumulado += tiempoParpadeo * 2;
        }

        invencible = false;
        sr.color = colorOriginal;
    }
    #endregion

    #region Resetear
    public void ResetearPosicion()
    {
        transform.position = posicionInicial;
        gameObject.SetActive(true);
        enabled = true;

        // Ocultar cursor al reiniciar
        OcultarCursor();
    }
    #endregion

    #region Colisiones
    private void OnCollisionEnter2D(Collision2D colision)
    {
        if (colision.gameObject.CompareTag("Enemigo") && !invencible)
        {
            // Reproducir sonido
            AudioManager.instancia.ReproducirSonido(AudioManager.instancia.sonidoGolpeJugador);

            // Quitar vida a través del GameManager
            GameManager gm = Object.FindFirstObjectByType<GameManager>();
            if (gm != null)
                gm.QuitarVida();
        }
    }
    #endregion
}

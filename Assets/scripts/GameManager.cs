using UnityEngine;
using System.Collections; // <<---- NECESARIO para IEnumerator
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject panelGameOver;          // Panel de Game Over
    public GameObject jugador;                // Referencia al jugador principal
    public MovimientoEnemigo[] enemigos;      // Todos los enemigos iniciales
    public MovimientoJugador Pjugador;        // Asignar en el Inspector
    public Cronometro cronometro;             // Referencia al cronómetro

    [Header("Sistema de Vidas")]
    public int vidasInicial = 3;      // Vidas al inicio del juego
    public float tiempoInvencibilidad = 1.5f; // Tiempo de invulnerabilidad después de recibir daño

    [HideInInspector]
    public int vidas;                 // Vidas actuales
    private bool jugadorInvencible = false;


    private bool juegoTerminado = false;

    void Start()
    {
        vidas = vidasInicial;
        // Reproducir sonido de inicio
        if (AudioManager.instancia != null)
            AudioManager.instancia.ReproducirSonido(AudioManager.instancia.sonidoInicio);
    }

    public void FinDelJuego()
    {
        if (!juegoTerminado)
        {
            // Parar cronómetro
            cronometro.PararCronometro();

            // Guardar récord aquí
            cronometro.GuardarRecord();

            // Reproducir sonido Game Over
            if (AudioManager.instancia != null)
                AudioManager.instancia.ReproducirSonido(AudioManager.instancia.sonidoGameOver);

            juegoTerminado = true;

            // Mostrar panel
            if (panelGameOver != null) panelGameOver.SetActive(true);

            // Congelar el tiempo
            Time.timeScale = 0f;

            // Liberar cursor
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            // Ocultar jugador
            if (jugador != null) jugador.SetActive(false);

            foreach (var e in enemigos)
                e.enabled = false;
        }
    }

    // Botón Reiniciar
    public void ReiniciarJuego()
    {

        // Resetear vidas
        vidas = vidasInicial;
        jugadorInvencible = false;

        Debug.Log("Reiniciando juego...");

        // Guardar récord antes de reiniciar
        cronometro.GuardarRecord();

        // Restaurar el tiempo
        Time.timeScale = 1f;

        // Reiniciar jugador
        if (Pjugador != null)
            Pjugador.ResetearPosicion();

        // Reactivar enemigos
        foreach (var e in enemigos)
            e.enabled = true;

        // Reiniciar cronómetro
        cronometro.ReiniciarCronometro();

        // Ocultar panel de Game Over
        if (panelGameOver != null)
            panelGameOver.SetActive(false);

        // Reproducir sonido de inicio nuevamente
        if (AudioManager.instancia != null)
            AudioManager.instancia.ReproducirSonido(AudioManager.instancia.sonidoInicio);

        juegoTerminado = false;
    }

    // Botón Salir
    public void SalirJuego()
    {
        Debug.Log("Salir del juego...");

        // Guardar récord al salir
        cronometro.GuardarRecord();

        // Reproducir sonido de Game Over antes de cerrar
        if (AudioManager.instancia != null)
            AudioManager.instancia.ReproducirSonido(AudioManager.instancia.sonidoGameOver);

        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    private IEnumerator InvencibilidadTemporal()
    {
        jugadorInvencible = true;

        // Opcional: feedback visual (parpadeo)
        SpriteRenderer sr = jugador.GetComponent<SpriteRenderer>();
        Color original = sr.color;

        float tiempoParpadeo = 0.1f;
        float tiempoAcumulado = 0f;
        while (tiempoAcumulado < tiempoInvencibilidad)
        {
            sr.color = Color.yellow; // visible
            yield return new WaitForSeconds(tiempoParpadeo);
            sr.color = original;     // original
            yield return new WaitForSeconds(tiempoParpadeo);

            tiempoAcumulado += tiempoParpadeo * 2;
        }

        jugadorInvencible = false;
    }

    public void QuitarVida()
    {
        if (!jugadorInvencible)
        {
            vidas--;
            Debug.Log("Vida perdida. Vidas restantes: " + vidas);

            // Activar invencibilidad temporal
            StartCoroutine(InvencibilidadTemporal());

            // Comprobar si se acabaron las vidas
            if (vidas <= 0)
            {
                FinDelJuego();
            }
        }
    }

}

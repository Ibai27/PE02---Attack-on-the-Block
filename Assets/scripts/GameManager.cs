using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject panelGameOver;    // Panel de Game Over
    public GameObject jugador;          // Referencia al jugador principal
    public MovimientoEnemigo[] enemigos; // Todos los enemigos iniciales
    public MovimientoJugador Pjugador; // Asignar en el Inspector
    public Cronometro cronometro;          // Referencia al cronómetro


    private bool juegoTerminado = false;

    public void FinDelJuego()
    {
        if (!juegoTerminado)
        {
            // Parar cronómetro
            cronometro.PararCronometro();

            // Guardar récord aquí
            cronometro.GuardarRecord();

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

            jugador.gameObject.SetActive(false);
            foreach(var e in enemigos)
                e.enabled = false;

        }
    }

    // Botón Reiniciar
    public void ReiniciarJuego()
    {
        Debug.Log("Reiniciando juego...");
        // Guardar récord también antes de reiniciar
        cronometro.GuardarRecord();

        // Restaurar el tiempo antes de recargar
        Time.timeScale = 1f;
    

        // Cargar el panel
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        // Restaurar jugador
        if (jugador != null)
            Pjugador.ResetearPosicion();

        // Reiniciar cronómetro
        cronometro.ReiniciarCronometro();


        juegoTerminado = false;
    }

    // Botón Salir
    public void SalirJuego()
    {
        
        Debug.Log("Salir del juego...");
        
        // Guardar récord también al salir
        cronometro.GuardarRecord();
        Application.Quit();

    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #endif
    }
}

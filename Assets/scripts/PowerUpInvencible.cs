using UnityEngine;

public class PowerUpInvencible : MonoBehaviour
{
    public string nombrePowerUp = "Invencibilidad";
    public float duracion = 5f; // Duración del efecto

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entra");
            MovimientoJugador jugador = other.GetComponent<MovimientoJugador>();
            if (jugador != null)
            {
                jugador.ActivarInvencibilidad(duracion);
            }

            // Avisar al Spawner para mostrar mensaje
            SpawnerPowerUp spawner = FindObjectOfType<SpawnerPowerUp>();
            if (spawner != null)
                spawner.PowerUpRecogido(nombrePowerUp);

            // Reproducir sonido de power-up
            if (AudioManager.instancia != null)
                AudioManager.instancia.ReproducirSonido(AudioManager.instancia.sonidoPowerUp);

            Destroy(gameObject);
        }
    }
}

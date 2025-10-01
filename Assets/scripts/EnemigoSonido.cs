using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemigoSonido : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D col)
    {
        // Rebote contra pared
        if (col.gameObject.CompareTag("Pared"))
        {
            AudioManager.instancia.ReproducirSonido(AudioManager.instancia.sonidoRebotePared);
        }

        // Rebote contra otro enemigo
        if (col.gameObject.CompareTag("Enemigo"))
        {
            AudioManager.instancia.ReproducirSonido(AudioManager.instancia.sonidoReboteEnemigo);
        }

        // Golpe al jugador
        if (col.gameObject.CompareTag("Player"))
        {
            AudioManager.instancia.ReproducirSonido(AudioManager.instancia.sonidoGolpeJugador);

        }
    }
}


using UnityEngine;
using TMPro; // 👈 Importar TextMeshPro

public class SpawnerPowerUp : MonoBehaviour
{
    public GameObject[] powerUps; // Array de prefabs de power-ups
    public float tiempoEntreSpawn = 10f;
    public float margen = 0.5f;
    public TextMeshProUGUI mensajePowerUp; 

    public float duracionMensaje = 2f;

    private float timer;
    private GameObject powerUpActual;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= tiempoEntreSpawn && powerUpActual == null)
        {
            SpawnPowerUp();
            timer = 0f;
        }
    }

    private void SpawnPowerUp()
    {
        if (powerUps.Length == 0) return;

        int indice = Random.Range(0, powerUps.Length);
        GameObject prefab = powerUps[indice];

        float alto = Camera.main.orthographicSize - margen;
        float ancho = alto * Camera.main.aspect - margen;

        Vector3 posicion = new Vector3(
            Random.Range(-ancho, ancho),
            Random.Range(-alto, alto),
            0f
        );

        powerUpActual = Instantiate(prefab, posicion, Quaternion.identity);

        // Forzar tamaño uniforme (ajusta 0.5f al tamaño deseado)
        powerUpActual.transform.localScale = Vector3.one * 0.5f;
        if (mensajePowerUp != null)
        {
            mensajePowerUp.text = "¡Ha aparecido un power-up: <color=yellow>" + prefab.name + "</color>!";
        }
    }

    public void PowerUpRecogido(string nombre)
    {
        if (mensajePowerUp != null)
        {
            mensajePowerUp.text = "Has conseguido: <color=green>" + nombre + "</color>!";
            CancelInvoke("BorrarMensaje");
            Invoke("BorrarMensaje", duracionMensaje); // Borrar mensaje tras 2 segundos
        }

        powerUpActual = null;
    }

    private void BorrarMensaje()
    {
        if (mensajePowerUp != null)
            mensajePowerUp.text = "";
    }

}

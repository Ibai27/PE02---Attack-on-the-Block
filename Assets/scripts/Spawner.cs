using UnityEngine;

public class SpawnerEnemigos : MonoBehaviour
{
    // Prefab del enemigo que se va a instanciar
    public GameObject prefabEnemigo;

    // Tiempo entre spawns en segundos
    public float tiempoEntreSpawns = 30f;

    // Límite de enemigos en la escena (opcional)
    public int maxEnemigos = 10;

    // Área de spawn (un rectángulo alrededor de la cámara)
    private float ancho;
    private float alto;

    // Guardar la posición inicial del prefab
    private Vector3 posicionInicialPrefab;

    void Start()
    {
        // Calcular los límites según la cámara ortográfica
        Camera camara = Camera.main;
        alto = camara.orthographicSize;
        ancho = alto * camara.aspect;

        // Guardar posición inicial del prefab
        if (prefabEnemigo != null)
            posicionInicialPrefab = prefabEnemigo.transform.position;

        // Inicia la corutina para spawnear enemigos
        StartCoroutine(SpawnEnemigos());
    }

   private System.Collections.IEnumerator SpawnEnemigos()
    {
        while (true)
        {
            // Espera el tiempo definido
            yield return new WaitForSeconds(tiempoEntreSpawns);

            // Contar enemigos actuales
            int enemigosActuales = GameObject.FindGameObjectsWithTag("Enemigo").Length;

            if (enemigosActuales < maxEnemigos)
            {
                // Genera posición aleatoria dentro del área visible
                Vector3 posicionSpawn = new Vector3(
                    Random.Range(-ancho + 0.5f, ancho - 0.5f),
                    Random.Range(-alto + 0.5f, alto - 0.5f),
                    0f
                );

                // Instanciar el enemigo
                Instantiate(prefabEnemigo, posicionSpawn, Quaternion.identity);
                Debug.Log("Spawn de un nuevo enemigo!");
            }
        }
    }


       public void InstanciarEnemigoInicial()
    {
        Instantiate(prefabEnemigo, posicionInicialPrefab, Quaternion.identity);
    }


    // Método público para obtener la posición inicial del prefab
    public Vector3 ObtenerPosicionInicial()
    {
        return posicionInicialPrefab;
    }
}

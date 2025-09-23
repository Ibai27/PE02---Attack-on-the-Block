using UnityEngine;
using TMPro;

public class Cronometro : MonoBehaviour
{
    [Header("Referencias UI")]
    public TextMeshProUGUI textoCronometro; // Cronómetro actual
    public TextMeshProUGUI textoRecord;     // Texto para mostrar récord

    private float tiempo;
    private bool enMarcha = true;
    private float record;

    void Start()
    {
        // Cargar récord guardado al iniciar
        record = PlayerPrefs.GetFloat("RecordTiempo", 0f);
        ActualizarTextoRecord();
    }

    void FixedUpdate()
    {
        if (enMarcha)
        {
            tiempo += Time.fixedDeltaTime;

            int minutos = Mathf.FloorToInt(tiempo / 60);
            int segundos = Mathf.FloorToInt(tiempo % 60);

            textoCronometro.text = string.Format("{0:00}:{1:00}", minutos, segundos);
        }
    }

    public void PararCronometro()
    {
        enMarcha = false;
    }

    public void ReiniciarCronometro()
    {
        tiempo = 0f;
        enMarcha = true;
    }

    public void GuardarRecord()
    {
        if (tiempo > record)
        {
            record = tiempo;
            PlayerPrefs.SetFloat("RecordTiempo", record);
            PlayerPrefs.Save();
            ActualizarTextoRecord();
        }
    }

    private void ActualizarTextoRecord()
    {
        int minutos = Mathf.FloorToInt(record / 60);
        int segundos = Mathf.FloorToInt(record % 60);
        textoRecord.text = "Récord: " + string.Format("{0:00}:{1:00}", minutos, segundos);
    }
}

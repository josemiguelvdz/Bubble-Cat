using UnityEngine;
using TMPro;

public class QualityLogic : MonoBehaviour
{
    [Tooltip("Dropdown"), SerializeField]
    private TMP_Dropdown dropdown;
    [Tooltip("Calidad"), SerializeField]
    private int quality;
    void Start()
    {
        quality = PlayerPrefs.GetInt("qualityNumber", 2); // La calidad se ajusta al empezar la partida dependiendo de las preferencias
        // anteriormente ajustadas por el jugador, si es que lo ha hecho.
        dropdown.value = quality;
        AdjustQuality();
    }

    public void AdjustQuality() // Cambia la calidad
    {
        QualitySettings.SetQualityLevel(dropdown.value);
        PlayerPrefs.SetInt("qualityNumber", dropdown.value);
        quality = dropdown.value;
    }

}

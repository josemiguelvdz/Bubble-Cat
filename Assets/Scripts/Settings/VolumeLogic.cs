using UnityEngine;
using UnityEngine.UI;

public class VolumeLogic : MonoBehaviour
{
    [Tooltip("Slider"), SerializeField]
    private Slider slider;
    [Tooltip("Valor del slider"), SerializeField]
    private float sliderValue;
    [Tooltip("Imagen de mute"), SerializeField]
    private Image imageMute;
    // Start is called before the first frame update
    void Start()
    {
        slider.value = PlayerPrefs.GetFloat("volumenAudio", 0.5f); // El volumen se ajusta al empezar la partida dependiendo de las preferencias
        // anteriormente ajustadas por el jugador, si es que lo ha hecho.
        AudioListener.volume = slider.value;
        CheckIfMute();
    }
    public void ChangeSlider(float value) // Se encarga de cambiar el valor en el slider
    {
        sliderValue = value;
        PlayerPrefs.SetFloat("volumenAudio", sliderValue);
        AudioListener.volume = slider.value;
        CheckIfMute();
    }

    public void CheckIfMute() // Si el volumen es 0, aparece un icono de mute
    {
        if(sliderValue == 0)
        {
            imageMute.enabled = true;
        }
        else
        {
            imageMute.enabled = false;
        }
    }
}

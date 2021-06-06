using UnityEngine;
using UnityEngine.UI;

public class BrigthnessLogic : MonoBehaviour
{
    [Tooltip("Slider"), SerializeField]
    private Slider slider;
    [Tooltip("Valor del slider"), SerializeField]
    private float sliderValue;
    [Tooltip("Slider"), SerializeField]
    private Image brightnessPanel;
    // Start is called before the first frame update
    void Start()
    {
        slider.value = PlayerPrefs.GetFloat("brillo", 0.5f); // El brillo se ajusta al empezar la partida dependiendo de las preferencias
        // anteriormente ajustadas por el jugador, si es que lo ha hecho.

        brightnessPanel.color = new Color(brightnessPanel.color.r, brightnessPanel.color.g, brightnessPanel.color.b, slider.value);
    }

    public void ChangeSlider(float value) // Cambia la opacidad para simular el brillo
    {
        sliderValue = value;
        PlayerPrefs.SetFloat("brillo", sliderValue);
        brightnessPanel.color = new Color(brightnessPanel.color.r, brightnessPanel.color.g, brightnessPanel.color.b, slider.value);
    }
}

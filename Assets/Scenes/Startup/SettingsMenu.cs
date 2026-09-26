using UnityEngine;
using UnityEngine.UI;

public sealed class SettingsMenu : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Slider volumeSlider;

    private void Awake()
    {
        if (volumeSlider != null)
        {
            volumeSlider.minValue = 0f;
            volumeSlider.maxValue = 1f;
            volumeSlider.value = AudioListener.volume;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }

        if (panel != null) panel.SetActive(false);
    }

    private void OnDestroy()
    {
        if (volumeSlider != null) volumeSlider.onValueChanged.RemoveListener(SetVolume);
    }

    public void Toggle()
    {
        if (panel != null) panel.SetActive(!panel.activeSelf);
    }

    private void SetVolume(float value)
    {
        AudioListener.volume = value;
    }
}
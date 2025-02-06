using UnityEngine;
using UnityEngine.UI;

public class SettingPanelController : MonoBehaviour
{
    [SerializeField] private Slider soundSlider = null;
    [SerializeField] private Slider musicSlider = null;
    [SerializeField] private Slider brightnessSlider = null;


    /// <summary>
    /// Handle on show this panel.
    /// </summary>
    public void OnShow()
    {
        soundSlider.value = PlayerDataController.SoundVolume;
        musicSlider.value = PlayerDataController.MusicVolume;
        brightnessSlider.value = PlayerDataController.Brightness;
    }



    public void OnSoundSliderChanged()
    {
        PlayerDataController.UpdateSoundVolume(soundSlider.value);
        SoundManager.Instance.UpdateVolumes();
    }

    public void OnMusicSliderChanged()
    {
        PlayerDataController.UpdateMusicVolume(musicSlider.value);
        SoundManager.Instance.UpdateVolumes();
    }


    public void OnBrightnessSliderChanged()
    {
        PlayerDataController.UpdateBrightness(brightnessSlider.value);
        Screen.brightness = PlayerDataController.Brightness;
    }


    public void OnClickCloseButton()
    {
        SoundManager.Instance.PlayButtonSound();
        gameObject.SetActive(false);
    }
}

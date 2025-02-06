using UnityEngine;

public class HomeManager : MonoBehaviour
{
    [SerializeField] private AudioClip backgroundMusic = null;
    private void Awake()
    {
        PlayerDataController.InitPlayerData();
    }
    private void Start()
    {
        SoundManager.Instance.UpdateVolumes();
        ViewManager.Instance.SetActiveView(ViewType.HomeView);
        SoundManager.Instance.PlayMusic(backgroundMusic);
        Screen.brightness = PlayerDataController.Brightness;
    }
}

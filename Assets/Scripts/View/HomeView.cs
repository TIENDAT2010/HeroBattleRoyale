using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeView : BaseView
{
    [SerializeField] private Text levelText = null;
    [SerializeField] private Text cashText = null;
    [SerializeField] private SettingPanelController settingPanel = null;

    public override void OnShow()
    {
        settingPanel.gameObject.SetActive(false);
        levelText.text = "Level: " + PlayerDataController.CurrentLevel.ToString();
        cashText.text = PlayerDataController.CurrentCash.ToString();
    }

    public override void OnHide()
    {
        gameObject.SetActive(false);
    }


    public void OnClickHeroesButton()
    {
        SoundManager.Instance.PlayButtonSound();
        ViewManager.Instance.LoadScene("Heroes");
    }


    public void OnClickStartButton()
    {
        SoundManager.Instance.PlayButtonSound();
        LevelGameConfigSO levelGameConfigSO = Resources.Load<LevelGameConfigSO>("LevelConfig/" + PlayerDataController.CurrentLevel);
        ViewManager.Instance.LoadScene(levelGameConfigSO.mapID);
    }


    public void OnClickSettingButton()
    {
        SoundManager.Instance.PlayButtonSound();
        settingPanel.gameObject.SetActive(true);
        settingPanel.OnShow();
    }
}

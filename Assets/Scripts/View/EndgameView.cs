
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndgameView : BaseView
{
    [SerializeField] private GameObject victoryPanelTop = null;
    [SerializeField] private GameObject defeatedPanelTop = null;
    [SerializeField] private GameObject resultPanel = null;
    [SerializeField] private GameObject sunbrustPanel = null;
    [SerializeField] private GameObject victoryPanelResult = null;
    [SerializeField] private GameObject defeatedPanelResult = null;
    [SerializeField] private CanvasGroup resultPanelCanvas = null;
    [SerializeField] private Text cashText = null;
    [SerializeField] private GameObject bottomsButton = null;
    [SerializeField] private GameObject closeButton = null;


    public override void OnShow()
    {
        if(IngameManager.Instance.GameState == GameState.LevelCompleted)
        {
            victoryPanelTop.SetActive(false);
            defeatedPanelTop.SetActive(false);
            resultPanel.SetActive(true);
            victoryPanelResult.SetActive(true);
            defeatedPanelResult.SetActive(false);
            bottomsButton.SetActive(false);

        }    
        else if(IngameManager.Instance.GameState == GameState.LevelFailed)
        {
            victoryPanelTop.SetActive(false);
            defeatedPanelTop.SetActive(false);
            resultPanel.SetActive(true);
            victoryPanelResult.SetActive(false);
            defeatedPanelResult.SetActive(true);
            bottomsButton.SetActive(false);
        }

        StartCoroutine(CRShowResultPanel());
    }


    private void Update()
    {
        if (sunbrustPanel.gameObject.activeSelf)
        {
            sunbrustPanel.transform.localEulerAngles += Vector3.forward * 150f * Time.deltaTime;
        }
    }

    public override void OnHide()
    {
        gameObject.SetActive(false);
    }



    /// <summary>
    /// Coroutine show the result panel
    /// </summary>
    /// <returns></returns>
    private IEnumerator CRShowResultPanel()
    {
        resultPanelCanvas.alpha = 0f;
        yield return new WaitForSeconds(5f);
        cashText.text = "0";
        sunbrustPanel.SetActive(false);
        closeButton.SetActive(false);
        float t = 0;
        float moveTime = 2f;
        while (t < moveTime)
        {
            t += Time.deltaTime;
            float factor = t / moveTime;
            float alphaCanvas = Mathf.Lerp(0, 1, factor);
            resultPanelCanvas.alpha = alphaCanvas;
            yield return null;
        }

        sunbrustPanel.SetActive(true);

        t = 0;
        int cashReward = IngameManager.Instance.GameState == GameState.LevelFailed ? 30 : 100;
        while (t < cashReward)
        {
            t += 1;
            cashText.text = "+" + t.ToString();
            SoundManager.Instance.PlaySound(SoundManager.Instance.addCoin);
            yield return new WaitForSeconds(0.05f);
        }

        PlayerDataController.UpdateCash(cashReward);
        closeButton.SetActive(true);
    }



    public void OnClickCloseButton()
    {
        if (IngameManager.Instance.GameState == GameState.LevelCompleted)
        {
            resultPanel.SetActive(false);
            victoryPanelTop.SetActive(true);
            defeatedPanelTop.SetActive(false);
            bottomsButton.SetActive(true);
        }
        else if (IngameManager.Instance.GameState == GameState.LevelFailed)
        {
            resultPanel.SetActive(false);
            victoryPanelTop.SetActive(false);
            defeatedPanelTop.SetActive(true);
            bottomsButton.SetActive(true);
        }
    }    


    public void OnClickHomeButton()
    {
        SoundManager.Instance.PlayButtonSound();
        ViewManager.Instance.LoadScene("Home");
    }    

    public void OnClickHeroesButton()
    {
        SoundManager.Instance.PlayButtonSound();
        ViewManager.Instance.LoadScene("Heroes");
    }    

    public void OnClickCountinueButton()
    {
        SoundManager.Instance.PlayButtonSound();
        LevelGameConfigSO levelGameConfigSO = Resources.Load<LevelGameConfigSO>("LevelConfig/" + PlayerDataController.CurrentLevel);
        ViewManager.Instance.LoadScene(levelGameConfigSO.mapID);
    }    
}

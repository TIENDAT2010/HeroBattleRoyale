using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class IngameView : BaseView
{
    [SerializeField] RectTransform ingameView = null;
    [SerializeField] ScrollRect heroScrollRect = null;
    [SerializeField] RectTransform contentRect = null;
    [SerializeField] RectTransform heroScrollViewRect = null;
    [SerializeField] RectTransform targetCoinPos = null;
    [SerializeField] HorizontalLayoutGroup horizontalLayoutGroup = null;
    [SerializeField] Slider playerSlider = null;
    [SerializeField] Slider enemySlider = null;
    [SerializeField] Text playerHealthText = null;
    [SerializeField] Text enemyHealthText = null;


    private List<UnlockedUIHeroItem> listUnlockedHeroItem = new List<UnlockedUIHeroItem>();


    PointerEventData pointer = new PointerEventData(EventSystem.current);
    List<RaycastResult> raycastResult = new List<RaycastResult>();
    private UnlockedUIHeroItem selectedHeroItem = null;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject())
        {
            pointer.position = Input.mousePosition;
            EventSystem.current.RaycastAll(pointer, raycastResult);
            foreach (RaycastResult result in raycastResult)
            {
                for (int i = 0; i < listUnlockedHeroItem.Count; i++)
                {
                    if (listUnlockedHeroItem[i].gameObject == result.gameObject)
                    {
                        selectedHeroItem = listUnlockedHeroItem[i];
                    }
                }
            }
            raycastResult.Clear();
        }
        if (Input.GetMouseButton(0) && selectedHeroItem != null && selectedHeroItem.IsCooling == false)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                heroScrollRect.enabled = false;
                heroScrollRect.StopMovement();
                IngameManager.Instance.OnHeroDrag(Input.mousePosition, selectedHeroItem.HeroName);
            }
        }
        if (Input.GetMouseButtonUp(0) && selectedHeroItem != null && selectedHeroItem.IsCooling == false)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                IngameManager.Instance.OnHeroEndDrag(Input.mousePosition, selectedHeroItem.HeroName, selectedHeroItem.CoinToDeploy, selectedHeroItem);
            }
            selectedHeroItem = null;
            heroScrollRect.enabled = true;
        }


        playerHealthText.text = IngameManager.Instance.PlayerMainTower.CurrentHealth + "/" + IngameManager.Instance.PlayerMainTower.TotalHealth;
        enemyHealthText.text = IngameManager.Instance.EnemyMainTower.CurrentHealth + "/" + IngameManager.Instance.EnemyMainTower.TotalHealth;


        playerSlider.value = IngameManager.Instance.PlayerMainTower.CurrentHealth / IngameManager.Instance.PlayerMainTower.TotalHealth;
        enemySlider.value = IngameManager.Instance.EnemyMainTower.CurrentHealth / IngameManager.Instance.EnemyMainTower.TotalHealth;
    }



    private IEnumerator MoveCoinItem(RectTransform coinItem, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        float t = 0;
        float moveTime = 0.5f;
        Vector2 startPos = coinItem.anchoredPosition;
        while (t < moveTime)
        {
            t += Time.deltaTime;
            float factor = t / moveTime;
            coinItem.anchoredPosition = Vector2.Lerp(startPos, targetCoinPos.anchoredPosition, factor);
            yield return null;
        }
        coinItem.gameObject.SetActive(false);
        IngameManager.Instance.UpdateCoin(1);
        SoundManager.Instance.PlaySound(SoundManager.Instance.addCoin);
    }



    public override void OnShow()
    {
        
    }


    public override void OnHide()
    {
        for (int i = 0; i < listUnlockedHeroItem.Count; i++)
        {
            listUnlockedHeroItem[i].gameObject.SetActive(false);
        }
        listUnlockedHeroItem.Clear();
        gameObject.SetActive(false);
    }



    public void CreateUnlockedHeroItem(string heroName, HeroID heroID, int level)
    {
        UnlockedUIHeroItem unlockedUIHeroItem = PoolManager.Instance.GetUnlockedHeroItem();
        unlockedUIHeroItem.transform.SetParent(contentRect);
        unlockedUIHeroItem.transform.localScale = Vector3.one;
        unlockedUIHeroItem.OnInit(heroName, heroID, level);
        listUnlockedHeroItem.Add(unlockedUIHeroItem);
    }    



    public void SetScrollView()
    {
        int itemCount = listUnlockedHeroItem.Count;
        Vector2 sizeDelta = heroScrollViewRect.sizeDelta;
        float padding = horizontalLayoutGroup.padding.left + horizontalLayoutGroup.padding.right;
        sizeDelta.x = (170 * itemCount) + padding + ((itemCount - 1) * horizontalLayoutGroup.spacing) + (170 * 2f);
        sizeDelta.x = Mathf.Clamp(sizeDelta.x, 515f, 1000f);
        heroScrollViewRect.sizeDelta = sizeDelta;
    }



    /// <summary>
    /// Create the coin item when an enemy hero died.
    /// </summary>
    /// <param name="heroPos"></param>
    /// <param name="coinReward"></param>
    public void OnEnemyHeroDead(Vector3 heroPos, int coinReward)
    {
        Vector2 screenPont = Camera.main.WorldToScreenPoint(heroPos);
        float delaytime = 0.3f;
        for(int i = 0; i < coinReward; i++)
        {
            Vector2 randomPoint = screenPont + Random.insideUnitCircle * 50;
            Vector2 localPoint = Vector2.zero;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(ingameView, randomPoint, null, out localPoint))
            {
                RectTransform coinItem = PoolManager.Instance.GetCoinItem(ingameView);
                coinItem.anchoredPosition = localPoint;
                StartCoroutine(MoveCoinItem(coinItem, delaytime));
                delaytime += 0.05f;
            }
        }
    }



    public void OnClickViewHeroButton()
    {
        SoundManager.Instance.PlayButtonSound();
        ViewManager.Instance.LoadScene("Heroes");
    }


    public void OnClickSwithViewButton()
    {
        CameraParentController cameraParentController = FindAnyObjectByType<CameraParentController>();
        cameraParentController.SwitchViewMode();
    }    


    public void OnClickHomeButton()
    {
        SoundManager.Instance.PlayButtonSound();
        ViewManager.Instance.LoadScene("Home");
    }    


    public void OnClickNextLevelButton()
    {
        int currentLevel = PlayerDataController.CurrentLevel;
        currentLevel++;
        PlayerDataController.UpdateLevel(currentLevel);
        LevelGameConfigSO levelGameConfigSO = Resources.Load<LevelGameConfigSO>("LevelConfig/" + PlayerDataController.CurrentLevel);
        ViewManager.Instance.LoadScene(levelGameConfigSO.mapID);

    }
    

    public void OnClickCoinButton()
    {
        IngameManager.Instance.UpdateCoin(100);
    }    
   
}

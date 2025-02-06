
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HerosView : BaseView
{
    [SerializeField] private RectTransform contentRect = null;
    [SerializeField] private Button unlockHeroButton = null;
    [SerializeField] private Button upgradeHeroButton = null;
    [SerializeField] private SwordHeroPanelController swordHeroPanel = null;
    [SerializeField] private BladeHeroPanelController bladeHeroPanel = null;
    [SerializeField] private BowHeroPanelController bowHeroPanel = null;
    [SerializeField] private KnifeHeroPanelController knifeHeroPanel = null;
    [SerializeField] private MageHeroPanelController mageHeroPanel = null;
    [SerializeField] private HealerHeroPanelController healerHeroPanel = null;
    [SerializeField] private BurnerHeroPanelController burnerHeroPanel = null;
    [SerializeField] private Text totalCashText = null;
    [SerializeField] private Text heroNameText = null;
    [SerializeField] private Text priceText = null;
    [SerializeField] private HeroUIItem heroItemUIPrefab = null;

    private List<HeroUIItem> listUnlockedHero = new List<HeroUIItem>();
    private HeroPanelControler currentHeroPanel = null;
    private HeroesManager heroesManager = null;
    private HeroUIItem currentHeroUIItem = null;



    public HeroUIItem GetHeroUIItem()
    {
        HeroUIItem heroUIItem = listUnlockedHero.Where(a => !a.gameObject.activeSelf).FirstOrDefault();
        if (heroUIItem == null)
        {
            heroUIItem = Instantiate(heroItemUIPrefab, Vector3.zero, Quaternion.identity);
            heroUIItem.transform.SetParent(contentRect);
            heroUIItem.transform.localScale = Vector3.one;
            listUnlockedHero.Add(heroUIItem);
        }
        heroUIItem.gameObject.SetActive(true);
        return heroUIItem;
    }






    public override void OnShow()
    {
        if (heroesManager == null) { heroesManager = FindFirstObjectByType<HeroesManager>(); }

        totalCashText.text = PlayerDataController.CurrentCash.ToString();

        if(contentRect.childCount == 0)
        {

            //Load blade hero configs
            PlayerBladeHeroConfigSO[] bladeHeroConfigs = Resources.LoadAll<PlayerBladeHeroConfigSO>("HeroConfig/BladeHeroConfig/Player");
            for (int i = 0; i < bladeHeroConfigs.Length; i++)
            {
                HeroUIItem heroUIItem = GetHeroUIItem();
                heroUIItem.OnInit(bladeHeroConfigs[i].name, HeroID.BladeHero);
            }



            //Load bow hero configs
            PlayerBowHeroConfigSO[] bowHeroConfigs = Resources.LoadAll<PlayerBowHeroConfigSO>("HeroConfig/BowHeroConfig/Player");
            for (int i = 0; i < bowHeroConfigs.Length; i++)
            {
                HeroUIItem heroUIItem = GetHeroUIItem();
                heroUIItem.OnInit(bowHeroConfigs[i].name, HeroID.BowHero);
            }



            //Load knife hero configs
            PlayerKnifeHeroConfigSO[] knifeHeroConfigs = Resources.LoadAll<PlayerKnifeHeroConfigSO>("HeroConfig/KnifeHeroConfig/Player");
            for (int i = 0; i < knifeHeroConfigs.Length; i++)
            {
                HeroUIItem heroUIItem = GetHeroUIItem();
                heroUIItem.OnInit(knifeHeroConfigs[i].name, HeroID.KnifeHero);
            }


            //Load mage hero configs
            PlayerMageHeroConfigSO[] mageHeroConfig = Resources.LoadAll<PlayerMageHeroConfigSO>("HeroConfig/MageHeroConfig/Player");
            for (int i = 0; i < mageHeroConfig.Length; i++)
            {
                HeroUIItem heroUIItem = GetHeroUIItem();
                heroUIItem.OnInit(mageHeroConfig[i].name, HeroID.MageHero);
            }


            //Load sword hero configs
            PlayerSwordHeroConfigSO[] swordHeroConfig = Resources.LoadAll<PlayerSwordHeroConfigSO>("HeroConfig/SwordHeroConfig/Player");
            for (int i = 0; i < swordHeroConfig.Length; i++)
            {
                HeroUIItem heroUIItem = GetHeroUIItem();
                heroUIItem.OnInit(swordHeroConfig[i].name, HeroID.SwordHero);
            }



            //Load healer hero configs
            PlayerHealerHeroConfigSO[] healerHeroConfig = Resources.LoadAll<PlayerHealerHeroConfigSO>("HeroConfig/HealerHeroConfig/Player");
            for (int i = 0; i < healerHeroConfig.Length; i++)
            {
                HeroUIItem heroUIItem = GetHeroUIItem();
                heroUIItem.OnInit(healerHeroConfig[i].name, HeroID.HealerHero);
            }



            //Load burner hero configs
            PlayerBurnerHeroConfigSO[] burnerHeroConfig = Resources.LoadAll<PlayerBurnerHeroConfigSO>("HeroConfig/BurnerHeroConfig/Player");
            for (int i = 0; i < burnerHeroConfig.Length; i++)
            {
                HeroUIItem heroUIItem = GetHeroUIItem();
                heroUIItem.OnInit(burnerHeroConfig[i].name, HeroID.BurnerHero);
            }



            HeroData firstHeroUnlock = PlayerDataController.GetFirstUnlockHero();
            for(int i = 0; i < listUnlockedHero.Count; i++)
            {
                if(listUnlockedHero[i].HeroName == firstHeroUnlock.heroName)
                {
                    currentHeroUIItem = listUnlockedHero[i];
                    OnHeroUIItemClicked(listUnlockedHero[i]);
                    break;
                }
            }
        }
        else
        {
            heroesManager.ShowHero(currentHeroUIItem.HeroName);
            heroesManager.PlayEffect();
        }
    }



    public override void OnHide()
    {
        gameObject.SetActive(false);
    }



    /// <summary>
    /// Update the hero panel with given hero ui item.
    /// </summary>
    /// <param name="heroUIItem"></param>
    public void OnHeroUIItemClicked(HeroUIItem heroUIItem)
    {
        currentHeroUIItem.RedMark.SetActive(false);
        currentHeroUIItem = heroUIItem;
        currentHeroUIItem.RedMark.SetActive(true);
        heroNameText.text = currentHeroUIItem.HeroName;
        heroesManager.ShowHero(currentHeroUIItem.HeroName);
        heroesManager.PlayEffect();

        if (currentHeroUIItem.HeroID == HeroID.SwordHero)
        {
            swordHeroPanel.gameObject.SetActive(true);
            swordHeroPanel.UpdateHeroInfos(heroUIItem.HeroName);
            currentHeroPanel = swordHeroPanel;

            bladeHeroPanel.gameObject.SetActive(false);
            bowHeroPanel.gameObject.SetActive(false);
            knifeHeroPanel.gameObject.SetActive(false);
            mageHeroPanel.gameObject.SetActive(false);
            healerHeroPanel.gameObject.SetActive(false);
            burnerHeroPanel.gameObject.SetActive(false);
        }
        else if (currentHeroUIItem.HeroID == HeroID.BladeHero)
        {
            bladeHeroPanel.gameObject.SetActive(true);
            bladeHeroPanel.UpdateHeroInfos(heroUIItem.HeroName);
            currentHeroPanel = bladeHeroPanel;

            swordHeroPanel.gameObject.SetActive(false);
            bowHeroPanel.gameObject.SetActive(false);
            knifeHeroPanel.gameObject.SetActive(false);
            mageHeroPanel.gameObject.SetActive(false);
            healerHeroPanel.gameObject.SetActive(false);
            burnerHeroPanel.gameObject.SetActive(false);
        }
        else if (currentHeroUIItem.HeroID == HeroID.BowHero)
        {
            bowHeroPanel.gameObject.SetActive(true);
            bowHeroPanel.UpdateHeroInfos(heroUIItem.HeroName);
            currentHeroPanel = bowHeroPanel;

            swordHeroPanel.gameObject.SetActive(false);
            bladeHeroPanel.gameObject.SetActive(false);
            knifeHeroPanel.gameObject.SetActive(false);
            mageHeroPanel.gameObject.SetActive(false);
            healerHeroPanel.gameObject.SetActive(false);
            burnerHeroPanel.gameObject.SetActive(false);
        }
        else if(currentHeroUIItem.HeroID == HeroID.KnifeHero)
        {
            knifeHeroPanel.gameObject.SetActive(true);
            knifeHeroPanel.UpdateHeroInfos(heroUIItem.HeroName);
            currentHeroPanel = knifeHeroPanel;

            swordHeroPanel.gameObject.SetActive(false);
            bladeHeroPanel.gameObject.SetActive(false);
            bowHeroPanel.gameObject.SetActive(false);
            mageHeroPanel.gameObject.SetActive(false);
            healerHeroPanel.gameObject.SetActive(false);
            burnerHeroPanel.gameObject.SetActive(false);
        }
        else if(currentHeroUIItem.HeroID == HeroID.MageHero)
        {
            mageHeroPanel.gameObject.SetActive(true);
            mageHeroPanel.UpdateHeroInfos(heroUIItem.HeroName);
            currentHeroPanel = mageHeroPanel;

            swordHeroPanel.gameObject.SetActive(false);
            bladeHeroPanel.gameObject.SetActive(false);
            bowHeroPanel.gameObject.SetActive(false);
            knifeHeroPanel.gameObject.SetActive(false);
            healerHeroPanel.gameObject.SetActive(false);
            burnerHeroPanel.gameObject.SetActive(false);
        }
        else if(currentHeroUIItem.HeroID == HeroID.HealerHero)
        {
            healerHeroPanel.gameObject.SetActive(true);
            healerHeroPanel.UpdateHeroInfos(heroUIItem.HeroName);
            currentHeroPanel = healerHeroPanel;

            swordHeroPanel.gameObject.SetActive(false);
            bladeHeroPanel.gameObject.SetActive(false);
            bowHeroPanel.gameObject.SetActive(false);
            knifeHeroPanel.gameObject.SetActive(false);
            mageHeroPanel.gameObject.SetActive(false);
            burnerHeroPanel.gameObject.SetActive(false);
        }
        else if(currentHeroUIItem.HeroID == HeroID.BurnerHero)
        {
            burnerHeroPanel.gameObject.SetActive(true);
            burnerHeroPanel.UpdateHeroInfos(heroUIItem.HeroName);
            currentHeroPanel = burnerHeroPanel;

            swordHeroPanel.gameObject.SetActive(false);
            bladeHeroPanel.gameObject.SetActive(false);
            bowHeroPanel.gameObject.SetActive(false);
            knifeHeroPanel.gameObject.SetActive(false);
            mageHeroPanel.gameObject.SetActive(false);
            healerHeroPanel.gameObject.SetActive(false);
        }    



        if (!currentHeroPanel.IsUnlocked)
        {
            upgradeHeroButton.gameObject.SetActive(false);
            unlockHeroButton.gameObject.SetActive(true);
            priceText.gameObject.SetActive(true);
            priceText.text = currentHeroPanel.PriceToUnlock.ToString();
            unlockHeroButton.interactable = currentHeroPanel.PriceToUnlock <= PlayerDataController.CurrentCash;
        }
        else
        {
            unlockHeroButton.gameObject.SetActive(false);
            if (currentHeroPanel.IsMaxLevel)
            {
                upgradeHeroButton.gameObject.SetActive(false);
                priceText.gameObject.SetActive(false);
            }
            else
            {
                priceText.gameObject.SetActive(true);
                priceText.text = currentHeroPanel.PriceToUnlock.ToString();
                upgradeHeroButton.gameObject.SetActive(true);
                upgradeHeroButton.interactable = currentHeroPanel.PriceToUnlock <= PlayerDataController.CurrentCash;
            }
        }
    }



    public void OnClickUnLockButton()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.heroUnlock);
        PlayerDataController.UpdateUnlockHero(currentHeroUIItem.HeroName, currentHeroUIItem.HeroID);
        PlayerDataController.UpdateCash(-currentHeroPanel.PriceToUnlock);
        totalCashText.text = PlayerDataController.CurrentCash.ToString();
        OnHeroUIItemClicked(currentHeroUIItem);
    }    


    public void OnClickUpgradeButton()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.heroUpgrade);
        PlayerDataController.UpgradeHero(currentHeroUIItem.HeroName);
        PlayerDataController.UpdateCash(-currentHeroPanel.PriceToUnlock);
        totalCashText.text = PlayerDataController.CurrentCash.ToString();
        OnHeroUIItemClicked(currentHeroUIItem);
    }

    public void OnClickHomeButton()
    {
        SoundManager.Instance.PlayButtonSound();
        ViewManager.Instance.LoadScene("Home");
    }    


    public void OnClickAddCashButton()
    {
        SoundManager.Instance.PlayButtonSound();
        PlayerDataController.UpdateCash(100);
        totalCashText.text = PlayerDataController.CurrentCash.ToString();
        OnHeroUIItemClicked(currentHeroUIItem);
    }    
}

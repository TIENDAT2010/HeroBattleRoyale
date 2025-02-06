using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnlockedUIHeroItem : MonoBehaviour
{
    [SerializeField] private Image cooldownImage = null;
    [SerializeField] private Image heroIcon = null;
    [SerializeField] private Text coinText = null;

    public string HeroName { private set ; get; }
    public bool IsCooling {  private set ; get; }
    public int CoinToDeploy { private set; get; }
    public float CooldownTime { private set; get; }

    public void OnInit(string heroName, HeroID heroID, int level)
    {
        IsCooling = false;
        HeroName = heroName;
        cooldownImage.gameObject.SetActive(false);
        heroIcon.sprite = Resources.Load<Sprite>("HeroIcons/" + heroName);

        if(heroID == HeroID.BladeHero)
        {
            PlayerBladeHeroConfigSO playerHeroConfig = Resources.Load<PlayerBladeHeroConfigSO>("HeroConfig/BladeHeroConfig/Player/" + heroName);
            PlayerBladeHeroData heroData = playerHeroConfig.GetHeroData(level);
            CoinToDeploy = heroData.coinToDeploy;
            CooldownTime = heroData.cooldown;
        }
        else if(heroID == HeroID.BowHero)
        {
            PlayerBowHeroConfigSO playerHeroConfig = Resources.Load<PlayerBowHeroConfigSO>("HeroConfig/BowHeroConfig/Player/" + heroName);
            PlayerBowHeroData heroData = playerHeroConfig.GetHeroData(level);
            CoinToDeploy = heroData.coinToDeploy;
            CooldownTime = heroData.cooldown;
        }
        else if (heroID == HeroID.BurnerHero)
        {
            PlayerBurnerHeroConfigSO playerHeroConfig = Resources.Load<PlayerBurnerHeroConfigSO>("HeroConfig/BurnerHeroConfig/Player/" + heroName);
            PlayerBurnerHeroData heroData = playerHeroConfig.GetHeroData(level);
            CoinToDeploy = heroData.coinToDeploy;
            CooldownTime = heroData.cooldown;
        }
        else if (heroID == HeroID.HealerHero)
        {
            PlayerHealerHeroConfigSO playerHeroConfig = Resources.Load<PlayerHealerHeroConfigSO>("HeroConfig/HealerHeroConfig/Player/" + heroName);
            PlayerHealerHeroData heroData = playerHeroConfig.GetHeroData(level);
            CoinToDeploy = heroData.coinToDeploy;
            CooldownTime = heroData.cooldown;
        }
        else if (heroID == HeroID.KnifeHero)
        {
            PlayerKnifeHeroConfigSO playerHeroConfig = Resources.Load<PlayerKnifeHeroConfigSO>("HeroConfig/KnifeHeroConfig/Player/" + heroName);
            PlayerKnifeHeroData heroData = playerHeroConfig.GetHeroData(level);
            CoinToDeploy = heroData.coinToDeploy;
            CooldownTime = heroData.cooldown;
        }
        else if (heroID == HeroID.MageHero)
        {
            PlayerMageHeroConfigSO playerHeroConfig = Resources.Load<PlayerMageHeroConfigSO>("HeroConfig/MageHeroConfig/Player/" + heroName);
            PlayerMageHeroData heroData = playerHeroConfig.GetHeroData(level);
            CoinToDeploy = heroData.coinToDeploy;
            CooldownTime = heroData.cooldown;
        }
        else if (heroID == HeroID.SwordHero)
        {
            PlayerSwordHeroConfigSO playerHeroConfig = Resources.Load<PlayerSwordHeroConfigSO>("HeroConfig/SwordHeroConfig/Player/" + heroName);
            PlayerSwordHeroData heroData = playerHeroConfig.GetHeroData(level);
            CoinToDeploy = heroData.coinToDeploy;
            CooldownTime = heroData.cooldown;
        }

        coinText.text = CoinToDeploy.ToString();
    }


    public void Cooldown()
    {
        IsCooling = true;
        cooldownImage.gameObject.SetActive(true);
        StartCoroutine(CRCooldownFillAmount());
    }

    private IEnumerator CRCooldownFillAmount()
    {
        float t = 0;
        float moveTime = CooldownTime;
        while (t < moveTime)
        {
            t += Time.deltaTime;
            float factor = t / moveTime;
            cooldownImage.fillAmount = Mathf.Lerp(1, 0, factor);
            yield return null;
        }
        cooldownImage.gameObject.SetActive(false);
        IsCooling = false;
    }    
}

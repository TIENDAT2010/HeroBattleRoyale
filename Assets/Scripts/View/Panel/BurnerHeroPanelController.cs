using UnityEngine;
using UnityEngine.UI;

public class BurnerHeroPanelController : HeroPanelControler
{
    [SerializeField] private Text damagePerSecondText = null;
    [SerializeField] private Slider damagePerSecondSlider = null;

    [SerializeField] private Text attackRangeText = null;
    [SerializeField] private Slider attackRangeSlider = null;



    public override void UpdateHeroInfos(string heroName)
    {
        PlayerBurnerHeroConfigSO burnerHeroConfig = Resources.Load<PlayerBurnerHeroConfigSO>("HeroConfig/BurnerHeroConfig/Player/" + heroName);
        IsUnlocked = PlayerDataController.IsUnlockHero(heroName);
        PlayerBurnerHeroData currentData = burnerHeroConfig.GetHeroData((IsUnlocked == false) ? 1 : PlayerDataController.GetLevelHero(heroName));
        PlayerBurnerHeroData maxData = burnerHeroConfig.GetHeroData(burnerHeroConfig.GetMaxLevel());
        PriceToUnlock = burnerHeroConfig.GetHeroData((IsUnlocked == false) ? 1 : currentData.level + 1).priceUnlock;

        IsMaxLevel = currentData.level == maxData.level;
        if (IsMaxLevel) { PriceToUnlock = 0; }

        levelText.text = (!IsUnlocked) ? "Level: 1" : "Level: " + currentData.level.ToString();

        healthText.text = currentData.health.ToString() + "/" + maxData.health.ToString();
        healthSlider.value = currentData.health / maxData.health;

        speedRunText.text = currentData.speedRun.ToString() + "/" + maxData.speedRun.ToString();
        speedRunSlider.value = currentData.speedRun / maxData.speedRun;

        damagePerSecondText.text = currentData.damagePerSecond.ToString() + "/" + maxData.damagePerSecond.ToString();
        damagePerSecondSlider.value = currentData.damagePerSecond / maxData.damagePerSecond;

        attackRangeText.text = currentData.attackRange.ToString() + "/" + maxData.attackRange.ToString();
        attackRangeSlider.value = currentData.attackRange / maxData.attackRange;
    }
}

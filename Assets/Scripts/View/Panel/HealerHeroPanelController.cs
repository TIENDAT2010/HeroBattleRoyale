using UnityEngine;
using UnityEngine.UI;

public class HealerHeroPanelController : HeroPanelControler
{
    [SerializeField] private Text healPerSecondText = null;
    [SerializeField] private Slider healPerSecondSlider = null;

    [SerializeField] private Text healRangeText = null;
    [SerializeField] private Slider healRangeSlider = null;



    public override void UpdateHeroInfos(string heroName)
    {
        PlayerHealerHeroConfigSO healerHeroConfig = Resources.Load<PlayerHealerHeroConfigSO>("HeroConfig/HealerHeroConfig/Player/" + heroName);
        IsUnlocked = PlayerDataController.IsUnlockHero(heroName);
        PlayerHealerHeroData currentData = healerHeroConfig.GetHeroData((IsUnlocked == false) ? 1 : PlayerDataController.GetLevelHero(heroName));
        PlayerHealerHeroData maxData = healerHeroConfig.GetHeroData(healerHeroConfig.GetMaxLevel());
        PriceToUnlock = healerHeroConfig.GetHeroData((IsUnlocked == false) ? 1 : currentData.level + 1).priceUnlock;

        IsMaxLevel = currentData.level == maxData.level;
        if (IsMaxLevel) { PriceToUnlock = 0; }

        levelText.text = (!IsUnlocked) ? "Level: 1" : "Level: " + currentData.level.ToString();

        healthText.text = currentData.health.ToString() + "/" + maxData.health.ToString();
        healthSlider.value = currentData.health / maxData.health;

        speedRunText.text = currentData.speedRun.ToString() + "/" + maxData.speedRun.ToString();
        speedRunSlider.value = currentData.speedRun / maxData.speedRun;

        healPerSecondText.text = currentData.healPerSecond.ToString() + "/" + maxData.healPerSecond.ToString();
        healPerSecondSlider.value = currentData.healPerSecond / maxData.healPerSecond;

        healRangeText.text = currentData.healRange.ToString() + "/" + maxData.healRange.ToString();
        healRangeSlider.value = currentData.healRange / maxData.healRange;
    }
}

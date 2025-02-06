using UnityEngine;
using UnityEngine.UI;

public class BowHeroPanelController : HeroPanelControler
{
    [SerializeField] private Text damageText = null;
    [SerializeField] private Slider damageSlider = null;

    [SerializeField] private Text attackRangeText = null;
    [SerializeField] private Slider attackRangeSlider = null;





    public override void UpdateHeroInfos(string heroName)
    {
        PlayerBowHeroConfigSO bowHeroConfig = Resources.Load<PlayerBowHeroConfigSO>("HeroConfig/BowHeroConfig/Player/" + heroName);
        IsUnlocked = PlayerDataController.IsUnlockHero(heroName);
        PlayerBowHeroData currentData = bowHeroConfig.GetHeroData((IsUnlocked == false) ? 1 : PlayerDataController.GetLevelHero(heroName));
        PlayerBowHeroData maxData = bowHeroConfig.GetHeroData(bowHeroConfig.GetMaxLevel());
        PriceToUnlock = bowHeroConfig.GetHeroData((IsUnlocked == false) ? 1 : currentData.level + 1).priceUnlock;

        IsMaxLevel = currentData.level == maxData.level;
        if (IsMaxLevel) { PriceToUnlock = 0; }


        levelText.text = (!IsUnlocked) ? "Level: 1" : "Level: " + currentData.level.ToString();

        healthText.text = currentData.health.ToString() + "/" + maxData.health.ToString();
        healthSlider.value = currentData.health / maxData.health;

        speedRunText.text = currentData.speedRun.ToString() + "/" + maxData.speedRun.ToString();
        speedRunSlider.value = currentData.speedRun / maxData.speedRun;

        damageText.text = currentData.damage.ToString() + "/" + maxData.damage.ToString();
        damageSlider.value = currentData.damage / maxData.damage;

        attackRangeText.text = currentData.attackRange.ToString() + "/" + maxData.attackRange.ToString();
        attackRangeSlider.value = currentData.attackRange / maxData.attackRange;
    }
}

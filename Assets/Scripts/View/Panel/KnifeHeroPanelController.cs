using UnityEngine;
using UnityEngine.UI;

public class KnifeHeroPanelController : HeroPanelControler
{
    [SerializeField] private Text damageText = null;
    [SerializeField] private Slider damageSlider = null;

    [SerializeField] private Text defenseText = null;
    [SerializeField] private Slider defenseSlider = null;





    public override void UpdateHeroInfos(string heroName)
    {
        PlayerKnifeHeroConfigSO knifeHeroConfig = Resources.Load<PlayerKnifeHeroConfigSO>("HeroConfig/KnifeHeroConfig/Player/" + heroName);
        IsUnlocked = PlayerDataController.IsUnlockHero(heroName);
        PlayerKnifeHeroData currentData = knifeHeroConfig.GetHeroData((IsUnlocked == false) ? 1 : PlayerDataController.GetLevelHero(heroName));
        PlayerKnifeHeroData maxData = knifeHeroConfig.GetHeroData(knifeHeroConfig.GetMaxLevel());
        PriceToUnlock = knifeHeroConfig.GetHeroData((IsUnlocked == false) ? 1 : currentData.level + 1).priceUnlock;

        IsMaxLevel = currentData.level == maxData.level;
        if (IsMaxLevel) { PriceToUnlock = 0; }

        levelText.text = (!IsUnlocked) ? "Level: 1" : "Level: " + currentData.level.ToString();

        healthText.text = currentData.health.ToString() + "/" + maxData.health.ToString();
        healthSlider.value = currentData.health / maxData.health;

        speedRunText.text = currentData.speedRun.ToString() + "/" + maxData.speedRun.ToString();
        speedRunSlider.value = currentData.speedRun / maxData.speedRun;

        damageText.text = currentData.damage.ToString() + "/" + maxData.damage.ToString();
        damageSlider.value = currentData.damage / maxData.damage;

        defenseText.text = currentData.defense.ToString() + "/" + maxData.defense.ToString();
        defenseSlider.value = currentData.defense / maxData.defense;
    }
}

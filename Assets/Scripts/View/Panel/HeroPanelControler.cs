using UnityEngine;
using UnityEngine.UI;

public class HeroPanelControler : MonoBehaviour
{
    [SerializeField] protected Text levelText = null;

    [SerializeField] protected Text healthText = null;
    [SerializeField] protected Slider healthSlider = null;

    [SerializeField] protected Text speedRunText = null;
    [SerializeField] protected Slider speedRunSlider = null;

    public int PriceToUnlock { get; protected set; }
    public bool IsMaxLevel { get; protected set; }
    public bool IsUnlocked { get; protected set; }


    /// <summary>
    /// Update the infos of the given hero name.
    /// </summary>
    /// <param name="heroName"></param>
    public virtual void UpdateHeroInfos(string heroName) { }
}

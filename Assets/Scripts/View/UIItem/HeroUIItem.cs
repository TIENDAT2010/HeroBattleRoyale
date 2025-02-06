using UnityEngine;
using UnityEngine.UI;

public class HeroUIItem : MonoBehaviour
{
    [SerializeField] private Image heroIcon = null;
    [SerializeField] private GameObject redMark = null;

    public string HeroName { private set; get; }
    public HeroID HeroID { private set; get; }
    public GameObject RedMark => redMark;


    public void OnInit(string heroName, HeroID heroID)
    {
        HeroName = heroName;
        HeroID = heroID;
        heroIcon.sprite = Resources.Load<Sprite>("HeroIcons/" + heroName);
        redMark.SetActive(false);
    }


    public void OnClickHeroUIButton()
    {
        redMark.SetActive(true);
        SoundManager.Instance.PlaySound(SoundManager.Instance.tick);
        ViewManager.Instance.HerosView.OnHeroUIItemClicked(this);
    }
}

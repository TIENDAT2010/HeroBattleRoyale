using UnityEngine;
using System.Collections.Generic;

public class HeroesManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem particleEffect = null;
    [SerializeField] GameObject[] heroPrefabs = null;

    private void Start()
    {
        ViewManager.Instance.SetActiveView(ViewType.HerosView);
    }



    /// <summary>
    /// Show the hero object with name.
    /// </summary>
    /// <param name="name"></param>
    public void ShowHero(string name)
    {
        for (int i = 0; i < heroPrefabs.Length; i++)
        {
            if (heroPrefabs[i].name == name)
            {
                heroPrefabs[i].gameObject.SetActive(true);
            }    
            else
            {
                heroPrefabs[i].SetActive(false);
            }
        }
    }



    /// <summary>
    /// Play the particle effect.
    /// </summary>
    public void PlayEffect()
    {
        particleEffect.Play();
    }
}

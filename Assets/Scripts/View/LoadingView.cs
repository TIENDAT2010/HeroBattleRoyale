using UnityEngine;
using UnityEngine.UI;

public class LoadingView : BaseView
{
    [SerializeField] private Text loadingText = null;
    [SerializeField] private Slider loadingSlider = null;

    public override void OnShow()
    {
        loadingSlider.value = 0f;
    }

    public override void OnHide()
    {
        loadingSlider.value = 0f;
        gameObject.SetActive(false);
    }


    /// <summary>
    /// Set the loading amount.
    /// </summary>
    /// <param name="amount"></param>
    public void SetLoadingAmount(float amount)
    {
        loadingSlider.value = amount;
        loadingText.text = System.Math.Round((amount / 1f) * 100f, 2).ToString() + "%";
    }
}

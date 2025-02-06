using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CoinPanelController : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup = null;
    [SerializeField] private RectTransform extraCoinTrans = null;
    [SerializeField] private Image timeSlider = null;
    [SerializeField] private Text timeText = null;
    [SerializeField] private Text coinText = null;


    private void OnEnable()
    {
        extraCoinTrans.gameObject.SetActive(false);
        StartCoroutine(CRCountdown());
    }

    private void Update()
    {
        coinText.text = IngameManager.Instance.CurrentCoin.ToString();
    }




    /// <summary>
    /// Coroutine count down to add more coin.
    /// </summary>
    /// <returns></returns>
    private IEnumerator CRCountdown()
    {
        while (gameObject.activeSelf)
        {
            float t = 5f;
            while (t > 0)
            {
                t -= Time.deltaTime;
                timeSlider.fillAmount = t / 5f;
                timeText.text = Mathf.RoundToInt(t).ToString();
                yield return null;
            }

            IngameManager.Instance.UpdateCoin(2);
            StartCoroutine(CRMoveCanvasGroup());
        }
    }



    /// <summary>
    /// Coroutine kove the canvas group up.
    /// </summary>
    /// <returns></returns>
    private IEnumerator CRMoveCanvasGroup()
    {
        extraCoinTrans.gameObject.SetActive(true);
        float t = 0;
        float moveTime = 0.5f;
        Vector2 startPos = extraCoinTrans.anchoredPosition;
        Vector2 endPos = new Vector2(startPos.x, startPos.y + 200);
        while (t < moveTime)
        {
            t += Time.deltaTime;
            float factor = t / moveTime;
            extraCoinTrans.anchoredPosition = Vector2.Lerp(startPos, endPos, factor);
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, factor);
            yield return null;
        }

        canvasGroup.alpha = 1f;
        extraCoinTrans.anchoredPosition = startPos;
        extraCoinTrans.gameObject.SetActive(false);
    }
}

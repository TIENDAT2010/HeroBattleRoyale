using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] private Image fadePanel = null;
    [SerializeField] private Transform cameraParent = null;
    [SerializeField] private Animator redKingAnimator = null;
    [SerializeField] private Animator blueKingAnimator = null;
    [SerializeField] private Transform skipButton = null;
    [SerializeField] private Transform clearDataButton = null;

    [Header("Brand Hero")]
    [SerializeField] private GameObject brandHero = null;
    [SerializeField] private Animator brandAnimator = null;
    [SerializeField] private Transform fireBurnEffect = null;
    [SerializeField] private ParticleSystem chargeEffect = null;
    [SerializeField] private ParticleSystem brandSpawnEffect = null;

    [Header("Richard Hero")]
    [SerializeField] private GameObject richardHero = null;
    [SerializeField] private Animator richardAnimator = null;
    [SerializeField] private ParticleSystem richardSpawnEffect = null;

    [Header("Headsman Hero")]
    [SerializeField] private GameObject headsmanHero = null;
    [SerializeField] private Animator headsmanAnimator = null;
    [SerializeField] private ParticleSystem headsmanSpawnEffect = null;

    [Header("Golem Hero")]
    [SerializeField] private GameObject golemHero = null;
    [SerializeField] private Animator golemAnimator = null;
    [SerializeField] private ParticleSystem golemAttackEffect = null;
    [SerializeField] private ParticleSystem golemSpawnEffect = null;

    [Header("Drakert Hero 01")]
    [SerializeField] private GameObject drakertHero01 = null;
    [SerializeField] private Animator drakertAnimator01 = null;
    [SerializeField] private ParticleSystem drakertSpawnEffect01 = null;

    [Header("Drakert Hero 02")]
    [SerializeField] private GameObject drakertHero02 = null;
    [SerializeField] private Animator drakertAnimator02 = null;
    [SerializeField] private ParticleSystem drakertSpawnEffect02 = null;

    private Vector3 camRedKingTowerPos = new Vector3(2f, 0f, -20f);
    private Vector3 camRedKingTowerAngles = new Vector3(0f, -130f, 0f);
    private Vector3 camBlueKingTowerPos = new Vector3(2f, 0f, 20f);
    private Vector3 camBlueKingTowerAngles = new Vector3(0f, -25f, 0f);

    private Vector3 camBattlePos = new Vector3(20f, 15f, 0f);
    private Vector3 camEndBattlePos = new Vector3(13f, 5f, 0f);
    private Vector3 camBattleAngles = new Vector3(0f, -90f, 0f);

    private IEnumerator Start()
    {
        cameraParent.position = camRedKingTowerPos;
        cameraParent.eulerAngles = camRedKingTowerAngles;
        skipButton.gameObject.SetActive(false);
        clearDataButton.gameObject.SetActive(false);

        //Fade out panel
        float t = 0;
        float fadeTime = 2f;
        Color startColor = fadePanel.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            float factor = t / fadeTime;
            fadePanel.color = Color.Lerp(startColor, endColor, factor);
            yield return null;
        }

        skipButton.gameObject.SetActive(true);
        clearDataButton.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);


        redKingAnimator.SetTrigger("Talk");
        yield return new WaitForSeconds(3f);


        cameraParent.position = camBlueKingTowerPos;
        cameraParent.eulerAngles = camBlueKingTowerAngles;
        blueKingAnimator.SetTrigger("Talk");
        yield return new WaitForSeconds(3f);


        cameraParent.position = camRedKingTowerPos;
        cameraParent.eulerAngles = camRedKingTowerAngles;
        redKingAnimator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(CRMoveHeadsman());
        StartCoroutine(CRMoveRichard());
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(CRMoveBrand());
        yield return new WaitForSeconds(1f);


        cameraParent.position = camBlueKingTowerPos;
        cameraParent.eulerAngles = camBlueKingTowerAngles;
        blueKingAnimator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(CRMoveDrakert());
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(CRMoveGolem());
        yield return new WaitForSeconds(1f);

        t = 0;
        Vector3 startMainCamAngle = Camera.main.transform.localEulerAngles;
        Vector3 endMainCamAngle = new Vector3(35f, 0f, 0);
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            float factor = t / fadeTime;
            cameraParent.position = Vector3.Lerp(camBlueKingTowerPos, camBattlePos, factor);
            cameraParent.eulerAngles = Vector3.Lerp(camBlueKingTowerAngles, camBattleAngles, factor);
            Camera.main.transform.localEulerAngles = Vector3.Lerp(startMainCamAngle, endMainCamAngle, factor);
            yield return null;
        }

        yield return new WaitForSeconds(3f);
        t = 0;
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            float factor = t / fadeTime;
            cameraParent.position = Vector3.Lerp(camBattlePos, camEndBattlePos, factor);
            yield return null;
        }


        yield return new WaitForSeconds(5f);
        if (skipButton.gameObject.activeSelf)
        {
            skipButton.gameObject.SetActive(false);
            clearDataButton.gameObject.SetActive(false);
            t = 0;
            while (t < fadeTime)
            {
                t += Time.deltaTime;
                float factor = t / fadeTime;
                fadePanel.color = Color.Lerp(endColor, startColor, factor);
                yield return null;
            }
            SceneManager.LoadScene("Home");
        }
    }


    public void OnClickSkipButton()
    {
        skipButton.gameObject.SetActive(false);
        SceneManager.LoadScene("Home");
    }

    public void OnClickClearDataButton()
    {
        PlayerPrefs.DeleteAll();
    }


    #region Brand Hero

    private IEnumerator CRMoveBrand()
    {
        brandSpawnEffect.gameObject.SetActive(true);
        brandSpawnEffect.Play();
        brandHero.gameObject.SetActive(true);
        brandAnimator.SetFloat("Speed", 0f);
        yield return new WaitForSeconds(6f);
        brandAnimator.SetFloat("Speed", 1f);

        Vector3 attackPosition = new Vector3(0f, 0f, -4f);

        //Move the Brand to the attack position
        float t = 0;
        while (t < 4f)
        {
            t += Time.deltaTime;
            float factor = t / 4f;
            brandHero.transform.position = Vector3.Lerp(brandSpawnEffect.transform.position, attackPosition, factor);
            yield return null;
        }
        brandAnimator.SetFloat("Speed", 0f);

        //Brand attack
        StartCoroutine(CRBrankAttack());
    }




    private IEnumerator CRBrankAttack()
    {
        //Get the particles and disable them
        ParticleSystem trailEffect = fireBurnEffect.GetChild(0).GetComponent<ParticleSystem>();
        ParticleSystem groundSlamEffect = fireBurnEffect.GetChild(1).GetComponent<ParticleSystem>();
        Light pointLight = fireBurnEffect.GetChild(2).GetComponent<Light>();
        trailEffect.gameObject.SetActive(false);
        groundSlamEffect.gameObject.SetActive(false);
        pointLight.gameObject.SetActive(false);
        chargeEffect.gameObject.SetActive(false);

        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            chargeEffect.gameObject.SetActive(true);
            chargeEffect.Play();
            brandAnimator.SetTrigger("Attack");
            yield return new WaitForSeconds(0.7f);
            chargeEffect.gameObject.SetActive(false);

            //Enable the trail effect
            fireBurnEffect.gameObject.SetActive(true);
            fireBurnEffect.transform.position = chargeEffect.transform.position;
            groundSlamEffect.gameObject.SetActive(false);
            pointLight.gameObject.SetActive(false);
            trailEffect.gameObject.SetActive(true);
            trailEffect.Play();

            //Move the fire burn effect
            List<Vector3> listPositions = new List<Vector3>();
            int movePoints = 40;
            Vector3 startPoint = fireBurnEffect.transform.position;
            Vector3 midPoint = Vector3.Lerp(startPoint, new Vector3(0f, 0f, 4f), 0.5f) + Vector3.up * 6;
            listPositions.Add(fireBurnEffect.transform.position);
            for (int i = 1; i <= movePoints; i++)
            {
                float t = i / (float)movePoints;
                listPositions.Add(CalculateQuadraticBezierPoint(t, startPoint, midPoint, new Vector3(0f, 0f, 4f)));
            }

            //Moving player to each point
            for (int i = 0; i < listPositions.Count; i++)
            {
                fireBurnEffect.transform.position = listPositions[i];
                yield return null;
            }

            trailEffect.gameObject.SetActive(false);
            groundSlamEffect.gameObject.SetActive(true);
            groundSlamEffect.Play();
            StartCoroutine(CRPlayLightEffect(pointLight));
            yield return new WaitForSeconds(2f);
            groundSlamEffect.gameObject.SetActive(false);
            fireBurnEffect.gameObject.SetActive(false);
        }
    }


    private IEnumerator CRPlayLightEffect(Light light)
    {
        light.gameObject.SetActive(true);
        float original = light.intensity;
        float intensity = light.intensity;
        while (intensity > 0f)
        {
            intensity = Mathf.Clamp(intensity - Time.deltaTime * 5f, 0f, original);
            light.intensity = intensity;
            yield return null;
        }
        light.intensity = original;
        light.gameObject.SetActive(false);
    }


    private Vector3 CalculateQuadraticBezierPoint(float t, Vector3 from, Vector3 middle, Vector3 to)
    {
        return Mathf.Pow((1 - t), 2) * from + 2 * (1 - t) * t * middle + Mathf.Pow(t, 2) * to;
    }


    #endregion


    #region Richard Hero

    private IEnumerator CRMoveRichard()
    {
        richardSpawnEffect.gameObject.SetActive(true);
        richardSpawnEffect.Play();
        richardHero.gameObject.SetActive(true);
        richardAnimator.SetFloat("Speed", 0f);
        yield return new WaitForSeconds(6f);
        richardAnimator.SetFloat("Speed", 1f);

        Vector3 attackPosition = new Vector3(-11f, 0f, -1f);
        Vector3 forwardDir = (attackPosition - richardHero.transform.position).normalized;

        //Move the Richard to the attack position
        float t = 0;
        while (t < 5f)
        {
            t += Time.deltaTime;
            float factor = t / 5f;
            richardHero.transform.position = Vector3.Lerp(richardSpawnEffect.transform.position, attackPosition, factor);
            richardHero.transform.forward = Vector3.Lerp(richardHero.transform.forward, forwardDir, 20f * Time.deltaTime);
            yield return null;
        }
        richardAnimator.SetFloat("Speed", 0f);
        StartCoroutine(CRRichardAttack());

        t = 0;
        Vector3 currentForward = richardHero.transform.forward;
        while (t < 0.5f)
        {
            t += Time.deltaTime;
            float factor = t / 0.5f;
            richardHero.transform.forward = Vector3.Lerp(currentForward, Vector3.forward, factor);
            yield return null;
        }

    }


    private IEnumerator CRRichardAttack()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            richardAnimator.SetTrigger("Attack");
        }
    }

    #endregion


    #region Headsman Hero

    private IEnumerator CRMoveHeadsman()
    {
        headsmanSpawnEffect.gameObject.SetActive(true);
        headsmanSpawnEffect.Play();
        headsmanHero.gameObject.SetActive(true);
        headsmanAnimator.SetFloat("Speed", 0f);
        yield return new WaitForSeconds(6f);
        headsmanAnimator.SetFloat("Speed", 1f);

        Vector3 attackPosition = new Vector3(11f, 0f, -1f);
        Vector3 forwardDir = (attackPosition - headsmanHero.transform.position).normalized;

        //Move the Headsman to the attack position
        float t = 0;
        while (t < 5f)
        {
            t += Time.deltaTime;
            float factor = t / 5f;
            headsmanHero.transform.position = Vector3.Lerp(headsmanSpawnEffect.transform.position, attackPosition, factor);
            headsmanHero.transform.forward = Vector3.Lerp(headsmanHero.transform.forward, forwardDir, 20f * Time.deltaTime);
            yield return null;
        }
        headsmanAnimator.SetFloat("Speed", 0f);
        StartCoroutine(CRHeadsmanAttack());


        t = 0;
        Vector3 currentForward = headsmanHero.transform.forward;
        while (t < 0.5f)
        {
            t += Time.deltaTime;
            float factor = t / 0.5f;
            headsmanHero.transform.forward = Vector3.Lerp(currentForward, Vector3.forward, factor);
            yield return null;
        }

    }


    private IEnumerator CRHeadsmanAttack()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            headsmanAnimator.SetTrigger("Attack");
        }
    }

    #endregion


    #region Golem Hero

    private IEnumerator CRMoveGolem()
    {
        golemSpawnEffect.gameObject.SetActive(true);
        golemSpawnEffect.Play();
        golemHero.gameObject.SetActive(true);
        golemAnimator.SetFloat("Speed", 0f);
        yield return new WaitForSeconds(3.5f);
        golemAnimator.speed = 0.7f;
        golemAnimator.SetFloat("Speed", 1f);

        Vector3 attackPosition = new Vector3(0f, 0f, 4f);

        //Move the Golem to the attack position
        float t = 0;
        while (t < 5f)
        {
            t += Time.deltaTime;
            float factor = t / 5f;
            golemHero.transform.position = Vector3.Lerp(golemSpawnEffect.transform.position, attackPosition, factor);
            yield return null;
        }
        golemAnimator.SetFloat("Speed", 0f);
        golemAnimator.speed = 1f;
        StartCoroutine(CRGolemAttack());

    }


    private IEnumerator CRGolemAttack()
    {
        while (true)
        {
            golemAnimator.SetTrigger("Attack");
            yield return new WaitForSeconds(0.4f);
            golemAttackEffect.gameObject.SetActive(true);
            golemAttackEffect.Play();

            yield return new WaitForSeconds(2f);
            golemAttackEffect.gameObject.SetActive(false);
        }
    }


    #endregion


    #region Drakert Hero
    private IEnumerator CRMoveDrakert()
    {
        drakertSpawnEffect01.gameObject.SetActive(true);
        drakertSpawnEffect01.Play();
        drakertHero01.gameObject.SetActive(true);
        drakertSpawnEffect02.gameObject.SetActive(true);
        drakertSpawnEffect02.Play();
        drakertHero02.gameObject.SetActive(true);
        drakertAnimator01.SetFloat("Speed", 0f);
        drakertAnimator02.SetFloat("Speed", 0f);
        yield return new WaitForSeconds(4f);
        drakertAnimator01.SetFloat("Speed", 1f);
        drakertAnimator02.SetFloat("Speed", 1f);

        Vector3 attackPos01 = new Vector3(11f, 0f, 1f);
        Vector3 attackPos02 = new Vector3(-11f, 0f, 1f);
        Vector3 forwardDir01 = (attackPos01 - drakertHero01.transform.position).normalized;
        Vector3 forwardDir02 = (attackPos02 - drakertHero02.transform.position).normalized;

        //Move the Drakert to the attack position
        float t = 0;
        while (t < 5f)
        {
            t += Time.deltaTime;
            float factor = t / 5f;
            drakertHero01.transform.position = Vector3.Lerp(drakertSpawnEffect01.transform.position, attackPos01, factor);
            drakertHero01.transform.forward = Vector3.Lerp(drakertHero01.transform.forward, forwardDir01, 20f * Time.deltaTime);
            drakertHero02.transform.position = Vector3.Lerp(drakertSpawnEffect02.transform.position, attackPos02, factor);
            drakertHero02.transform.forward = Vector3.Lerp(drakertHero02.transform.forward, forwardDir02, 20f * Time.deltaTime);
            yield return null;
        }
        drakertAnimator01.SetFloat("Speed", 0f);
        drakertAnimator02.SetFloat("Speed", 0f);
        StartCoroutine(CRDrakertAttack());


        t = 0;
        Vector3 currentForward01 = drakertHero01.transform.forward;
        Vector3 currentForward02 = drakertHero02.transform.forward;
        while (t < 0.5f)
        {
            t += Time.deltaTime;
            float factor = t / 0.5f;
            drakertHero01.transform.forward = Vector3.Lerp(currentForward01, -Vector3.forward, factor);
            drakertHero02.transform.forward = Vector3.Lerp(currentForward02, -Vector3.forward, factor);
            yield return null;
        }

    }


    private IEnumerator CRDrakertAttack()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            drakertAnimator01.SetTrigger("Attack");
            drakertAnimator02.SetTrigger("Attack");
        }
    }


    #endregion
}

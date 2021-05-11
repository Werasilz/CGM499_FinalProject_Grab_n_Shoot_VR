using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScriptHealthBar : MonoBehaviour
{
    Vector3 firstScale;
    public GameObject maskFill;

    public SpriteRenderer hpBar;

    public Gradient gradient;
    GradientColorKey[] colorKey;
    GradientAlphaKey[] alphaKey;

    float newScale;
    float newScaleRegen;

    public Image regenFill;

    void Start()
    {
        firstScale = maskFill.transform.localScale;
        SetGradient();
    }

    void Update()
    {
        SetTextColor();
        //Set scale of sprite mask
        newScale = firstScale.y * (PlayerManager.Instance.hpPlayer / GameManager.Instance.maxPlayerHealth);

        maskFill.transform.localScale = new Vector3(newScale, firstScale.y, firstScale.z);

        regenFill.fillAmount = 1 - (PlayerManager.Instance.timeToHealing / 10);
    }

    void SetGradient()
    {
        gradient = new Gradient();
        colorKey = new GradientColorKey[3];
        alphaKey = new GradientAlphaKey[1];

        colorKey[0].color = Color.red;
        colorKey[0].time = 0.0f;
        colorKey[1].color = Color.yellow;
        colorKey[1].time = 0.35f;
        colorKey[2].color = new Color(0.35f, 0.99f, 0.92f, 1f);
        colorKey[2].time = 0.7f;

        alphaKey[0].alpha = 1.0f;
        alphaKey[0].time = 0.0f;

        gradient.SetKeys(colorKey, alphaKey);
    }

    public void SetTextColor()
    {
        float value = newScale;
        hpBar.color = gradient.Evaluate(value);
    }
}

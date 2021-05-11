using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIGunController : MonoBehaviour
{
    [HideInInspector] public TextMeshProUGUI textCount;
    [HideInInspector] public Image crop;

    public Gradient gradient;
    GradientColorKey[] colorKey;
    GradientAlphaKey[] alphaKey;

    float tmp;
    Color color;
    [SerializeField] private GunController GunController;

    public bool SetScale;
    Vector3 scaleXYZ;

    void Awake()
    {
        textCount = GetComponentInChildren<TextMeshProUGUI>();
        crop = textCount.gameObject.GetComponentInChildren<Image>();
        GunController = GetComponentInParent<GunController>();
    }

    void Start()
    {

        scaleXYZ = transform.localScale;

        SetGradient();
        Calculate();
    }

    void Update()
    {
       // CheckTextScale();
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

    void Calculate()
    {
        tmp = 100 / 12;
    }

    public void SetTextColor()
    {
        float value;
        value = tmp * GunController.countBullet / 100;
        textCount.color = gradient.Evaluate(value);
        crop.color = gradient.Evaluate(value);
    }


    void CheckTextScale()
    {
        if (SetScale)
        {
            transform.localScale = new Vector3(
                transform.localScale.x + 0.02f * Time.deltaTime,
                transform.localScale.y + 0.02f * Time.deltaTime,
                transform.localScale.z + 0.02f * Time.deltaTime);

            if (scaleXYZ.x < transform.localScale.x / 1.2f)
            {
                transform.localScale = scaleXYZ;
                SetScale = false;
            }
        }
    }
}

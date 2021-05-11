using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PopUpDamage : MonoBehaviour
{
    private Image image;
    private TextMeshProUGUI text;
    private Color textColor;
    private Color imageColor;
    private Camera mainCamera;

    private bool popupType;
    private float disappearTime = 1f;
    private float fadeSpeed = 3f;

    public bool isImage;

    private int damage;

    void Awake()
    {
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();

        damage = Random.Range(150, 201);

        if (isImage)
        {
            image = GetComponentInChildren<Image>();
            imageColor = image.color;
        }
        else
        {
            text = GetComponentInChildren<TextMeshProUGUI>();
            textColor = text.color;
        }
    }

    private void Update()
    {
        StartPopup();
        SetDamageText();
        SetRotationToCamera();
    }

    void SetRotationToCamera()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.x, mainCamera.transform.eulerAngles.y, transform.rotation.z);
    }

    public void SetDamageText()
    {
        if (!isImage)
        {
            text.text = damage.ToString();
        }
    }

    public void StartPopup()
    {
        if (!isImage)
        {
            float moveY = 0.1f;
            transform.position += new Vector3(0, moveY) * Time.deltaTime;
            disappearTime -= Time.deltaTime;

            // Fade Color
            if (disappearTime < 0)
            {
                textColor.a -= fadeSpeed * Time.deltaTime;
                text.color = textColor;
            }

            // Destroy when color fade to 0
            if (textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            float moveY = 0.1f;
            transform.position += new Vector3(0, moveY) * Time.deltaTime;
            disappearTime -= Time.deltaTime;

            // Fade Color
            if (disappearTime < 0)
            {
                imageColor.a -= fadeSpeed * Time.deltaTime;
                image.color = imageColor;
            }

            // Destroy when color fade to 0
            if (imageColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}

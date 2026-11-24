using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class StatusEffectUI : MonoBehaviour
{
    [SerializeField] private Image displayImage;
    [SerializeField] private TMP_Text text;
    [SerializeField] private RectTransform selfTransform;

    public void DestorySelf()
    {
        Destroy(gameObject);
    }

    public void UpdateDisplay(Sprite sprite, int count)
    {
        displayImage.sprite = sprite;
        text.text = count+"";
    }

    public void SetLocation(float x, float y)
    {
        selfTransform.anchoredPosition = new Vector2(x, y);
    }

    public void SetPos(float x, float y)
    {
        selfTransform.anchoredPosition = new Vector2(x, y);
    }
}

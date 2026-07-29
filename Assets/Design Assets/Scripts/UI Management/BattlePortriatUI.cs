using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;


public class BattlePortriatUI : MonoBehaviour
{
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text magicText;
    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider magicBar;
    [SerializeField] private Image characterBackground;
    [SerializeField] private Image characterMain;
    [SerializeField] private Image selectionIndicate;

    [field: SerializeField] private List<KeySprite> decisionSprites;

    public void SetupDisplay(PlayerData playerData)
    {
        //setupDisplay
        characterMain.sprite = playerData.BaseInfo.GetSprite("neutral");
        characterBackground.sprite = playerData.BaseInfo.CharacterBackground;
        SetupSliders(playerData.Hp, playerData.MaxHp, playerData.Mp, playerData.MaxMp);
    }

    public void ChangeStatus(string state)
    {
        selectionIndicate.sprite = GetSprite(state);
    }

    private void SetupSliders(int hp, int maxhp, int mp, int maxmp)
    {
        healthText.text = hp.ToString() + "/" + maxhp.ToString();
        magicText.text = mp.ToString() + "/" + maxmp.ToString();

        healthBar.maxValue = maxhp;
        healthBar.value = hp;
        magicBar.maxValue = maxmp;
        magicBar.value = mp;
    }

    private Sprite GetSprite(string name)
    {
        foreach(KeySprite dd in decisionSprites)
        {
            if(name == dd.key)
            {
                return dd.sprite;
            }
        }
        return null;
    }

}

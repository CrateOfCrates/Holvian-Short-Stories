using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;


public class BattlePortriatUI : BattleBarsUI
{
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

    public void UpdateStatsDisplay(PlayerData playerData)
    {

        SetupSliders(playerData.Hp, playerData.MaxHp, playerData.Mp, playerData.MaxMp);
    }

    public void ChangeStatus(string state)
    {
        selectionIndicate.sprite = GetSprite(state);
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

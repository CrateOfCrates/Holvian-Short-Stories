using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;


public class BattlePortriatUI : BattlePortGenericUI
{
    [SerializeField] private Image characterBackground;
    [SerializeField] private Image characterMain;
    [SerializeField] private Image selectionIndicate;
    [SerializeField] private Image animationDisplay;

    [field: SerializeField] private List<KeySprite> decisionSprites;

    public void SetupDisplay(PlayerData playerData)
    {
        //setupDisplay
        characterMain.sprite = playerData.BaseInfo.GetSprite("neutral");
        characterBackground.sprite = playerData.BaseInfo.CharacterBackground;
        SetupSliders(playerData.Hp, playerData.MaxHp, playerData.Mp, playerData.MaxMp);
        animationDisplay.enabled = false;
    }
    
    public void UpdateStatsDisplay(PlayerData playerData)
    {
        SetupSliders(playerData.Hp, playerData.MaxHp, playerData.Mp, playerData.MaxMp);
        LoadStatusEffects(playerData.StatusEffects);
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

    public IEnumerator PlayAnimation(AnimationObject animation)
    {
        if(animation == null){yield break;}

        animationDisplay.enabled = true;

        float frameDuration = 1f / Mathf.Max(animation.Fps, 1f);

        for(int i = 0; i < animation.Frames; i++)
        {
            animationDisplay.sprite = animation.Sprites[i];
            yield return new WaitForSeconds(frameDuration);
        }

        animationDisplay.enabled = false;
    }

}

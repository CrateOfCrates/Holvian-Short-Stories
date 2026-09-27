using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;
using System.Collections;

public class SingleEnemyManager : MonoBehaviour
{

    [SerializeField] private SpriteRenderer enemySprite;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private BattleEnemyUI battleEnemyUI;
    [SerializeField] private SpriteRenderer enemySpriteAnimations;

    private BattleManager battleManager;
    private int index;

    public BattleEnemyUI BattleEnemyUI => battleEnemyUI;

    public void SetupLoad(EnemyData ed, BattleManager battleManager, int index)
    {
        enemySprite.sprite = ed.BaseInfo.GetSprite("neutral");
        this.battleManager = battleManager;
        this.index = index;

        //regenerate box collider
        boxCollider.size = enemySprite.sprite.bounds.size;
        boxCollider.offset = enemySprite.sprite.bounds.center;

        battleEnemyUI.SetupDisplay(ed);
    }

    public void ConnectUI(BattleEnemyUI battleEnemyUI)
    {
        this.battleEnemyUI = battleEnemyUI;
    }

    public void OnClick()
    {
        battleManager.SelectEnemyTarget(index);
    }

    public IEnumerator PlayAnimation(AnimationObject animation)
    {
        if(animation == null){yield break;}

        enemySpriteAnimations.enabled = true;

        float frameDuration = 1f / Mathf.Max(animation.Fps, 1f);

        for(int i = 0; i < animation.Frames; i++)
        {
            enemySpriteAnimations.sprite = animation.Sprites[i];
            yield return new WaitForSeconds(frameDuration);
        }

        enemySpriteAnimations.enabled = false;
    }

}

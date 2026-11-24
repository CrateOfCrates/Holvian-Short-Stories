using UnityEngine;

public class BattleEnemyUI : BattlePortGenericUI
{
    public void SetupDisplay(EnemyData enemyData)
    {
        SetupSliders(enemyData.Hp, enemyData.MaxHp, enemyData.Mp, enemyData.MaxMp);
        LoadStatusEffects(enemyData.StatusEffects);
    }
}

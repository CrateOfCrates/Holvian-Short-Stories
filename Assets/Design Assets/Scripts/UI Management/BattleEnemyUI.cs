using UnityEngine;

public class BattleEnemyUI : BattleBarsUI
{
    public void SetupDisplay(EnemyData enemyData)
    {
        SetupSliders(enemyData.Hp, enemyData.MaxHp, enemyData.Mp, enemyData.MaxMp);
    }
}

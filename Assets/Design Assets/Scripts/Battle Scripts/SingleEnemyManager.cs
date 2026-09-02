using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SingleEnemyManager : MonoBehaviour
{

    [SerializeField] private SpriteRenderer enemySprite;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private BattleEnemyUI battleEnemyUI;

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

    // public void OnMouseEnter()
    // {
    //     battleManager.ToggleEnemyDisplay(index, true);
    // }

    // public void OnMouseExit()
    // {
    //     battleManager.ToggleEnemyDisplay(index, false);
    // }

}

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SingleEnemyManager : MonoBehaviour
{

    [SerializeField] private SpriteRenderer enemySprite;
    [SerializeField] private BoxCollider2D boxCollider;

    private BattleManager battleManager;
    private int index;

    public void SetupLoad(Sprite sprite, BattleManager battleManager, int index)
    {
        enemySprite.sprite = sprite;
        this.battleManager = battleManager;
        this.index = index;

        //regenerate box collider
        boxCollider.size = enemySprite.sprite.bounds.size;
        boxCollider.offset = enemySprite.sprite.bounds.center;
    }

    public void OnClick()
    {
        battleManager.SelectEnemyTarget(index);
    }

}

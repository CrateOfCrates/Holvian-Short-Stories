using UnityEngine;

[CreateAssetMenu(menuName = "Combat/Status/Damage")]
public class DamageSEO : StatusEffectObject
{
    [SerializeField] protected int damagePerStack;
    [SerializeField] protected int bonusDamage;
    [SerializeField] protected StatusEffectPartType triggerType;

    public int DamagePerStack => damagePerStack;
    public int BonusDamage => bonusDamage;
    public StatusEffectPartType TriggerType => triggerType;
}
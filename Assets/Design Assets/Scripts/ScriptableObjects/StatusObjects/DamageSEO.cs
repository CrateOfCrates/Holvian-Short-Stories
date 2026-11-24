using UnityEngine;

[CreateAssetMenu(menuName = "Combat/Status/Damage")]
public class DamageSEO : StatusEffectObject
{
    [SerializeField] protected double damagePerStack;
    [SerializeField] protected int bonusDamage;
    [SerializeField] protected StatusEffectPartType triggerType;
    [SerializeField] protected AnimationObject tickAnimation;

    public double DamagePerStack => damagePerStack;
    public int BonusDamage => bonusDamage;
    public StatusEffectPartType TriggerType => triggerType;
    public AnimationObject TickAnimation => tickAnimation;
}
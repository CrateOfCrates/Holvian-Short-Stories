using UnityEngine;

[CreateAssetMenu(menuName = "Combat/Status/StatChange")]
public class StatChangeSEO : StatusEffectObject
{
    //CHANGE LATER ISNT GOOD SYSTEM
    [SerializeField] protected StatChangeType affectedStat;
    [SerializeField] protected double mult;
    [SerializeField] protected bool scaleWithStacks;

    public StatChangeType AffectedStat => affectedStat;
    public double Mult => mult;
    public bool ScaleWithStacks => scaleWithStacks;
    public override StatusEffectType GetStatusEffectType(){return StatusEffectType.statChange;}
}

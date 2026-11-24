using UnityEngine;

[CreateAssetMenu(menuName = "Combat/Status/StatChange")]
public class StatChangeSEO : StatusEffectObject
{
    //CHANGE LATER ISNT GOOD SYSTEM
    [SerializeField] protected StatChangeType affectedStat;
    [SerializeField] protected double multPerStack;

    public StatChangeType AffectedStat => affectedStat;
    public double MultPerStack => multPerStack;
    public override StatusEffectType GetStatusEffectType(){return StatusEffectType.statChange;}
}

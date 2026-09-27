using UnityEngine;

[CreateAssetMenu(menuName = "Combat/Status/StatChange")]
public class StatChangeSEO : StatusEffectObject
{
    //CHANGE LATER ISNT GOOD SYSTEM
    [SerializeField] protected string affectedStat;
    [SerializeField] protected int mult;
}

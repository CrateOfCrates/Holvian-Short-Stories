using UnityEngine;
using UnityEngine.UI;

public abstract class EntityObject : ScriptableObject
{
    [field: SerializeField] private string entityName;
    [SerializeField] private int maxhp;
    [SerializeField] private int maxmp;

    public string EntityName => entityName;
    public int MaxHp => maxhp;
    public int MaxMp => maxmp;
}

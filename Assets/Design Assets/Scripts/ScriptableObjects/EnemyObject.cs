using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Combat/Enemy")]
public class EnemyObject : EntityObject
{
    [SerializeField] private int actionCount;

    public int ActionCount => actionCount;
}

using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Combat/Moves")]
public class MoveObject : ScriptableObject
{
    [SerializeField] private string moveName;

    //[SerializeReference, SubclassSelector]
    [SerializeField] private List<TokenObject> baseTokens = new List<TokenObject>();
    [SerializeField] int mpCost;
    [SerializeField] Target moveTarget;

    public string MoveName => moveName;
    public int MpCost => mpCost;
    public List<TokenObject> BaseTokens => baseTokens;
    public Target MoveTarget => moveTarget;
    

}

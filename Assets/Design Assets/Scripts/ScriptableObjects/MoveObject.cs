using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Moves")]
public class MoveObject : ScriptableObject
{
    [SerializeField] private string moveName;

    //[SerializeReference, SubclassSelector]
    [SerializeField] private List<TokenObject> baseTokens = new List<TokenObject>();
    [SerializeField] int mpCost;

    public string MoveName => moveName;
    public int MpCost => mpCost;
    public List<TokenObject> BaseTokens => baseTokens;
    

}

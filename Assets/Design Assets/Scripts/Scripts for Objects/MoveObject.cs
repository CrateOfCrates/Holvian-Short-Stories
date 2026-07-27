using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Moves")]
public class MoveObject : ScriptableObject
{
    [field: SerializeField] private string moveName;

    [SerializeReference, SubclassSelector]
    [field: SerializeField] private List<TokenObject> tokens = new List<TokenObject>();

    public string MoveName => moveName;
    public List<TokenObject> Tokens => tokens;

}

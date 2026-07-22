using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Moves")]
public class MoveObject : ScriptableObject
{
    public string moveName;
    [SerializeField] public List<TokenObject> tokens;

}

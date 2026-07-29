using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(menuName = "Player Character")]
public class PlayerObject : EntityObject
{
    //Start
    [field: SerializeField] private Sprite characterBackground;

    public Sprite CharacterBackground => characterBackground;
    

}

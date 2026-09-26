using System;
using UnityEngine;

public static class DamageCalc
{
    public static int CalculateDamage(EntityData user, EntityData target, TokenData token)
    {
        //calc for damage
        double moveDamage = token.Potency * Math.Sqrt(user.Attack);
        double finalDamage = moveDamage - target.Defense;

        return (int)finalDamage;
        
    }

    
}

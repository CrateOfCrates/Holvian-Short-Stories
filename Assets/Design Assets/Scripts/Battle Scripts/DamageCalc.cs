using System;
using Unity.VisualScripting;
using UnityEngine;

public static class DamageCalc
{
    public static int CalculateDamage(EntityData user, EntityData target, TokenData token)
    {
        //calc for damage
        double moveDamage = token.Potency * user.Attack * 0.02;

        //offensive calcs
        foreach(var (key, value) in user.StatusEffects)
        {
            if(key.GetStatusEffectType() != StatusEffectType.statChange){continue;}

            StatChangeSEO temp = (StatChangeSEO)key;

            if(temp.AffectedStat != StatChangeType.damageChangeMultAdd){continue;}

            //makes sure it's a damage change ADD
            double multiplier = (value.Count * temp.MultPerStack) + 1;
            moveDamage = moveDamage * multiplier;
        }
        
        //defensive calcs
        double finalDamage = moveDamage - target.Armor*0.02;
        finalDamage = finalDamage * (1.0 - target.Defense / (100.0 + Math.Abs(target.Defense)));
        

        //A move must do at least 0.05 * it's final damage
        if(Math.Floor(moveDamage * 0.05) >= Math.Floor(finalDamage))
        {
            //Debug.Log($"Damage Floor Hit! {Math.Floor(moveDamage * 0.01)} compared to final damage of {Math.Floor(finalDamage)}");
            return (int)Math.Floor(moveDamage * 0.01);
        } else
        {
            //Debug.Log($"Hit for final damage of {Math.Floor(finalDamage)}");
            return (int)Math.Floor(finalDamage);
        }

        
        
    }

    
}

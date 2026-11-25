using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

[System.Serializable]
public class TokenObject
{
    [SerializeField] private Sprite display;
    [SerializeField] Target tokenTarget;
    [SerializeField] AnimationObject animationPlayedTarget;

    [SerializeField] private string onUseText;
    [SerializeField] private string tokenDescription;

    [SerializeReference] [SubclassSelector] List<TokenParts> tokenParts;

    public Sprite Display => display;
    public Target TokenTarget => tokenTarget;
    public AnimationObject AnimationPlayedTarget => animationPlayedTarget;
    public List<TokenParts> TokenParts => tokenParts;
    public string TokenDescription => tokenDescription;

    public string GetTextDisplay(string userName)
    {
        if(onUseText == "")
        {
            return "";
        }
        else {
            string s = userName + " " + onUseText;
            return s;
        }
    }
    
}

[System.Serializable]
public abstract class TokenParts
{
    [SerializeField] protected float delay = 0.0f;
    [SerializeField] bool showAnimation = false;

    protected IEnumerator Animate(EntityData user, EntityData target, List<PlayerData> allPlayerData, List<EnemyData> allEnemyData)
    {
        if(!showAnimation){yield break;}
        
        TokenData token = user.GetSelectedToken();
        if(user.GetCurrentTarget().TargetTeam == WhichTeam.player)
        {
            //play animation
            if(target is PlayerData pd && token.AnimationPlayedTarget != null){
                IEnumerator ct = pd.Portriat.PlayAnimation(token.AnimationPlayedTarget);
                yield return ct;
            }
        } else
        {
            //play animation
            if(target is EnemyData ed && token.AnimationPlayedTarget != null){
                IEnumerator ct = ed.SingleEnemyManager.PlayAnimation(token.AnimationPlayedTarget);
                yield return ct;
            }
        }

        foreach(PlayerData pd in allPlayerData){pd.UpdateDisplay();}
        foreach(EnemyData ed in allEnemyData){ed.UpdateDisplay();}


        yield return new WaitForSeconds(delay);
    }

    public virtual IEnumerator Execute(EntityData user, EntityData target, List<PlayerData> pd, List<EnemyData> ed)
    {  
        yield return Animate(user, target, pd, ed);
    }
}

[System.Serializable]
public class DamageTarget : TokenParts
{
    [SerializeField] private int basePower; 

    public override IEnumerator Execute(EntityData user, EntityData target, List<PlayerData> pd, List<EnemyData> ed)
    {
        target.TakeDamage(DamageCalc.CalculateDamage(user, target, basePower));
        yield return Animate(user, target, pd, ed);
    }
}

[System.Serializable]
public class ApplyDamageStatus : TokenParts
{
    [SerializeReference] private DamageSEO type;
    [SerializeField] private int statusPotency;

    public override IEnumerator Execute(EntityData user, EntityData target, List<PlayerData> pd, List<EnemyData> ed)
    {
        target.ApplyStatus(type, statusPotency);
        yield return Animate(user, target, pd, ed);
    }
}

[System.Serializable]
public class ApplyStatChange : TokenParts
{
    [SerializeReference] private StatChangeSEO type;
    [SerializeField] private int statusPotency;

    public override IEnumerator Execute(EntityData user, EntityData target, List<PlayerData> pd, List<EnemyData> ed)
    {
        target.ApplyStatus(type, statusPotency);
        yield return Animate(user, target, pd, ed);
    }
}


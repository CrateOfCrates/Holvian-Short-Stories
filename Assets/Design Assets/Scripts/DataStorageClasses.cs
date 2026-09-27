using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement; 
using System;
using TMPro;
using System.Collections;
using Unity.VisualScripting;
using System.IO;


[Serializable]
public struct KeySprite
    {
        public string key;
        public Sprite sprite;
    }

[Serializable]
public class Targeting
{
    [SerializeField] private int whichTarget;
    [SerializeField] private WhichTeam targetTeam;
    [SerializeField] private int moveUsed;
    [SerializeField] private int tokenProgress;

    public int WhichTarget => whichTarget;
    public WhichTeam TargetTeam => targetTeam;
    public int MoveUsed => moveUsed;
    public int TokenProgress => tokenProgress;

    public Targeting(){
        moveUsed = -1;
        whichTarget = -1;
        targetTeam = WhichTeam.undec;
        tokenProgress = 0;
    }

    // public Targeting(int moveUsed, int whichTarget, WhichTeam targetTeam, bool needsTargeting)
    // {
    //     this.moveUsed = moveUsed;
    //     this.whichTarget = whichTarget;
    //     this.targetTeam = targetTeam;
    //     this.needsTargeting = needsTargeting;
    // }

    public void AdvanceNextToken()
    {
        tokenProgress++;
    }

    public void SetTarget(int moveUsed, int whichTarget, WhichTeam targetTeam)
    {
        this.moveUsed = moveUsed;
        this.whichTarget = whichTarget;
        this.targetTeam = targetTeam;
    }

    public void SetTarget(int whichTarget, WhichTeam targetTeam)
    {
        this.whichTarget = whichTarget;
        this.targetTeam = targetTeam;
    }

    public void SetTarget(int moveUsed)
    {
        this.moveUsed = moveUsed;
    }

    public void ClearTarget()
    {
        moveUsed = -1;
        whichTarget = -1;
        targetTeam = WhichTeam.undec;
        tokenProgress = 0;
    }

}

[System.Serializable]
public class EntityData
{
    [SerializeField] protected int hp;
    [SerializeField] protected int mp;
    [SerializeField] protected int maxHp;
    [SerializeField] protected int maxMp;

    [SerializeField] protected int speed;
    [SerializeField] protected int attack;
    [SerializeField] protected int defense;

    [SerializeField] protected List<MoveData> activeMoves;
    [SerializeField] protected List<Targeting> targets = new List<Targeting>();

    protected Dictionary<StatusEffectObject, StatusEffectInstance> statusEffects = new Dictionary<StatusEffectObject, StatusEffectInstance>();
    
    public int Hp => hp;
    public int Mp => mp;
    public int MaxHp => maxHp;
    public int MaxMp => maxMp;

    public int Speed => speed;
    public int Attack => attack;
    public int Defense => defense;

    public List<MoveData> ActiveMoves => activeMoves;
    public List<Targeting> Targets => targets;

    public Targeting GetCurrentTarget(){return targets[0];}
    public MoveData GetCurrentMove(){
        //Debug.Log($"[GETCURRENTMOVE] Player Object ID: {System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(this)} | AM Count: {targets[0].MoveUsed}");
        return activeMoves[targets[0].MoveUsed];
    }
    public int GetTokenProgress(){return targets[0].TokenProgress;}
    public TokenData GetSelectedToken(){return activeMoves[targets[0].MoveUsed].ActiveTokens[targets[0].TokenProgress];}
    public TokenData GetSelectedToken(int i){return activeMoves[targets[i].MoveUsed].ActiveTokens[targets[i].TokenProgress];}

    public void AdvanceNextToken(){
        targets[0].AdvanceNextToken();

        //check if the move has finished executing or is on last turn
        if(targets[0].TokenProgress >= activeMoves[targets[0].MoveUsed].GetTokenCount())
        {
            //Debug.Log("targeting cleared");
            targets[0].ClearTarget();
        }
    }

    public void ApplyStatus(StatusEffectObject type, int potency)
    {
        Debug.Log("Applying " + type + " with Potency " + potency);

        if(statusEffects.Count == 0)
        {
            AddStatus(type, potency);
            return;
        }

        if (statusEffects.ContainsKey(type))
        {
            //modify
            statusEffects[type].ChangeCount(potency);

        } else
        {
            AddStatus(type, potency);
        }
    }

    private void AddStatus(StatusEffectObject type, int potency)
    {
        if(type is DamageSEO damageSEO)
        {
            DamageSEI temp = new DamageSEI();
            temp.SetBaseInfo(damageSEO);
            statusEffects[type] = temp;
        }

        statusEffects[type].SetAppliedTo(this);
        statusEffects[type].ChangeCount(potency);


    }

    public void OnTurnEndTick()
    {
        foreach(var (key, value) in statusEffects)
        {
            //Debug.Log("STATUS EFFECT: " + value);
            value.OnTurnEnd();
        }
    }

    protected void LoadStats(EntityObject EO)
    {
        hp = EO.BaseMaxHp;
        mp = EO.BaseMaxMp;
        maxHp = EO.BaseMaxHp;
        maxMp = EO.BaseMaxMp;
        speed = EO.BaseSpeed;
        attack = EO.BaseAttack;
        defense = EO.BaseDefense;
        LoadMoves(EO.StartingMoves);
        // Debug.Log(currentTargets.Count);
        //Debug.LogWarning(activeMoves.Count);
    }

    private void LoadMoves(List<MoveObject> moves){

        activeMoves = new List<MoveData>();

        for(int i = 0; i < moves.Count; i++)
        {
            activeMoves.Add(new());
            activeMoves[i].LoadData(moves[i]);
        }
    }

    public virtual WhichTeam GetTeam(){return WhichTeam.undec;}
    public virtual string GetName(){return "N/A";}

    public void TakeDamage(int damage)
    {
        hp = hp - damage;
        //Debug.Log("Take Dagage. Hp:" + hp);
    }

}

[System.Serializable]
public class PlayerData : EntityData
{
    [SerializeField] private PlayerObject baseInfo;
    [SerializeField] private bool exists;

    public PlayerObject BaseInfo => baseInfo;
    public bool Exists => exists;

    public void SetExists(bool b){exists = b;}

    public void LoadData(PlayerObject playerObject)
    {
        LoadStats(playerObject);
        baseInfo = playerObject;
    }

    public bool GetNeedTargeting(){
        if(exists && (targets[0].MoveUsed >= 0)){
            return GetCurrentMove().NeedAim(GetTokenProgress());
        } else {
            return false;
        }
    }

    public void SetupForCombat()
    {
        targets.Clear();
        targets.Add(new Targeting());
        //Debug.Log("WEEE   " + hp);
        //Debug.Log($"[SETUPFORCOMBAT] Player Object ID: {System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(this)} | Targets Count: {targets.Count}");
        //Debug.Log($"[SETUPFORCOMBAT] Player Object ID: {System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(this)} | AM Count: {activeMoves.Count}");
    }
    public override WhichTeam GetTeam(){return WhichTeam.player;}
    public override string GetName(){return baseInfo.EntityName;}
}

[System.Serializable]
public class EnemyData : EntityData
{
    [SerializeField] private EnemyObject baseInfo;
    [SerializeField] private SingleEnemyManager singleEnemyManager;

    public EnemyObject BaseInfo => baseInfo;
    public SingleEnemyManager SingleEnemyManager => singleEnemyManager;

    public void LoadData(EnemyObject enemyObject, SingleEnemyManager singleEnemyManager)
    {
        LoadStats(enemyObject);
        baseInfo = enemyObject;
        this.singleEnemyManager = singleEnemyManager;

        targets.Clear();
        targets.Add(new Targeting());
        //Debug.Log($"[LOAD_STATS] Player Object ID: {System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(this)} | Targets Count: {targets.Count}");
    }
    public override WhichTeam GetTeam(){return WhichTeam.enemy;}
    public override string GetName(){return baseInfo.EntityName;}

    public void UpdateDisplay()
    {
        singleEnemyManager.BattleEnemyUI.SetupDisplay(this);
    }
}

[System.Serializable]
public class MoveData
{
    [SerializeField] private List<TokenData> activeTokens;
    [SerializeField] private int mpCost;
    [SerializeField] private MoveObject baseInfo;

    public List<TokenData> ActiveTokens => activeTokens;
    public int MpCost => mpCost;
    public MoveObject BaseInfo => baseInfo;
    public string MoveName => baseInfo.MoveName;

    public void LoadData(MoveObject moveObject)
    {
        activeTokens = new List<TokenData>();
        for(int i = 0; i < moveObject.BaseTokens.Count; i++)
        {
            TokenData temp = new TokenData(moveObject.BaseTokens[i]);
            activeTokens.Add(temp);
        }
        mpCost = moveObject.MpCost;
        baseInfo = moveObject;
    }

    //Returns if the current token needs aim or not
    public bool NeedAim(){return activeTokens[0].BaseInfo.NeedAim();}
    public bool NeedAim(int index){return activeTokens[index].BaseInfo.NeedAim();}

    public int GetTokenCount(){return activeTokens.Count;}

}

[System.Serializable]
public class TokenData
{
    [SerializeField] private TokenObject baseInfo;
    [SerializeField] private int potency;

    public TokenData(TokenObject baseInfo)
    {
        this.baseInfo = baseInfo;
        this.potency = baseInfo.Potency;
    }

    public TokenObject BaseInfo => baseInfo;
    public int Potency => potency;
    public AnimationObject AnimationPlayedTarget => baseInfo.AnimationPlayedTarget;

    public void SingleAttackToken(EntityData user, EntityData target)
    {
        target.TakeDamage(DamageCalc.CalculateDamage(user, target, this));
        AdressAfterModifiers(user, target);
    }

    private void AdressAfterModifiers(EntityData user, EntityData target)
    {
        foreach(TokenParts tp in baseInfo.TokenParts){

            if(tp.TriggerTime != TriggerTime.after){ continue;}

            tp.Execute(user, target);
        }
    }

//     public void ExecuteToken(EntityData user, WhichTeam wt)
//     {
//         //means there is no target
//         switch (baseInfo.TokenType)
//         {
//             case TokenType.singleAttack:
//                 //activate damagecalc and apply to target
//                 target.TakeDamage(DamageCalc.CalculateDamage(user, target, this));
//                 break;
            
//             case TokenType.delay:
//                 break;
//         }
//     }
}

[System.Serializable]
public class StatusEffectInstance
{
    protected int count;
    protected EntityData appliedTo;

    public virtual void OnApply() { }
    public virtual void OnTurnEnd() { Debug.Log("NEEDS POLYMORPHISM");}
    public virtual void OnTokenEnd() { }
    public virtual void OnExpire() { }

    public void SetAppliedTo(EntityData apt)
    {
        appliedTo = apt;
    }

    protected void Decay(StatusEffectObject baseInfo)
    {
        if(baseInfo.DecayRate == StatusEffectDecayRate.fractional)
        {
            double removeMult = (1.0 - baseInfo.DecayCount)/100;
            ChangeCount(count * removeMult);

        } else if (baseInfo.DecayRate == StatusEffectDecayRate.subtractional)
        {
            ChangeCount(baseInfo.DecayCount * -1);
        }

        Debug.LogWarning("ADD CHECK FOR NEGATIVES AND ZEROES");
    }

    public void ChangeCount(double change)
    {
        count = (int)Math.Floor(count + change);
        Debug.Log("Count Changed to: " + count);
    }

}

public class DamageSEI : StatusEffectInstance
{
    protected DamageSEO baseInfo;

    public DamageSEO BaseInfo => baseInfo;

    public void SetBaseInfo(DamageSEO damageSEO)
    {
        Debug.Log("ATTACHED BASE INFO");
        baseInfo = damageSEO;
    }

    public override void OnTurnEnd()
    {
        Debug.Log("OVERRIDE VERSIOM");

        Debug.Log(baseInfo);

        if(baseInfo.TriggerType == StatusEffectPartType.endOfTurn)
        {
            Debug.Log("DEAL DAMAGE FROM EFFECT. TARGET BEING: " + appliedTo.GetName() + "FOR " + baseInfo.DamagePerStack * count);
            appliedTo.TakeDamage(baseInfo.DamagePerStack * count);

        }

        if(baseInfo.DecayType == StatusEffectPartType.endOfTurn)
        {
            Decay(baseInfo);
        }
    }


}




using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement; 
using System;
using TMPro;
using Unity.VisualScripting;
using System.IO;
using NUnit.Framework;


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
    [SerializeField] protected string name;

    [SerializeField] protected int hp;
    [SerializeField] protected int mp;
    [SerializeField] protected int maxHp;
    [SerializeField] protected int maxMp;

    [SerializeField] protected int speed;
    [SerializeField] protected int attack;
    [SerializeField] protected int armor;
    [SerializeField] protected int defense;

    [SerializeField] private int level;

    [SerializeField] protected List<MoveData> activeMoves;
    [SerializeField] protected List<Targeting> targets = new List<Targeting>();

    protected Dictionary<StatusEffectObject, StatusEffectInstance> statusEffects = new Dictionary<StatusEffectObject, StatusEffectInstance>();
    
    public string Name => name;

    public int Hp => hp;
    public int Mp => mp;
    public int MaxHp => maxHp;
    public int MaxMp => maxMp;

    public int Speed => speed;
    public int Attack => attack;
    public int Defense => defense;
    public int Armor => armor;
    public int Level => level;

    public List<MoveData> ActiveMoves => activeMoves;
    public List<Targeting> Targets => targets;

    public Dictionary<StatusEffectObject, StatusEffectInstance> StatusEffects => statusEffects;

    public Targeting GetCurrentTarget(){return targets[0];}
    public MoveData GetCurrentMove(){return activeMoves[targets[0].MoveUsed];}
    public TokenData GetSelectedToken(){return activeMoves[targets[0].MoveUsed].ActiveTokens[targets[0].TokenProgress];}
    public int GetTokenProgress(){return targets[0].TokenProgress;}

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

    public IEnumerator OnTurnEndTick()
    {
        foreach(var (key, value) in statusEffects)
        {
            //Debug.Log("STATUS EFFECT: " + value);
            IEnumerator valueanim = value.OnTurnEnd();
            yield return valueanim;
        }
        yield return new WaitForSeconds(0f);
    }

    protected void LoadStats(EntityObject EO)
    {
        name = EO.EntityName;
        hp = EO.BaseMaxHp;
        mp = EO.BaseMaxMp;
        maxHp = EO.BaseMaxHp;
        maxMp = EO.BaseMaxMp;
        speed = EO.BaseSpeed;
        attack = EO.BaseAttack;
        defense = EO.BaseDefense;
        armor = EO.BaseArmor;
        level = EO.StartingLevel;
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
    public virtual void UpdateDisplay(){return;}
    //public virtual string GetName(){return name;}

    public void TakeDamage(int damage)
    {
        hp = hp - damage;
        //Debug.Log("Take Dagage. Hp:" + hp);
        if(hp < 0)
        {
            hp = 0;
            Debug.LogWarning("NEED TO IMPLEMENT DEATH HERE");
        }
    }

}

[System.Serializable]
public class PlayerData : EntityData
{
    [SerializeField] private PlayerObject baseInfo;
    [SerializeField] private bool exists;
    [SerializeField] private BattlePortriatUI portriat;

    public PlayerObject BaseInfo => baseInfo;
    public bool Exists => exists;
    public BattlePortriatUI Portriat => portriat;

    public void SetExists(bool b){exists = b;}

    public void LoadData(PlayerObject playerObject)
    {
        LoadStats(playerObject);
        baseInfo = playerObject;
    }

    public void SetPortriat(BattlePortriatUI bp)
    {
        portriat = bp;
    }

    public bool GetNeedTargeting(){
        if(exists && (targets[0].MoveUsed >= 0)){
            return GetCurrentMove().NeedAim;
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
    //public override string GetName(){return baseInfo.EntityName;}

    public override void UpdateDisplay()
    {
        portriat.UpdateStatsDisplay(this);
    }
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
    //public override string GetName(){return baseInfo.EntityName;}

    public override void UpdateDisplay()
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
    public string MoveDescription => baseInfo.MoveDescription;
    //Returns if the current token needs aim or not
    public bool NeedAim => CombatStuffHelper.NeedAim(baseInfo.MoveTarget);

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
    public string TokenDescription => baseInfo.TokenDescription;
    public TokenType TokenType => baseInfo.TokenType;

    public string GetTextDisplay(string name) => baseInfo.GetTextDisplay(name);

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
    protected Sprite displaySprite;

    public int Count => count;
    public Sprite DisplaySprite => displaySprite;

    public virtual void OnApply() { }
    public virtual IEnumerator OnTurnEnd() { Debug.Log("NEEDS POLYMORPHISM"); yield return new WaitForSeconds(0f);}
    public virtual void OnTokenEnd() { }
    public virtual void OnExpire() { }

    public void SetAppliedTo(EntityData apt)
    {
        appliedTo = apt;
        appliedTo.UpdateDisplay();
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
        if(count <= 0)
        {
            count = 0;
        }

        appliedTo.UpdateDisplay();
    }

    public void ChangeCount(double change)
    {
        count = (int)Math.Floor(count + change);
        //Debug.Log("Count Changed to: " + count);
    }

    protected IEnumerator playAnim(EntityData guy, AnimationObject animationObject)
    {
        if(guy is PlayerData pd){
            IEnumerator ct = pd.Portriat.PlayAnimation(animationObject);
            yield return ct;

        }

        if(guy is EnemyData ed){
            IEnumerator ct = ed.SingleEnemyManager.PlayAnimation(animationObject);
            //Debug.Log("ANIMATION SHOULD BE HERE: " + ct);
            yield return ct;
        }
    }

}

public class DamageSEI : StatusEffectInstance
{
    protected DamageSEO baseInfo;

    public DamageSEO BaseInfo => baseInfo;

    public void SetBaseInfo(DamageSEO damageSEO)
    {
        //Debug.Log("ATTACHED BASE INFO");
        baseInfo = damageSEO;
        displaySprite = baseInfo.DisplaySprite;
    }

    public override IEnumerator OnTurnEnd()
    {
        Debug.Log("OVERRIDE VERSIOM");

        //Debug.Log(baseInfo);

        if(baseInfo.TriggerType == StatusEffectPartType.endOfTurn)
        {
            Debug.Log("DEAL DAMAGE FROM EFFECT. TARGET BEING: " + appliedTo.Name + "FOR " + (int)Math.Floor(baseInfo.DamagePerStack * count));
            IEnumerator anim = playAnim(appliedTo, baseInfo.TickAnimation);
            yield return anim;
            int damage = (int)Math.Floor(baseInfo.DamagePerStack * count);
            
            appliedTo.TakeDamage(damage);

        }

        if(baseInfo.DecayType == StatusEffectPartType.endOfTurn)
        {
            Decay(baseInfo);
        }

        yield return new WaitForSeconds(0f);
    }


}




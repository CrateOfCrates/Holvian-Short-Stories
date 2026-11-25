using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private BattleState battleState = BattleState.startBattle;
    [SerializeField] private BattleUniversalUI battleUniversalUI;

    [SerializeField] private Transform parentTransformWorld;
    [SerializeField] private Transform parentTransformUI;

    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject enemyPrefabUI;

    [SerializeField] private List<PlayerData> allPlayerData;
    [SerializeField] private List<EnemyData> allEnemyData;

    //Currently selected player
    [SerializeField] private int selectedPlayer;
    //Determines how much down moves is scrolled
    [SerializeField] private int scrollMoves;

    private int turn;

    private const int numberOfMovesPerRow = 4;

    public void InitializeBattle()
    {
        battleState = BattleState.startBattle;

        turn=0;

        //Clear Lists
        allEnemyData.Clear();

        //Get Player Data
        allPlayerData = new List<PlayerData>(DataManager.Instance.AllPlayerData);
        //Set up the copies for combat
        foreach(PlayerData pd in allPlayerData)
        {
            pd.SetupForCombat();
        }

        selectedPlayer = -1;
        scrollMoves = 0;

        //Get and load Enemy Data
        EncounterObject enemyEncounter = DataManager.Instance.Encounter;
        //Debug.Log(enemyDataToLoad.Count);
        allEnemyData = new List<EnemyData>(enemyEncounter.Enemies.Count); 
        for(int i = 0; i < enemyEncounter.Enemies.Count; i++)
        {
            //Spawn Enemy Display
            SingleEnemyManager temp = SpawnEnemy(enemyEncounter.GetXLocation(i), enemyEncounter.GetYLocation(i), enemyEncounter.GetXLocationUI(i), enemyEncounter.GetYLocationUI(i));

            //load data into EnemyObject
            allEnemyData.Add(new());
            allEnemyData[i].LoadData(enemyEncounter.GetEnemyObject(i), temp);

            //Load Enemy General Sprite
            allEnemyData[i].SingleEnemyManager.SetupLoad(allEnemyData[i], this, i);
        }
        //UI
        battleUniversalUI.InitializeCombatUI(allPlayerData, allEnemyData, enemyEncounter.BackgroundForEncounter);

        BattleStateTurns(BattleState.playerSelect);
        
    }

    //Changes between BattleStates (like playerturn, enemyturn, etc)
    private void BattleStateTurns(BattleState newState)
    {
        battleState = newState;

        switch (battleState)
        {
            case BattleState.playerSelect:
                PlayerTurn();
                break;

            case BattleState.enemySelect:
                EnemyTurn();
                break;

            case BattleState.execute:
                //setup turn UI
                BattleText.Instance.Clear();
                battleUniversalUI.ToggleUI(false);
                List<EntityData> turnOrder = GetTurnOrder();
                StartCoroutine(TurnPart(turnOrder));
                break;
        }
    }

    //Player Turn and Interaction
    private void PlayerTurn()
    {
        BattleText.Instance.Clear();
        battleUniversalUI.ToggleUI(true);
        selectedPlayer = -1;
        scrollMoves = 0;
        UpdateUI();

        // foreach(PlayerData pd in allPlayerData)
        // {
        //     if(pd.GetNeedTargeting()){
        //         Debug.LogWarning(pd.GetName());
        //     }
        // }
    }

    //Code for selecting a move
    public void SelectMove(int index)
    {
        //Set pre-variables
        Targeting temp = allPlayerData[selectedPlayer].GetCurrentTarget();
        int selectedMove = scrollMoves*numberOfMovesPerRow + index;

        //Means the button can only be clicked if its in the right battleState
        if(!(battleState == BattleState.playerSelect || battleState == BattleState.selectMove)){return;}
        //Prevents move selection when no player is selected
        if(selectedPlayer == -1){return;}
        //Prevents move selection when a move is currently in execution
        if(temp.MoveUsed != -1 && temp.TokenProgress != 0 && temp.MoveUsed != selectedMove){return;}

        //Set the move in
        temp.SetTarget(selectedMove);

        //check if the move needs targeting 
        if (allPlayerData[selectedPlayer].GetNeedTargeting())
        {
            //yes aim is needed and aim is not present
            battleState = BattleState.selectMove;
        } else
        {
            if(allPlayerData[selectedPlayer].GetCurrentMove().BaseInfo.MoveTarget == Target.self)
            {
                Debug.Log("target is self, so sets targeting to self");
                allPlayerData[selectedPlayer].GetCurrentTarget().SetTarget(selectedPlayer, WhichTeam.player);
            }
        }

        UpdateUI();
    }

    //Code for choosing an enemy as a target
    public void SelectEnemyTarget(int index)
    {
        if(battleState != BattleState.selectMove)
        {
            return;
        }

        //Debug.Log("Target is the enemy!");

        //mark enemy as target
        allPlayerData[selectedPlayer].GetCurrentTarget().SetTarget(index, WhichTeam.enemy);
        //move selected
        battleState = BattleState.playerSelect;
        UpdateUI();

    }

    //For changing players and selecting them for positive moves
    public void SelectPlayer(int index)
    {
        //Ensure valid BattleState
        if(!(battleState == BattleState.playerSelect || battleState == BattleState.selectMove)){return;}
        //Ensure the current player exists
        if(!allPlayerData[index].Exists){return;}


        if(battleState == BattleState.playerSelect)
        {
            //Player Select: The current player is selected.
            selectedPlayer = index;
            scrollMoves = 0;
        } else
        {
            //Set the current target as the clicked-on-player
            allPlayerData[selectedPlayer].GetCurrentTarget().SetTarget(index, WhichTeam.player);
            battleState = BattleState.playerSelect;
        }

        //UI
        UpdateUI();
    }

    public void MoveScroll(string direction)
    {
        if(battleState != BattleState.playerSelect)
        {
            return;
        }

        if(selectedPlayer == -1)
        {
            //no player is selected
            return;
        }

        //check if possible
        if(direction == "up" && scrollMoves > 0)
        {
            scrollMoves--;

        } else if (direction == "down" && (scrollMoves+1) * numberOfMovesPerRow <= allPlayerData[selectedPlayer].ActiveMoves.Count)
        {
            scrollMoves++;
        } 
        
        //Update UI
        UpdateUI();
    }

    public void FinishPlayerTurn()
    {
        if(battleState != BattleState.playerSelect)
        {
            return;
        }
        //player turn is done
        selectedPlayer = -1;
        scrollMoves = 0;
        battleUniversalUI.CustomizePlayerPortriatBorders(ref allPlayerData, selectedPlayer, battleState);
        //Pass to enemy
        BattleStateTurns(BattleState.enemySelect);
    }

    private void UpdateUI()
    {
        battleUniversalUI.CustomizePlayerPortriatBorders(ref allPlayerData, selectedPlayer, battleState);
        if(selectedPlayer < 0)
        {
            battleUniversalUI.loadMoves(false);
        } else
        {
            battleUniversalUI.StatusButton(scrollMoves, allPlayerData[selectedPlayer].ActiveMoves.Count);
            battleUniversalUI.LoadMoveBorders(scrollMoves, allPlayerData[selectedPlayer], battleState);
        }

    }

    //ENEMY TURN

    private void EnemyTurn()
    {
        Debug.Log("THE ENEMY TURN");
        //Fill with random targeting
        foreach(EnemyData ed in allEnemyData)
        {
            if(ed.GetCurrentTarget().TokenProgress != 0){continue;}
            ed.SetCurrentTarget(ed.GetTargetingBasedOnAi(turn, allPlayerData, allEnemyData));
        }
        BattleStateTurns(BattleState.execute);
    }

    //EXECUTE

    IEnumerator TurnPart(List<EntityData> turnOrder)
    {
        foreach(EntityData e in turnOrder)
        {
            //get the move being used and advance to next token
            if(e.GetCurrentTarget().MoveUsed == -1)
            {
                Debug.Log("Current Target Doesnt Exist");
                continue;
            }

            Debug.Log(e.Name + " is acting! Move Used: " + e.GetCurrentMove().BaseInfo.MoveName + " with token progress: " + e.GetTokenProgress());
            TokenData token = e.GetSelectedToken();

            EntityData target;

            //Main Animation Runner
            if(e.GetCurrentTarget().TargetTeam == WhichTeam.player)
            {
                //target is on the player team
                target = allPlayerData[e.GetCurrentTarget().WhichTarget];
            } else
            {
                target = allEnemyData[e.GetCurrentTarget().WhichTarget];
            }

            BattleText.Instance.AddMessage(token.GetTextDisplay(e.Name));

            yield return token.ExecuteToken(e, target, allPlayerData, allEnemyData);

            //update UI
            foreach(PlayerData pd in allPlayerData){pd.UpdateDisplay();}
            foreach(EnemyData ed in allEnemyData){ed.UpdateDisplay();}

            //update the token and check for exits
            e.AdvanceNextToken();

            //Debug.Log("Activate Delay");
            yield return new WaitForSeconds(1.0f);
            
        }

        //turn is finished
        StartCoroutine(FinalizeTurn(turnOrder));
    }

    private IEnumerator FinalizeTurn(List<EntityData> turnOrder)
    {
        foreach(EntityData e in turnOrder)
        {
            IEnumerator ed = e.OnTurnEndTick();
            yield return ed;
        }
        //UI UPDATES
        foreach(PlayerData pd in allPlayerData){pd.UpdateDisplay();}
        foreach(EnemyData ed in allEnemyData){ed.UpdateDisplay();}

        Debug.Log("TURN FINISHED");
        turn++;

        BattleStateTurns(BattleState.playerSelect);
    }

    private List<EntityData> GetTurnOrder()
    {
        List<PlayerData> trimmedPlayers = new List<PlayerData>(allPlayerData);

        for(int i = 0; i < trimmedPlayers.Count; i++)
        {
            if(!trimmedPlayers[i].Exists)
            {
                trimmedPlayers.RemoveAt(i);
                i--;
            }
        }
        List<EntityData> combinedTargeting = trimmedPlayers.Concat<EntityData>(allEnemyData).ToList();

        combinedTargeting = combinedTargeting.OrderByDescending(x => x.Speed).ToList();

        // foreach(EntityData t in combinedTargeting)
        // {
        //     Debug.Log(t.GetName());
        // }

        return combinedTargeting;
    }

    private SingleEnemyManager SpawnEnemy(double xLocation, double yLocation, double xUi, double yUi)
    {
        Vector3 spawnPosition = new Vector3((float)xLocation, (float)yLocation, 0);
        Quaternion spawnRotation = Quaternion.identity;

        GameObject clone = Instantiate(enemyPrefab, spawnPosition, spawnRotation, parentTransformWorld);
        SingleEnemyManager singleEnemyManager = clone.GetComponent<SingleEnemyManager>();

        //singleEnemyManager.ChangeSprite();
        spawnPosition = new Vector3((float)xUi, (float)yUi, 0);
        GameObject cloneUI = Instantiate(enemyPrefabUI, spawnPosition, spawnRotation, parentTransformUI);
        BattleEnemyUI beUI = cloneUI.GetComponent<BattleEnemyUI>();

        singleEnemyManager.ConnectUI(beUI);

        return singleEnemyManager;

    }

    private void HoverScanner()
    {
        GameObject topScan = CombatStuffHelper.HoverScan();
        if(topScan == null){return;}

        

        if(battleState == BattleState.playerSelect || battleState == BattleState.selectMove){
            if(topScan.name.Substring(0, 5) == "Token")
            {
                GameObject moveGameObject = topScan.transform.parent.gameObject.transform.parent.gameObject;

                //These rely on the NAMES of the MOVES AND TOKENS! DO NOT CHANGE THEM OR BAD THINGS WILL HAPPEN!
                int scanTargetMove = int.Parse(""+moveGameObject.name[14]);
                int scanTargetToken = int.Parse(""+topScan.name[7]);
                
                PlayerData scanPlayer = allPlayerData[selectedPlayer];
                MoveData scanMove = scanPlayer.ActiveMoves[scanTargetMove];

                //if doesnt exist, stop
                if(scanMove.ActiveTokens.Count <= scanTargetToken){return;}

                TokenData targetToken = scanMove.ActiveTokens[scanTargetToken];
                //Debug.Log(targetToken.TokenDescription);
                BattleText.Instance.SetMessage(targetToken.TokenDescription);
            } else if(topScan.name.Substring(0, 4) == "Move")
            {
                GameObject moveGameObject = topScan.transform.parent.gameObject;
                int scanTargetMove = int.Parse(""+moveGameObject.name[14]);

                PlayerData scanPlayer = allPlayerData[selectedPlayer];
                MoveData scanMove = scanPlayer.ActiveMoves[scanTargetMove];

                if(scanMove == null){return;}

                BattleText.Instance.SetMessage(scanMove.MoveDescription);

            } else
            {
                BattleText.Instance.SetMessage("");
            }
        }
    }

    //modify text for hovering over
    void Update()
    {
        HoverScanner();
    }

    void Start()
    {
        InitializeBattle();
    }

    
}

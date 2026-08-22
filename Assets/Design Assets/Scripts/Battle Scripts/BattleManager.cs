using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour
{
    public enum BattleState {startBattle, playerSelect, selectMove, enemySelect, execute, victory, defeat};
    [SerializeField] private BattleState battleState = BattleState.startBattle;
    [SerializeField] private BattleUniversalUI battleUniversalUI;

    [SerializeField] private Transform parentTransform;
    [SerializeField] private GameObject enemyPrefab;

    [SerializeField] private List<PlayerData> allPlayerData;
    [SerializeField] private List<EnemyData> allEnemyData;

    //Currently selected player
    [SerializeField] private int selectedPlayer;
    //Determines how much down moves is scrolled
    [SerializeField] private int scrollMoves;

    public void InitializeBattle()
    {
        battleState = BattleState.startBattle;

        //Clear Lists
        allEnemyData.Clear();

        //Get Player Data
        allPlayerData = DataManager.Instance.AllPlayerData;

        selectedPlayer = -1;
        scrollMoves = 0;

        //Get and load Enemy Data
        EncounterObject enemyEncounter = DataManager.Instance.Encounter;
        //Debug.Log(enemyDataToLoad.Count);
        allEnemyData = new List<EnemyData>(enemyEncounter.Enemies.Count); 
        for(int i = 0; i < enemyEncounter.Enemies.Count; i++)
        {
            //Spawn Enemy Display
            SingleEnemyManager temp = SpawnEnemy(enemyEncounter.GetXLocation(i), enemyEncounter.GetYLocation(i));

            //load data into EnemyObject
            allEnemyData.Add(new());
            allEnemyData[i].LoadData(enemyEncounter.GetEnemyObject(i), temp);

            //Load Enemy General Sprite
            allEnemyData[i].SingleEnemyManager.SetupLoad(allEnemyData[i].BaseInfo.GetSprite("neutral"), this, i);
        }
        //UI
        battleUniversalUI.InitializeCombatUI(allPlayerData, allEnemyData);

        BattleStateTurns(BattleState.playerSelect);
        
    }

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
                ExecuteTurn();
                break;
        }
    }

    //Player Turn and Interaction

    private void PlayerTurn()
    {
        return;
    }

    public void SelectMove(int index)
    {
        //if selected during another selectmove, cancel.
        if(!(battleState == BattleState.playerSelect || battleState == BattleState.selectMove))
        {
            return;
        }
        if(selectedPlayer == -1)
        {
            //no player is selected
            return;
        }

        //A move can't be selected if the move is currently in execution.

        int selectedMove = scrollMoves*4 + index;
        Debug.Log("SelectMove in BattleManager: " + selectedMove);

        //Set the target and determines if aim is needed.
        if (allPlayerData[selectedPlayer].ActiveMoves[selectedMove].BaseInfo.NeedAim)
        {
            allPlayerData[selectedPlayer].CurrentTarget.SetTarget(selectedMove, true);
            battleState = BattleState.selectMove;
        } else
        {
            allPlayerData[selectedPlayer].CurrentTarget.SetTarget(selectedMove, true);
        }

        UpdateUI();
    }

    public void SelectEnemyTarget(int index)
    {
        if(battleState != BattleState.selectMove)
        {
            return;
        }

        Debug.Log("Target is the enemy!");

        //mark enemy as target
        allPlayerData[selectedPlayer].CurrentTarget.SetTarget(index, WhichTeam.enemy);
        //move selected
        battleState = BattleState.playerSelect;
        UpdateUI();

    }

    public void SelectPlayer(int index)
    {
        if(!(battleState == BattleState.playerSelect || battleState == BattleState.selectMove))
        {
            return;
        }

        //ensure no out of bounds occur
        if(!allPlayerData[index].Exists)
        {
            return;
        }

        if(battleState == BattleState.playerSelect)
        {
            //Player Select
            selectedPlayer = index;
            scrollMoves = 0;
        } else
        {
            //Target Player
            allPlayerData[selectedPlayer].CurrentTarget.SetTarget(index, WhichTeam.player);
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

        } else if (direction == "down" && (scrollMoves+1) * 4 <= allPlayerData[selectedPlayer].ActiveMoves.Count)
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
        CustomizePlayerPortriatBorders();
        //Pass to enemy
        BattleStateTurns(BattleState.enemySelect);
    }

    private void CustomizePlayerPortriatBorders()
    {
        //loop through
        for(int i = 0; i < allPlayerData.Count; i++)
        {
            if(allPlayerData[i].CurrentTarget.MoveUsed != -1)
            {
                //move has been selected by this player
                if(selectedPlayer == i)
                {
                    battleUniversalUI.LoadPlayerPortriatBorder(i, "selected_confirmed");
                } else
                {
                    battleUniversalUI.LoadPlayerPortriatBorder(i, "confirmed");
                }
            } else
            {
                if(selectedPlayer == i)
                {
                    //both
                    battleUniversalUI.LoadPlayerPortriatBorder(i, "selected");
                } else
                {
                    battleUniversalUI.LoadPlayerPortriatBorder(i, "neutral");
                }
            }
        }
    }

    private void UpdateUI()
    {
        CustomizePlayerPortriatBorders();
        battleUniversalUI.StatusButton(scrollMoves, allPlayerData[selectedPlayer].ActiveMoves.Count);
        battleUniversalUI.LoadMoveBorders(scrollMoves, allPlayerData[selectedPlayer].CurrentTarget);
        battleUniversalUI.loadMoves(scrollMoves, allPlayerData[selectedPlayer]);
    }

    //ENEMY TURN

    private void EnemyTurn()
    {
        Debug.Log("THE ENEMY TURN");
        //Fill with random targeting
        BattleStateTurns(BattleState.execute);

    }

    //EXECUTE

    private void ExecuteTurn()
    {
        //Determine Turn Order
        List<EntityData> turnOrder = GetTurnOrder();

        

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

    private SingleEnemyManager SpawnEnemy(int xLocation, int yLocation)
    {
        Vector3 spawnPosition = new Vector3(xLocation, yLocation, 0);
        Quaternion spawnRotation = Quaternion.identity;

        GameObject clone = Instantiate(enemyPrefab, spawnPosition, spawnRotation, parentTransform);
        SingleEnemyManager singleEnemyManager = clone.GetComponent<SingleEnemyManager>();

        //singleEnemyManager.ChangeSprite();

        return singleEnemyManager;

    }

    void Start()
    {
        InitializeBattle();
    }
}

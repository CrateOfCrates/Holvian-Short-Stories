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

    //Select moves based on move index in the list
    [SerializeField] private List<Targeting> selectedMovesByPlayers;
    [SerializeField] private List<Targeting> selectedMovesByEmemies;
    //Currently selected player
    [SerializeField] private int selectedPlayer;
    //Determines how much down moves is scrolled
    [SerializeField] private int scrollMoves;

    public void InitializeBattle()
    {
        battleState = BattleState.startBattle;

        //Clear Lists
        allEnemyData.Clear();
        selectedMovesByPlayers.Clear();
        selectedMovesByEmemies.Clear();

        //Get Player Data
        allPlayerData = DataManager.Instance.AllPlayerData;

        //Load up list
        for(int i = 0; i < allPlayerData.Count; i++)
        {
            //if exists
            if (allPlayerData[i].Exists)
            {
                Targeting t = new Targeting(-1, i, WhichTeam.player, -1, WhichTeam.undec);
                selectedMovesByPlayers.Add(t);
            } else
            {
                selectedMovesByPlayers.Add(new());
            }
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
            SingleEnemyManager temp = SpawnEnemy(enemyEncounter.GetXLocation(i), enemyEncounter.GetYLocation(i));

            //load data into EnemyObject
            allEnemyData.Add(new());
            allEnemyData[i].LoadData(enemyEncounter.GetEnemyObject(i), temp);

            //Load Enemy General Sprite
            allEnemyData[i].SingleEnemyManager.SetupLoad(allEnemyData[i].BaseInfo.GetSprite("neutral"), this, i);

            //Set up enemy move loading
            for(int j = 0; j < allEnemyData[i].BaseInfo.ActionCount; j++)
            {
                Targeting t = new Targeting(-1, i, WhichTeam.enemy, -1, WhichTeam.undec);
                selectedMovesByEmemies.Add(t);
                
            }
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

        //Determine if changing to aim mode is necessary

        int selectedMove = scrollMoves*4 + index;
        Debug.Log("SelectMove in BattleManager: " + selectedMove);
        
        selectedMovesByPlayers[selectedPlayer].SetUser(selectedMove, selectedPlayer, WhichTeam.player);
         //Means if targeting is not necessary, make targeting -2.
        selectedMovesByPlayers[selectedPlayer].SetTarget(-2);

        if(allPlayerData[selectedPlayer].ActiveMoves[selectedMove].BaseInfo.NeedAim)
        {
            //Means is necessary. So make -1
            selectedMovesByPlayers[selectedPlayer].SetTarget(-1);
            battleState = BattleState.selectMove;
            
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
        selectedMovesByPlayers[selectedPlayer].SetTarget(index, WhichTeam.enemy);
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
            selectedMovesByPlayers[selectedPlayer].SetTarget(index, WhichTeam.player);
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
        for(int i = 0; i < selectedMovesByPlayers.Count; i++)
        {
            if(selectedMovesByPlayers[i].MoveUsed != -1)
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
        battleUniversalUI.LoadMoveBorders(scrollMoves, selectedMovesByPlayers[selectedPlayer]);
        battleUniversalUI.loadMoves(scrollMoves, allPlayerData[selectedPlayer]);
    }

    //ENEMY TURN

    private void EnemyTurn()
    {
        Debug.Log("enemy turn");
        //Fill with random targeting
        BattleStateTurns(BattleState.execute);

    }

    //EXECUTE

    private void ExecuteTurn()
    {
        //Determine Turn Order
        List<Targeting> turnOrder = GetTurnOrder();
    }

    private List<Targeting> GetTurnOrder()
    {
        List<Targeting> trimmedPlayers = new List<Targeting>(selectedMovesByPlayers);

        for(int i = 0; i < trimmedPlayers.Count; i++)
        {
            if(trimmedPlayers[i].WhichUser == -1)
            {
                trimmedPlayers.RemoveAt(i);
                i--;
            }
        }

        var combinedTargeting = trimmedPlayers
            .Concat(selectedMovesByEmemies)
            .ToList();

        combinedTargeting = combinedTargeting.OrderByDescending(x => (x.UserTeam == WhichTeam.player) ? allPlayerData[x.WhichUser].Speed : allEnemyData[x.WhichUser].Speed).ToList();

        foreach(Targeting t in combinedTargeting)
        {
            Debug.Log(t.WhichUser + " " + t.UserTeam);
        }

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

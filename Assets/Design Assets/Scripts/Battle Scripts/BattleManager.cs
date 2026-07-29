using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour
{
    public enum BattleState {startBattle, playerSelect, enemySelect, execute, victory, defeat};
    [SerializeField] private BattleState battleState = BattleState.startBattle;
    [SerializeField] private BattleUniversalUI battleUniversalUI;

    [SerializeField] private Transform parentTransform;
    [SerializeField] private GameObject enemyPrefab;

    //Player Selection

    //Select moves based on move index in the list
    [SerializeField] private List<int> selectedMovesByPlayers;
    //Currently selected player
    [SerializeField] private int selectedPlayer;
    //Determines how much down moves is scrolled
    [SerializeField] private int scrollMoves;

    public void InitializeBattle()
    {
        battleState = BattleState.startBattle;
        //Get Player Data
        IReadOnlyList<PlayerData> allPlayerData = DataManager.Instance.AllPlayerData;

        //Load up list
        selectedMovesByPlayers.Clear();
        selectedMovesByPlayers = Enumerable.Repeat(-1, 6).ToList();
        selectedPlayer = 0;
        scrollMoves = 0;

        //Get and load Enemy Data
        EncounterObject enemyEncounter = DataManager.Instance.Encounter;
        //Debug.Log(enemyDataToLoad.Count);
        List<EnemyData> allEnemyData = new List<EnemyData>(enemyEncounter.Enemies.Count); 
        for(int i = 0; i < enemyEncounter.Enemies.Count; i++)
        {
            //Spawn Enemy Display
            SingleEnemyManager temp = SpawnEnemy(enemyEncounter.GetXLocation(i), enemyEncounter.GetYLocation(i));

            //load data into EnemyObject
            allEnemyData.Add(new());
            allEnemyData[i].LoadData(enemyEncounter.GetEnemyObject(i), temp);

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
        }
    }

    //Player Turn and Interaction

    private void PlayerTurn()
    {
        return;
    }

    public void SelectMove(int index)
    {
        if(battleState != BattleState.playerSelect)
        {
            return;
        }

        int selectedMove = scrollMoves*4 + index;
        Debug.Log("SelectMove in BattleManager: " + selectedMove);
        
        selectedMovesByPlayers[selectedPlayer] = selectedMove;
        battleUniversalUI.LoadMoveBorders(scrollMoves, selectedMove);

        //player portriat
        CustomizePlayerPortriatBorders();
    }

    public void SelectPlayer(int index)
    {
        if(battleState != BattleState.playerSelect)
        {
            return;
        }

        selectedPlayer = index;

        //player portriat customize
        CustomizePlayerPortriatBorders();
    }

    private void CustomizePlayerPortriatBorders()
    {
        //loop through
        for(int i = 0; i < selectedMovesByPlayers.Count; i++)
        {
            if(selectedMovesByPlayers[i] != -1)
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

    private void EnemyTurn()
    {
        return;
    }

    private SingleEnemyManager SpawnEnemy(int xLocation, int yLocation)
    {
        Vector3 spawnPosition = new Vector3(xLocation, yLocation, 0);
        Quaternion spawnRotation = Quaternion.identity;

        GameObject clone = Instantiate(enemyPrefab, spawnPosition, spawnRotation, parentTransform);
        SingleEnemyManager singleEnemyManager = clone.GetComponent<SingleEnemyManager>();

        singleEnemyManager.ChangeSprite();

        return singleEnemyManager;

    }

    void Start()
    {
        InitializeBattle();
    }
}

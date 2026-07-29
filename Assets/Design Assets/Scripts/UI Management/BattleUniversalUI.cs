using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class BattleUniversalUI : MonoBehaviour
{
    [SerializeField] private List<BattleButtonUI> moveUI;
    [SerializeField] private List<BattlePortriatUI> portriatUI;

    public void InitializeCombatUI(IReadOnlyList<PlayerData> players, List<EnemyData> enemies)
    {
        for(int i = 0; i < players.Count; i++)
        {
            portriatUI[i].SetupDisplay(players[i]);
        }

        //load first moves from first player and set first player as selected
        loadMoves(0, players[0]);
        portriatUI[0].ChangeStatus("selected");
    }

    //moveScroll dictates how much down the moves are scrolled
    //if moveScroll = 0, mean show moves 0-3 on the list
    //if moveScroll = 1, mean show moves 4-7
    
    public void loadMoves(int moveScroll, PlayerData whichPlayer)
    {
        foreach(BattleButtonUI battleButtonUI in moveUI)
        {
            battleButtonUI.Display(false);
        }
        for(int i = 0; i+(moveScroll*4) < whichPlayer.ActiveMoves.Count; i++)
        {
            moveUI[i].SetupDisplay(whichPlayer.ActiveMoves[i+(moveScroll*4)]);
        }
    }

    public void LoadMoveBorders(int moveScroll, int selectedMove)
    {
        foreach(BattleButtonUI battleButtonUI in moveUI)
        {
            battleButtonUI.ChangeStatus("neutral");
        }

        if(selectedMove - (moveScroll*4) > 3)
        {
            return;
        }
        //ok fine now
        moveUI[selectedMove-(moveScroll*4)].ChangeStatus("selected");
    }

    public void LoadPlayerPortriatBorder(int index, string change)
    {
        portriatUI[index].ChangeStatus(change);
    }

}

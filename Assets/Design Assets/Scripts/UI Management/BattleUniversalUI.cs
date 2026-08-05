using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class BattleUniversalUI : MonoBehaviour
{
    [SerializeField] private List<BattleButtonUI> moveUI;
    [SerializeField] private List<BattlePortriatUI> portriatUI;

    [SerializeField] private Image moveUpButton;
    [SerializeField] private Image moveDownButton;
    [field: SerializeField] private List<KeySprite> moveControlButtons;


    public void InitializeCombatUI(List<PlayerData> players, List<EnemyData> enemies)
    {
        for(int i = 0; i < players.Count; i++)
        {
            if(players[i].Exists)
            {
                portriatUI[i].SetupDisplay(players[i]);
            }
        }

        // loadMoves(0, players[0]);
        // portriatUI[0].ChangeStatus("selected");
        // StatusButton(0, players[0].ActiveMoves.Count);
    }

    //moveScroll dictates how much down the moves are scrolled
    //if moveScroll = 0, mean show moves 0-3 on the list
    //if moveScroll = 1, mean show moves 4-7

    //count = 5 -> 0 1 2 3 4
    
    public void loadMoves(int moveScroll, PlayerData whichPlayer)
    {
        foreach(BattleButtonUI battleButtonUI in moveUI)
        {
            battleButtonUI.Display(false);
        }
        //Debug.Log(moveScroll*4+1);
        for(int i = 0; i < 4; i++)
        {
            //Debug.Log(i);
            if(whichPlayer.ActiveMoves.Count <= i + moveScroll * 4)
            {
                break;
            }
            moveUI[i].SetupDisplay(whichPlayer.ActiveMoves[i+(moveScroll*4)]);
        }
    }

    public void LoadMoveBorders(int moveScroll, Targeting targeting)
    {
        foreach(BattleButtonUI battleButtonUI in moveUI)
        {
            battleButtonUI.ChangeStatus("neutral");
        }

        if(targeting.MoveUsed == -1 && targeting.WhichTarget == -1)
        {
            //none selected
            return;
        }

        if(targeting.MoveUsed - (moveScroll*4) < 0 || targeting.MoveUsed - (moveScroll*4) > 3)
        {
            //Will not be shown
            return;
        }
        //ok fine now
        if(targeting.WhichTarget == -1)
        {
            //means currently selecting a target
            moveUI[targeting.MoveUsed-(moveScroll*4)].ChangeStatus("choosetarget");
        } else
        {
            //means target alerady found
            moveUI[targeting.MoveUsed-(moveScroll*4)].ChangeStatus("selected");
        }
    }

    public void LoadPlayerPortriatBorder(int index, string change)
    {
        portriatUI[index].ChangeStatus(change);
    }

    public void StatusButton(int moveScroll, int count)
    {
        //Enable All
        moveUpButton.sprite = GetSprite("up_on");
        moveDownButton.sprite = GetSprite("down_on");

        if(moveScroll <= 0)
        {
            //disable
            moveUpButton.sprite = GetSprite("up_off");
            
        }
        if ((moveScroll+1) * 4 > count)
        {
            //disable
            moveDownButton.sprite = GetSprite("down_off");
        }
    }

    private Sprite GetSprite(string name)
    {
        foreach(KeySprite ep in moveControlButtons)
        {
            if(name == ep.key)
            {
                return ep.sprite;
            }
        }
        return null;
    }

}

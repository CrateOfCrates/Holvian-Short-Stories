using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;

public class BattleUniversalUI : MonoBehaviour
{
    [SerializeField] private List<BattleButtonUI> moveUI;
    [SerializeField] private List<BattlePortriatUI> portriatUI;

    [SerializeField] private Image moveUpButton;
    [SerializeField] private Image moveDownButton;

    [SerializeField] private Image displayBackground;
    [SerializeField] private Sprite select;
    [SerializeField] private Sprite execute;

    [SerializeField] private SpriteRenderer battleBackground;

    private const int numberOfMovesPerRow = 4;

    [field: SerializeField] private List<KeySprite> moveControlButtons;

    //Fully Set up the Combat UI
    public void InitializeCombatUI(List<PlayerData> players, List<EnemyData> enemies, Sprite bg)
    {
        for(int i = 0; i < players.Count; i++)
        {
            if(players[i].Exists)
            {
                portriatUI[i].SetupDisplay(players[i]);
                players[i].SetPortriat(portriatUI[i]);
            }
        }

        battleBackground.sprite = bg;
    }

    //moveScroll dictates how much down the moves are scrolled
    //if moveScroll = 0, mean show moves 0-3 on the list
    //if moveScroll = 1, mean show moves 4-7

    //count = 5 -> 0 1 2 3 4
    
    public void loadMoves(int moveScroll, PlayerData whichPlayer)
    {
        loadMoves(false);
        //Debug.Log(moveScroll*4+1);
        for(int i = 0; i < numberOfMovesPerRow; i++)
        {
            //Debug.Log(i);
            if(whichPlayer.ActiveMoves.Count <= i + moveScroll * numberOfMovesPerRow)
            {
                break;
            }
            moveUI[i].SetupDisplay(whichPlayer.ActiveMoves[i+(moveScroll*numberOfMovesPerRow)]);
        }
    }

    //Load the borders for moves and everything around them
    public void LoadMoveBorders(int moveScroll, PlayerData pd, BattleState battleState)
    {
        loadMoves(moveScroll, pd);

        Targeting targeting = pd.GetCurrentTarget();
        foreach(BattleButtonUI battleButtonUI in moveUI){battleButtonUI.ChangeStatusToBase();}

        //Conditions where the Specialized Border won't be shown
        if(targeting.MoveUsed == -1 && targeting.WhichTarget == -1){return;}
        if(targeting.MoveUsed - (moveScroll*numberOfMovesPerRow) < 0 || targeting.MoveUsed - (moveScroll*numberOfMovesPerRow) > 3){return;}

        if(battleState == BattleState.selectMove)
        {
            //means currently selecting a target
            moveUI[targeting.MoveUsed-(moveScroll*numberOfMovesPerRow)].ChangeStatus("choosetarget");
            return;
        }

        //below conditions indicate a selector is needed
        moveUI[targeting.MoveUsed-(moveScroll*numberOfMovesPerRow)].ChangeSelectedToken(targeting.TokenProgress);

        if(targeting.WhichTarget == -1 && pd.GetNeedTargeting())
        {
            //Means needs a target for the current token but has none
            Debug.Log("Means needs a target for the current token but has none");
            moveUI[targeting.MoveUsed-(moveScroll*numberOfMovesPerRow)].ChangeStatus("attention");
        } else {
            //Means everything should be fine
            moveUI[targeting.MoveUsed-(moveScroll*numberOfMovesPerRow)].ChangeStatus("executing");
        }

    }

    public void CustomizePlayerPortriatBorders(ref List<PlayerData> allPlayerData, int selectedPlayer, BattleState battleState)
    {
        //loop through
        for(int i = 0; i < allPlayerData.Count; i++)
        {
            //Skip if the border for the current player doesn't exist
            if (!allPlayerData[i].Exists){continue;}
            //Debug.Log(allPlayerData[i].GetName() + ": " + allPlayerData[i].GetCurrentTarget().MoveUsed + " OR " + allPlayerData[i].GetNeedTargeting());
            if(allPlayerData[i].GetCurrentTarget().MoveUsed != -1 && selectedPlayer == i && battleState == BattleState.selectMove)
            {
                //Means there IS A MOVE and it is the CURRENT PLAYER and MOVES ARE BEING AIMED
                LoadPlayerPortriatBorder(i, "active_selected");
                continue;
            }
            
            if(allPlayerData[i].GetCurrentTarget().MoveUsed == -1 || (allPlayerData[i].GetNeedTargeting() && allPlayerData[i].GetCurrentTarget().WhichTarget == -1))
            {
                //Means a move has NOT already been selected OR a move NEEDS targeting and DOESNT HAVE IT
                if(selectedPlayer == i)
                {
                    LoadPlayerPortriatBorder(i, "attention_selected");
                } else
                {
                    LoadPlayerPortriatBorder(i, "attention");
                }
            } else
            {
                //This means that a move has been selected AND targeting concerns have been resolved.
                if(selectedPlayer == i)
                {
                    //both, is selected and move is ready to choose
                    LoadPlayerPortriatBorder(i, "confirmed_selected");
                } else
                {
                    LoadPlayerPortriatBorder(i, "confirmed");
                }
            }
        }
    }

    public void loadMoves(bool b)
    {
        //hide the move buttons themselves
        foreach(BattleButtonUI battleButtonUI in moveUI)
        {
            battleButtonUI.Display(b);
        }
    }
    
    //turn ui on and off
    public void ToggleUI(bool b)
    {
        loadMoves(b);
        moveUpButton.enabled = b;
        moveDownButton.enabled = b;
        if(b)
        {
            displayBackground.sprite = select;
        } else
        {
            displayBackground.sprite = execute;
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
        if ((moveScroll+1) * numberOfMovesPerRow > count)
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

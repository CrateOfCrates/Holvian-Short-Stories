using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class BattleButtonUI : DisplayButtonUI
{

    [SerializeField] private TMP_Text nameText;
    [SerializeField] private List<Image> images;
    [SerializeField] private Image selectionIndicate;

    [SerializeField] private RectTransform tokenIndicateTransform;
    [SerializeField] private Image tokenIndicateImage;
    [SerializeField] private int startingInt;
    [SerializeField] private int leapInt;

    [field: SerializeField] private List<KeySprite> decisionSprites;

    private void UpdateTokenImages(List<TokenData> tokenList)
    {
        for(int i = 0; i < tokenList.Count; i++)
        {
            TokenData activeToken = tokenList[i];
            Image activeImage = images[i];

            activeImage.sprite = activeToken.BaseInfo.Display;
        }

        //set tokenselector
    }

    public void ChangeStatus(string state)
    {
        selectionIndicate.sprite = GetSprite(state);
    }

    public void ChangeStatusToBase()
    {
        selectionIndicate.sprite = GetSprite("neutral");
        tokenIndicateImage.enabled = false;
    }


    //Set the indication arrow to demonstrate which token is currenty about to be run / running
    public void ChangeSelectedToken(int tokenselector)
    {
        tokenIndicateImage.enabled = true;
        tokenIndicateTransform.anchoredPosition = new Vector2(tokenselector*leapInt+startingInt, tokenIndicateTransform.anchoredPosition.y);
    }

    //Set up the display
    public void SetupDisplay(MoveData move)
    {
        Display(true);
        nameText.text = move.MoveName;
        UpdateTokenImages(move.ActiveTokens);
    }

    private Sprite GetSprite(string name)
    {
        foreach(KeySprite dd in decisionSprites)
        {
            if(name == dd.key)
            {
                return dd.sprite;
            }
        }
        return null;
    }

}

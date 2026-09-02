using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class BattleButtonUI : MonoBehaviour
{

    [SerializeField] private TMP_Text nameText;
    [SerializeField] private List<Image> images;
    [SerializeField] private List<Image> tokenBorders;
    [SerializeField] private Image selectionIndicate;

    [field: SerializeField] private List<KeySprite> decisionSprites;

    private void UpdateTokenImages(List<TokenData> tokenList)
    {
        for(int i = 0; i < tokenList.Count; i++)
        {
            TokenData activeToken = tokenList[i];
            Image activeImage = images[i];

            activeImage.sprite = activeToken.BaseInfo.Display;
            tokenBorders[i].sprite = GetSprite("neutral");
        }

        //set tokenselector
    }

    public void ChangeStatus(string state)
    {
        selectionIndicate.sprite = GetSprite(state);
    }

    public void ChangeSelectedToken(int tokenselector, string state)
    {
        tokenBorders[tokenselector].sprite = GetSprite(state);
    }


    public void SetupDisplay(MoveData move)
    {
        Display(true);
        nameText.text = move.MoveName;
        UpdateTokenImages(move.ActiveTokens);
    }

    public void Display(bool status)
    {
        gameObject.SetActive(status);
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

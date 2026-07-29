using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class BattleButtonUI : MonoBehaviour
{

    [SerializeField] private TMP_Text nameText;
    [SerializeField] private List<Image> images;
    [SerializeField] private Image selectionIndicate;

    [field: SerializeField] private List<KeySprite> decisionSprites;

    private void UpdateTokenImages(List<TokenObject> tokenList)
    {
        for(int i = 0; i < tokenList.Count; i++)
        {
            TokenObject activeToken = tokenList[i];
            Image activeImage = images[i];

            activeImage.sprite = activeToken.Display;
        }
    }

    public void ChangeStatus(string state)
    {
        selectionIndicate.sprite = GetSprite(state);
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

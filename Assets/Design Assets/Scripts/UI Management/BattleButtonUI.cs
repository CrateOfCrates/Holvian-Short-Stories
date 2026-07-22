using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class BattleButtonUI : MonoBehaviour
{

    public TMP_Text nameText;
    public List<Image> images;

    public MoveObject TEMPmove;

    private void updateTokenImages(List<TokenObject> tokenList)
    {
        for(int i = 0; i < tokenList.Count; i++)
        {
            TokenObject activeToken = tokenList[i];
            Image activeImage = images[i];

            activeImage.sprite = activeToken.display;
        }
    }

    public void setupDisplay(MoveObject move)
    {
        nameText.text = move.moveName;
        updateTokenImages(move.tokens);
    }

    void Start()
    {
        setupDisplay(TEMPmove);
    }
}

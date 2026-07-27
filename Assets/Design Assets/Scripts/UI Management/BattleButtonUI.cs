using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class BattleButtonUI : MonoBehaviour
{

    [SerializeField] private TMP_Text nameText;
    [SerializeField] private List<Image> images;

    //TEMP
    [SerializeField] private MoveObject TEMPmove;



    private void UpdateTokenImages(List<TokenObject> tokenList)
    {
        for(int i = 0; i < tokenList.Count; i++)
        {
            TokenObject activeToken = tokenList[i];
            Image activeImage = images[i];

            activeImage.sprite = activeToken.Display;
        }
    }

    public void SetupDisplay(MoveObject move)
    {
        nameText.text = move.MoveName;
        UpdateTokenImages(move.Tokens);
    }

    void Start()
    {
        SetupDisplay(TEMPmove);
    }
}

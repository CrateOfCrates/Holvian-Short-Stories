using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattlePortriatUI : MonoBehaviour
{
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text magicText;
    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider magicBar;
    [SerializeField] private Image characterBackground;
    [SerializeField] private Image characterMain;

    //temp
    [SerializeField] private PlayerObject TEMPplayer;

    public void SetupDisplay(PlayerObject playerObject)
    {
        //setupDisplay
        characterMain.sprite = playerObject.GetSprite("neutral");
        characterBackground.sprite = playerObject.CharacterBackground;
        SetupSliders(playerObject.Hp, playerObject.MaxHp, playerObject.Mp, playerObject.MaxMp);
    }

    private void SetupSliders(int hp, int maxhp, int mp, int maxmp)
    {
        healthText.text = hp.ToString() + "/" + maxhp.ToString();
        magicText.text = mp.ToString() + "/" + maxmp.ToString();

        healthBar.maxValue = maxhp;
        healthBar.value = hp;
        magicBar.maxValue = maxmp;
        magicBar.value = mp;
    }


    void Start()
    {
        SetupDisplay(TEMPplayer);
    }
    
}

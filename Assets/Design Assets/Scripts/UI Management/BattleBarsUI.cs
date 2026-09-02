using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class BattleBarsUI : MonoBehaviour
{
    [SerializeField] protected TMP_Text healthText;
    [SerializeField] protected TMP_Text magicText;
    [SerializeField] protected Slider healthBar;
    [SerializeField] protected Slider magicBar;

    protected void SetupSliders(int hp, int maxhp, int mp, int maxmp)
    {
        healthText.text = hp.ToString() + "/" + maxhp.ToString();
        magicText.text = mp.ToString() + "/" + maxmp.ToString();

        healthBar.maxValue = maxhp;
        healthBar.value = hp;
        magicBar.maxValue = maxmp;
        magicBar.value = mp;
    }
}

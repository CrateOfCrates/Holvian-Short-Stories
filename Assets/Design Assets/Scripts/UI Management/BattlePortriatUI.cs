using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattlePortriatUI : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text healthText;
    public TMP_Text magicText;

    public Slider healthBar;
    public Slider magicBar;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        setupDisplay("jim", 35, 150, 25, 60);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setupDisplay(string name, int hp, int maxhp, int mp, int maxmp)
    {
        nameText.text = name;
        healthText.text = hp.ToString() + "/" + maxhp.ToString();
        magicText.text = mp.ToString() + "/" + maxmp.ToString();

        healthBar.maxValue = maxhp;
        healthBar.value = hp;
        magicBar.maxValue = maxmp;
        magicBar.value = mp;
    }
}

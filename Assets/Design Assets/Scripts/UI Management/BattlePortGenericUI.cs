using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System;

public class BattlePortGenericUI : MonoBehaviour
{
    [SerializeField] protected TMP_Text healthText;
    [SerializeField] protected TMP_Text magicText;
    [SerializeField] protected Slider healthBar;
    [SerializeField] protected Slider magicBar;

    //the coords of the original are
    [SerializeField] private RectTransform statusCreator;
    [SerializeField] private GameObject statusDisplayPrefab;

    [SerializeField] private int horizontalShift;
    [SerializeField] private int verticalShift;

    private Dictionary<StatusEffectObject, StatusEffectUI> statusEffectDisplays = new Dictionary<StatusEffectObject, StatusEffectUI>();

    private const int numberinRow = 4;
    private const int numberinColumn = 2;
    private const int maxStatusDisplay = numberinRow * numberinColumn;
    private const float ratio = 0.03125f;

    protected void SetupSliders(int hp, int maxhp, int mp, int maxmp)
    {
        healthText.text = hp.ToString() + "/" + maxhp.ToString();
        magicText.text = mp.ToString() + "/" + maxmp.ToString();

        healthBar.maxValue = maxhp;
        healthBar.value = hp;
        magicBar.maxValue = maxmp;
        magicBar.value = mp;
    }

    protected void LoadStatusEffects(Dictionary<StatusEffectObject, StatusEffectInstance> statusEffects)
    {   
        //exit if no status effects
        if(statusEffects.Count == 0){return;} 

        //targets
        List<StatusEffectObject> toBeRemoved = new List<StatusEffectObject>();

        foreach (var kvp in statusEffects)
        {
            
            if (statusEffectDisplays.ContainsKey(kvp.Key))
            {
                //Debug.Log("ooohh im so big and round");
                statusEffectDisplays[kvp.Key].UpdateDisplay(kvp.Value.DisplaySprite, kvp.Value.Count);
                if(kvp.Value.Count == 0)
                {
                    toBeRemoved.Add(kvp.Key);
                }
            } else
            {
                //no key present
                
                //dont add a new one if the count is ZERO
                if(kvp.Value.Count == 0) {continue;}

                GameObject temp = Instantiate(statusDisplayPrefab, statusCreator);
                StatusEffectUI target = temp.GetComponent<StatusEffectUI>();

                target.UpdateDisplay(kvp.Value.DisplaySprite, kvp.Value.Count);

                //determine how to move
                int ct = statusEffectDisplays.Count;
                int vertshift = (int)Math.Floor((double)(ct / numberinRow));
                int horishift = (int)Math.Floor((double)(ct % numberinRow));

                target.SetPos(horishift*horizontalShift, vertshift*verticalShift);

                statusEffectDisplays[kvp.Key] = target;

            }
        }
        
        //remove uncessary
        foreach (StatusEffectObject seo in toBeRemoved)
        {
            statusEffectDisplays[seo].DestorySelf();
            statusEffectDisplays.Remove(seo);
            
        }

        //reset internal positions
        int i = 0;
        foreach (var value in statusEffectDisplays.Values)
        {
            int vertshift = (int)Math.Floor((double)(i / numberinRow));
            int horishift = (int)Math.Floor((double)(i % numberinRow));

            value.SetPos(horishift*horizontalShift, vertshift*verticalShift);
            i++;

        }
    }
}

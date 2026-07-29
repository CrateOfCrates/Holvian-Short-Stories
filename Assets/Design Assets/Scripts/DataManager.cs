using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.SceneManagement; 
using System;
using TMPro;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    [SerializeField] private List<PlayerData> allPlayerData;
    [SerializeField] private EncounterObject encounter;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    //Getters
    public IReadOnlyList<PlayerData> AllPlayerData => allPlayerData;
    public EncounterObject Encounter => encounter;

    //Setters
    public void BattleEncounter(EncounterObject encounter)
    {
        this.encounter = encounter;
        SceneManager.LoadScene("CombatTestScene");
    }

    //index dictates which player to load from base
    public void LoadPlayerFromBase(int index)
    {
        allPlayerData[index].LoadData(allPlayerData[index].BaseInfo);
    }
}

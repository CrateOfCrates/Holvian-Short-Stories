using UnityEngine;
using UnityEngine.SceneManagement; 
using System.Collections.Generic;

public class loadTest : MonoBehaviour
{
    public EncounterObject encounterObject;
    public List<bool> b;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DataManager.Instance.LoadPlayerFromBase(1);
        DataManager.Instance.LoadPlayerFromBase(2);
        DataManager.Instance.LoadPlayerFromBase(3);
        DataManager.Instance.LoadPlayerFromBase(5);
        DataManager.Instance.BattleEncounter(encounterObject, b);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

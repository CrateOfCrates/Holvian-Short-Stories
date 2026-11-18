using UnityEngine;
using UnityEngine.SceneManagement; 
using System.Collections.Generic;

public class loadTest : MonoBehaviour
{
    public EncounterObject encounterObject;
    public List<bool> b;
    public List<PlayerObject> pbs;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DataManager.Instance.LoadPlayerFromBase(0, pbs[0]);
        DataManager.Instance.LoadPlayerFromBase(1, pbs[1]);
        DataManager.Instance.LoadPlayerFromBase(2, pbs[2]);
        DataManager.Instance.LoadPlayerFromBase(3, pbs[3]);
        DataManager.Instance.BattleEncounter(encounterObject, b);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

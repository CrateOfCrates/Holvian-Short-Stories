using UnityEngine;
using UnityEngine.SceneManagement; 

public class loadTest : MonoBehaviour
{
    public EncounterObject encounterObject;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DataManager.Instance.LoadPlayerFromBase(0);
        DataManager.Instance.LoadPlayerFromBase(1);
        DataManager.Instance.BattleEncounter(encounterObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

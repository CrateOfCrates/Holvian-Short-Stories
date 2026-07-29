using System;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(menuName = "Encounter")]
public class EncounterObject : ScriptableObject
{
    [Serializable]
    public struct EnemyEncounter
    {
        [SerializeField] public int xLocation;
        [SerializeField] public int yLocation;

        public EnemyObject enemyObject;

    }
    
    [field: SerializeField] private List<EnemyEncounter> enemies;
    public List<EnemyEncounter> Enemies => enemies;

    public EnemyObject GetEnemyObject(int index)
    {
        return enemies[index].enemyObject;
    }

    public int GetXLocation(int index)
    {
        return enemies[index].xLocation;
    }

    public int GetYLocation(int index)
    {
        return enemies[index].yLocation;
    }


}

using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Combat/EnemyEncounter")]
public class EncounterObject : ScriptableObject
{
    [Serializable]
    public struct EnemyEncounter
    {
        [SerializeField] public double xLocation;
        [SerializeField] public double yLocation;
        [SerializeField] public double xLocationUI;
        [SerializeField] public double yLocationUI;

        public EnemyObject enemyObject;

    }
    
    [field: SerializeField] private List<EnemyEncounter> enemies;
    public List<EnemyEncounter> Enemies => enemies;

    public EnemyObject GetEnemyObject(int index)
    {
        return enemies[index].enemyObject;
    }

    public double GetXLocation(int index)
    {
        return enemies[index].xLocation;
    }

    public double GetYLocation(int index)
    {
        return enemies[index].yLocation;
    }

    public double GetXLocationUI(int index)
    {
        return enemies[index].xLocationUI;
    }

    public double GetYLocationUI(int index)
    {
        return enemies[index].yLocationUI;
    }


}

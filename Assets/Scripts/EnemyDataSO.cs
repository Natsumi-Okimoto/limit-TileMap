
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="EnemyDataSO",menuName ="Create EnemyDataSO")]
public class EnemyDataSO : ScriptableObject
{
    [SerializeField]
    public List<EnemyData> enemyDatasList = new List<EnemyData>();
}

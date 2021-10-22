
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="ConditionDataSO",menuName = "Create ConditionDataSO")]
public class ConditionDataSO : ScriptableObject
{
    [SerializeField]
    public List<ConditionData> conditionDatasList = new List<ConditionData>();
}

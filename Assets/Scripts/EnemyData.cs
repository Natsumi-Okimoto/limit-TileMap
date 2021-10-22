using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData 
{
    public string name;
    public int enemyNo;
    public int hp;
    public int attackPower;
    public float moveSpeed;
    public float attackInterval;
    public int exp;

    public EnemyController enemyPrefab;

    // デバフ用のコンディションのデータ
    public EnemyDebuffData[] debuffDatas;


    /// <summary>
    /// デバフ用のコンディションの登録用
    /// </summary>
    [System.Serializable]
    public class EnemyDebuffData
    {

        // デバフ用のコンディションの設定
        public ConditionType debuffConditionType;

        // デバフ用のコンディションの付与確率
        [Range(0, 100)]
        public int rate;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{

    [SerializeField]
    public int DestroyCount;
    public int ClearCount;

    [SerializeField]
    public EnemyController enemyPrefab;
    [SerializeField]
    public Transform leftbottomTran;
    [SerializeField]
    public Transform rightTopTran;
    [SerializeField]
    public BattleUIManager battleUI;

   
    // Start is called before the first frame update
    void Start()
    {
        GenerateEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GenerateEnemy()
    {
        for(int i = 0; i < ClearCount; i++)
        {
            float PosX = Random.Range(leftbottomTran.position.x, rightTopTran.position.x);
            float PosZ = Random.Range(leftbottomTran.position.z, rightTopTran.position.z);
            EnemyController enemy = Instantiate(enemyPrefab,new Vector3(PosX,leftbottomTran.position.y,PosZ),enemyPrefab.transform.rotation);
            enemy.SetUpEnemyParameter(this,battleUI);
        }
    }

    public void AddDestroyCount()
    {
        DestroyCount++;
        CheckClearCount();
    }

    private void CheckClearCount()
    {
        if (DestroyCount >= ClearCount)
        {
            Debug.Log("clear");
            SceneStateManager.instance.PreparateStageScene(SceneName.Main);
        }
    }
}

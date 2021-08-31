using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{

    [SerializeField]
    public int DestroyCount;
    public int ClearCount;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddDestroyCount()
    {
        DestroyCount++;
    }

    private void CheckClearCount()
    {
        if (DestroyCount >= ClearCount)
        {
            Debug.Log("clear");
            //SceneStateManager.instance.PreparateStageScene(SceneName.Main);
        }
    }
}

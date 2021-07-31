using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySymbol : MonoBehaviour
{
    public SymbolType symbolType;
public void StartBattle()
    {
        SceneStateManager.instance.NextScene(SceneName.Battle);

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

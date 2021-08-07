using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymbolBase : MonoBehaviour
{
     public SymbolType symbolType;


    public virtual void TriggerSymbol()
    {
        DestroySymbol();
        //子供の中に処理を書く

    }

    protected virtual void DestroySymbol()
    {
        Destroy(gameObject, 0.5f);
        //TODO 演出の獲得
    }
    // Start is called before the first frame update
    protected virtual void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }
}

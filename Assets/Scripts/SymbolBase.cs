using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymbolBase : MonoBehaviour
{
     public SymbolType symbolType;


    public virtual void TriggerSymbol()
    {
        DestroySymbol();
        //�q���̒��ɏ���������

    }

    protected virtual void DestroySymbol()
    {
        Destroy(gameObject, 0.5f);
        //TODO ���o�̊l��
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

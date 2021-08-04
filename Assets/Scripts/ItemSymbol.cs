using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSymbol : MonoBehaviour
{
    public SymbolType symbolType;
   
   

    public void HealMoveCount()
    {
        MapMoveController.MaxMoveCount+= 10;
       
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

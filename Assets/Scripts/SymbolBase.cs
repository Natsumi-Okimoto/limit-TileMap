using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymbolBase : MonoBehaviour
{
     public SymbolType symbolType;


    public virtual void TriggerSymbol()
    {
        DestroySymbol();
        //q‹Ÿ‚Ì’†‚Éˆ—‚ğ‘‚­

    }

    protected virtual void DestroySymbol()
    {
        Destroy(gameObject, 0.5f);
        //TODO ‰‰o‚ÌŠl“¾
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

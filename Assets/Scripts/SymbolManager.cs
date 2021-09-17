using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Tilemaps;

/// <summary>
/// シンボルの生成・管理・制御を行うクラス
/// </summary>
public class SymbolManager : MonoBehaviour
{
    [SerializeField] 
    private List<SymbolBase> symbolsList = new List<SymbolBase>();

    public List<SymbolBase> SymbolsList
    {
        set => symbolsList = value;
        get => symbolsList;
    }

    [SerializeField] // Debug 用。インスペクターで確認できるようにしている
    private List<EnemySymbol> enemiesList = new List<EnemySymbol>();

    [SerializeField] // カメラのマスク機能を操作する場合に SpriteMask をアサイン
    private Transform spriteMaskTran;

    public Tilemap tilemapCollider; // Grid_Collison の Tilemap をアサイン
    // Start is called before the first frame update
    void Start()
    {
        
    }

    /// <summary>
    /// すべてのシンボルの初期設定
    /// </summary>
    public void SetUpAllSymbols()
    {
        List<SymbolBase> specialSymbols = new List<SymbolBase>();

        for(int i = 0; i < symbolsList.Count; i++)
        {
            symbolsList[i].transform.SetParent(this.transform);
            symbolsList[i].OnEnterSymbol(this);

            // 特殊シンボルの確認と List への追加
            if (symbolsList[i].symbolType == SymbolType.Orb)
            {
                specialSymbols.Add(symbolsList[i]);
            }
        }

        // Enemy の種類だけを抽出して List に代入
        enemiesList = GetListSimbolTypeFromSymbolsList(SymbolType.enemy);

        // TODO 各特殊シンボルをエネミーの上に配置
    }

    /// <summary>
    /// Symbol の List を取得
    /// </summary>
    /// <returns></returns>
    public List<SymbolBase> GetSymbolsList()
    {
        return symbolsList;
    }

    /// <summary>
    /// すべてのシンボルの画像を表示/非表示
    /// </summary>
    public void SwitchDisplayAllSymbols(bool isSwitch)
    {
        for (int i=0;i<symbolsList.Count;i++)
        {
            symbolsList[i].SwitchDisplaySymbol(isSwitch);
        }
    }

    /// <summary>
    /// 指定された以外のシンボルのゲームオブジェクトの表示/非表示
    /// </summary>
    public void SwitchActivateExeptSymbols(bool isSwitch,int exceptSymbolTypeNo)
    {
        foreach(SymbolBase symbol in symbolsList.Where(x => x.symbolType != (SymbolType)exceptSymbolTypeNo))
        {
            symbol.SwitchActivateSymbol(isSwitch);
        }
    }

    /// <summary>
    /// List からシンボルを削除
    /// </summary>
    /// <param name="symbol"></param>
    public void RemoveSymbolsList(SymbolBase symbol)
    {
        symbolsList.Remove(symbol);
    }

    /// <summary>
    /// List からすべてのシンボルを削除
    /// </summary>
    public void AllClearSymbolsList()
    {
        symbolsList.Clear();
    }

    /// <summary>
    /// SymbolList から引数で指定した種類のみを抽出する
    /// </summary>
    private List<EnemySymbol>GetListSimbolTypeFromSymbolsList(SymbolType getSymbolType)
    {
        return symbolsList.Where(x => x.symbolType == getSymbolType).Select(x => x.GetComponent<EnemySymbol>()).ToList();
    }

    /// <summary>
    /// 全エネミーのシンボルの移動処理
    /// </summary>
    /// <returns></returns>
    public IEnumerator EnemisMove()
    {
        for (int i=0;i<enemiesList.Count;i++)
        {
            // プレイヤーと接触しているエネミーは移動させない
            if (enemiesList[i].isSymbolTriggerd)
            {
                continue;
            }

            // エネミーの移動
            enemiesList[i].EnemyMove();

            // １体ずつ時間差で動くように、少しだけ処理を中断して待機
            yield return new WaitForSeconds(0.05f);

            Debug.Log("敵の移動：" + i + "体目");
        }
    }

    /// <summary>
    /// エネミーの List から情報の削除
    /// </summary>
    /// <param name="enemySymbol"></param>
    public void RemoveEnemySymbol(EnemySymbol enemySymbol)
    {
        enemiesList.Remove(enemySymbol);
    }

    /// <summary>
    /// すべてのエネミーのコライダーを制御
    /// </summary>
    /// <param name="isSwitch"></param>
    public void SwitchEnemyCollider(bool isSwitch)
    {
        for(int i = 0; i < enemiesList.Count; i++)
        {
            enemiesList[i].SwitchCollider(isSwitch);
        }
    }

    /// <summary>
    /// SpriteMask ゲームオブジェクトの Transfrom を取得
    /// </summary>
    /// <returns></returns>
    public Transform GetSpriteMaskTransform()
    {
        return spriteMaskTran;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

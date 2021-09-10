using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using System;


public enum StageType
{
    Field,
    Dungeon,
}

[System.Serializable]
public struct SymbolGenerateData
{
    public SymbolBase symbolBasePrefab;
    public int symbolWeight;
}
public class StageGenerater : MonoBehaviour
{

    //StageType.Field用のタイル群
    [SerializeField] private Tile[] fieldBaseTiles;
    [SerializeField] private Tile[] fieldWalkTiles;
    [SerializeField] private Tile[] fieldCollisionTiles;

    //タイルを配置するマップ
    [SerializeField] private Tilemap tileMapBase;
    [SerializeField] private Tilemap tileMapWalk;
    [SerializeField] private Tilemap tileMapCollision;

    //並べる数
    [SerializeField] private int row; //行/水平（横）方向
    [SerializeField] private int column;　//列/垂直（縦）方向

    [SerializeField]
    private List<SymbolGenerateData> symbolGenerateDatasList = new List<SymbolGenerateData>();
    [SerializeField]
    private List<SymbolGenerateData> specialSymbolGenerateDatasList = new List<SymbolGenerateData>();

    [SerializeField, Header("シンボルの生成率"), Range(0, 100)]
    private int generateSymbolRate;
    // Start is called before the first frame update
    void Start()
    {
        //Debug用
        GenerateStageFromRandomTiles();
        GenerateSymbols(-1);
    }

    public void GenerateStageFromRandomTiles(StageType stageType = StageType.Field)
    {
        //Grid_Baseと外壁用のGrid_Colloderを配置
        for(int i = -row; i < row; i++)
        {
            for(int j = -column; j < column; j++)
            {
                switch (stageType)
                {
                    case StageType.Field:
                        if (i == -row || i == row - 1 || j == -column || j == column - 1)
                        {
                            tileMapCollision.SetTile(new Vector3Int(i, j, 0), fieldCollisionTiles[0]);
                        }
                        else
                        {
                            tileMapBase.SetTile(new Vector3Int(i, j, 0), fieldBaseTiles[0]);
                        }
                        break;

                    case StageType.Dungeon:
                    default:
                        break;
                }
            }
        }

        int generateValue = 0;

        for(int i = -row; i < row; i++)
        {
            for(int j = -column; j < column; j++)
            {
                if (i == -row || i == row - 1 || j == -column || j == column - 1 || (i == 0 && j == 0))
                {
                    continue;
                }

                int maxRandomRange = UnityEngine.Random.Range(30, 80);

                generateValue += UnityEngine.Random.Range(0, maxRandomRange);

                if (generateValue <= 100)
                {
                    continue;
                }

                if (UnityEngine.Random.Range(0, 100) <= 20)
                {
                    tileMapCollision.SetTile(new Vector3Int(i, j, 0), fieldCollisionTiles[UnityEngine.Random.Range(0, fieldCollisionTiles.Length)]);
                }
                else
                {
                    tileMapWalk.SetTile(new Vector3Int(i, j, 0), fieldWalkTiles[UnityEngine.Random.Range(0, fieldWalkTiles.Length)]);
                }

                generateValue = 0;
            }
        }
    }

    /// <summary>
    /// 通常のシンボルをランダムに作成
    /// </summary>
    /// <param name="generateSymbolCount"></param>
    /// <returns></returns>
    public List<SymbolBase>GenerateSymbols(int generateSymbolCount)
    {
        // List に登録する
        List<SymbolBase> symbolsList = new List<SymbolBase>();

        // 重み付けの合計値を算出
        int totalWeight = symbolGenerateDatasList.Select(x => x.symbolWeight).Sum();

        for(int i = -row + 1; i < row - 1; i++)
        {
            for(int j = -column + 1; j < column - 1; j++)
            {
                // プレイヤーのスタート地点の場合
                if (i == 0 && j == 0)
                {
                    // 何も行わずに次の処理へ
                    continue;
                }

                // タイルマップの座標に変換
                Vector3Int tilePos = tileMapCollision.WorldToCell(new Vector3(i, j, 0));

                // タイルの ColliderType が Grid ではないか確認
                if (tileMapCollision.GetColliderType(tilePos) == Tile.ColliderType.Grid)
                {
                    // Grid の場合には配置しないので、何も行わずに次の処理へ
                    continue;
                }

                // 80 % はシンボルなし => 264 マスの場合、大体35～55個シンボルが出来る
                if (UnityEngine.Random.Range(0, 100) > generateSymbolRate)
                {
                    continue;
                }

                int index = 0;
                int value = UnityEngine.Random.Range(0, totalWeight);

                // 重みづけから生成するシンボルを確認
                for (int x = 0; x < symbolGenerateDatasList.Count; x++)
                {
                    if (value <= symbolGenerateDatasList[x].symbolWeight)
                    {
                        index = x;

                        break;
                    }
                    value -= symbolGenerateDatasList[x].symbolWeight;
                }

                // 抽選されたシンボルを生成
                symbolsList.Add(Instantiate(symbolGenerateDatasList[index].symbolBasePrefab, new Vector3(i, j, 0), Quaternion.identity));

                generateSymbolCount--;

                // generateSymbolCount = -1 でスタートの場合は抽選回数なし
                if (generateSymbolCount == 0)
                {
                    break;
                }
            }
            if (generateSymbolCount == 0)
            {
                break;
            }
        }
        // 完成したリストを戻す
        return symbolsList;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}

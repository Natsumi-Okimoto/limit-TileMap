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
    // Start is called before the first frame update
    void Start()
    {
        //Debug用
        GenerateStageFromRandomTiles();
        //GenerateSymbols(-1);
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


    // Update is called once per frame
    void Update()
    {
        
    }
}

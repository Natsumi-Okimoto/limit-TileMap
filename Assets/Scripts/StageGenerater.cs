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

    //StageType.Field�p�̃^�C���Q
    [SerializeField] private Tile[] fieldBaseTiles;
    [SerializeField] private Tile[] fieldWalkTiles;
    [SerializeField] private Tile[] fieldCollisionTiles;

    //�^�C����z�u����}�b�v
    [SerializeField] private Tilemap tileMapBase;
    [SerializeField] private Tilemap tileMapWalk;
    [SerializeField] private Tilemap tileMapCollision;

    //���ׂ鐔
    [SerializeField] private int row; //�s/�����i���j����
    [SerializeField] private int column;�@//��/�����i�c�j����

    [SerializeField]
    private List<SymbolGenerateData> symbolGenerateDatasList = new List<SymbolGenerateData>();
    [SerializeField]
    private List<SymbolGenerateData> specialSymbolGenerateDatasList = new List<SymbolGenerateData>();

    [SerializeField, Header("�V���{���̐�����"), Range(0, 100)]
    private int generateSymbolRate;
    // Start is called before the first frame update
    void Start()
    {
        //Debug�p
        GenerateStageFromRandomTiles();
        GenerateSymbols(-1);
    }

    public void GenerateStageFromRandomTiles(StageType stageType = StageType.Field)
    {
        //Grid_Base�ƊO�Ǘp��Grid_Colloder��z�u
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
    /// �ʏ�̃V���{���������_���ɍ쐬
    /// </summary>
    /// <param name="generateSymbolCount"></param>
    /// <returns></returns>
    public List<SymbolBase>GenerateSymbols(int generateSymbolCount)
    {
        // List �ɓo�^����
        List<SymbolBase> symbolsList = new List<SymbolBase>();

        // �d�ݕt���̍��v�l���Z�o
        int totalWeight = symbolGenerateDatasList.Select(x => x.symbolWeight).Sum();

        for(int i = -row + 1; i < row - 1; i++)
        {
            for(int j = -column + 1; j < column - 1; j++)
            {
                // �v���C���[�̃X�^�[�g�n�_�̏ꍇ
                if (i == 0 && j == 0)
                {
                    // �����s�킸�Ɏ��̏�����
                    continue;
                }

                // �^�C���}�b�v�̍��W�ɕϊ�
                Vector3Int tilePos = tileMapCollision.WorldToCell(new Vector3(i, j, 0));

                // �^�C���� ColliderType �� Grid �ł͂Ȃ����m�F
                if (tileMapCollision.GetColliderType(tilePos) == Tile.ColliderType.Grid)
                {
                    // Grid �̏ꍇ�ɂ͔z�u���Ȃ��̂ŁA�����s�킸�Ɏ��̏�����
                    continue;
                }

                // 80 % �̓V���{���Ȃ� => 264 �}�X�̏ꍇ�A���35�`55�V���{�����o����
                if (UnityEngine.Random.Range(0, 100) > generateSymbolRate)
                {
                    continue;
                }

                int index = 0;
                int value = UnityEngine.Random.Range(0, totalWeight);

                // �d�݂Â����琶������V���{�����m�F
                for (int x = 0; x < symbolGenerateDatasList.Count; x++)
                {
                    if (value <= symbolGenerateDatasList[x].symbolWeight)
                    {
                        index = x;

                        break;
                    }
                    value -= symbolGenerateDatasList[x].symbolWeight;
                }

                // ���I���ꂽ�V���{���𐶐�
                symbolsList.Add(Instantiate(symbolGenerateDatasList[index].symbolBasePrefab, new Vector3(i, j, 0), Quaternion.identity));

                generateSymbolCount--;

                // generateSymbolCount = -1 �ŃX�^�[�g�̏ꍇ�͒��I�񐔂Ȃ�
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
        // �����������X�g��߂�
        return symbolsList;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}

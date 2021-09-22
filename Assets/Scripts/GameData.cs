using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public float MaxMoveCount;
    public int HitPoint;
    public int EnemyAttackPower;
    public int MaxHitPoint;
    [Header("���݂̃G�l�~�[��HP")]
    public int Enemyhp;
    public int MaxEnemyHP;
    public int MaxWaveCount;
    [SerializeField, Header("���݃E�F�[�u�̒l")]
    public int carrentWaveCount;
    public int attackPower;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

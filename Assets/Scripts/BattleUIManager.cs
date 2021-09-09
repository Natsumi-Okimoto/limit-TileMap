using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class BattleUIManager : MonoBehaviour
{

    [SerializeField]
    private Slider slider;

    [SerializeField]
    private Slider Slider;

    [SerializeField]
    private Text txtInfo;

    [SerializeField]
    private CanvasGroup canvasGroupInfo;



    public void UpdateEnemyHPvar(int Enemyhp,int MaxEnemyHP)
    {
        float enemyValue = (float)Enemyhp / MaxEnemyHP;
        slider.DOValue(enemyValue, 0.25f);
    }

    public void UpdatePlayerHPvar()
    {
        float value = (float)GameData.instance.HitPoint / GameData.instance.MaxHitPoint;
        Debug.Log(value);
        Slider.DOValue(value, 0.25f);
    }

    /// <summary>
    /// �Q�[���I�[�o�[�\��
    /// </summary>
    public void DisplayGameOverInfo()
    {

        // InfoBackGround �Q�[���I�u�W�F�N�g�̎��� CanvasGroup �R���|�[�l���g�� Alpha �̒l���A1�b������ 1 �ɕύX���āA�w�i�ƕ�������ʂɌ�����悤�ɂ���
        canvasGroupInfo.DOFade(1.0f, 1.0f);

        // ��������A�j���[�V���������ĕ\��
        txtInfo.DOText("Game Over...", 1.0f);
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

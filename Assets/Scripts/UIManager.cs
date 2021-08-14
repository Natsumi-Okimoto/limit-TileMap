using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text txtScore;        // txtScore �Q�[���I�u�W�F�N�g�̎��� Text �R���|�[�l���g���C���X�y�N�^�[����A�T�C������
    [SerializeField]
    private Slider slider;

    /// <summary>
    /// �X�R�A�\�����X�V
    /// </summary>
    /// <param name="MoveCount"></param>
    public void UpdateDisplayMoveCount(float MoveCount)
    {
        txtScore.text = MoveCount.ToString();
    }

    public void UpdateHPvar()
    {
        float value =(float) GameData.instance.HitPoint / GameData.instance.MaxHitPoint;
        slider.DOValue(value, 0.25f);
    }
}

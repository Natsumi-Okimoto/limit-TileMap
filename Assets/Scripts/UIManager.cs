using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text txtScore;        // txtScore �Q�[���I�u�W�F�N�g�̎��� Text �R���|�[�l���g���C���X�y�N�^�[����A�T�C������

    /// <summary>
    /// �X�R�A�\�����X�V
    /// </summary>
    /// <param name="MoveCount"></param>
    public void UpdateDisplayMoveCount(float MoveCount)
    {
        txtScore.text = MoveCount.ToString();
    }
}

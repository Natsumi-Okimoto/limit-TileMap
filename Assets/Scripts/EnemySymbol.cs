using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Tilemaps;


public enum MoveDirectionType
{
    Up,
    Down,
    Left,
    Right,
    Count
}
public class EnemySymbol : SymbolBase
{
    private Tilemap tilemapCollider;
    private BoxCollider2D boxCol;
    private float moveDuration = 0.05f;


    public override void OnEnterSymbol(SymbolManager symbolManager)
    {
        // SymbolBase(�e�N���X)�ɋL�q����Ă��� OnEnterSymbol ���\�b�h�����s
        base.OnEnterSymbol(symbolManager);

        // �ړ������ Ray �𗘗p����̂ŁA�v���C���[�Ɠ����悤�ɁA�R���C�_�[�̃^�C���}�b�v�̏����擾
        tilemapCollider = symbolManager.tilemapCollider;

        // BoxColider2D �̏����擾
        TryGetComponent(out boxCol );
    }
    public override void TriggerSymbol(MapMoveController mapMoveController)
    {
        // SymbolBase(�e�N���X)�� TriggerSymbol ���\�b�h�����s
        base.TriggerSymbol(mapMoveController);
        // DOTween �ŃG�l�~�[�̃V���{�����A�j��������̂ŁAOnExitSymbol ���\�b�h���Ŏ��s����
        //StartBattle();

        Debug.Log("�ړ���œG�ɐڐG");

        // �G�l�~�[�̃V���{���̃A�j�����o�B���o��AOnComplete ���\�b�h���g���� OnExitSymbol ���\�b�h���s
        tween = transform.DOShakeScale(0.75f, 1.0f).SetEase(Ease.OutQuart).OnComplete(() => { OnExitSymbol(); });
    }

    protected override void OnExitSymbol()
    {
        // �G�l�~�[�̃V���{���p�� List ����폜
        symbolManager.RemoveEnemySymbol(this);

        // SymbolBase(�e�N���X)�� OnExitSymbol �����s
        base.OnExitSymbol();

        // �o�g���̏���
        StartBattle();
    }

    /// <summary>
    /// �o�g���̏���
    /// </summary>
    public void StartBattle()
    {
        //SceneStateManager.instance.NextScene(SceneName.Battle);
        // �V�[���J�ڂ̏���
        SceneStateManager.instance.PreparateBatlleScene();

        Debug.Log("Start");

    }

    /// <summary>
    /// �G�l�~�[�������_���ȕ����ɂP�}�X�ړ����邩�A���̏�őҋ@
    /// </summary>
    public void EnemyMove()
    {
        // �ړ���������������_���ɂP�ݒ�
        MoveDirectionType randomDirType = (MoveDirectionType)Random.Range(0, (int)MoveDirectionType.Count);

        // �ړ���������̏������W�ɕϊ�
        Vector3 nextPos = GetMoveDirection(randomDirType);

        // �����̃R���C�_�[���I�t�ɂ��� Ray �������̃R���C�_�[�ɓ������Ă��܂��딻���h��
        SwitchCollider(false);

        // �ړ���������� Ray �𓊎˂��đ��̃V���{�������݂��Ă��Ȃ������m�F
        RaycastHit2D hit = Physics2D.Raycast(transform.position, nextPos, 0.8f, LayerMask.GetMask("Symbol"));

        // Scene �r���[�ɂ� Ray �̉���
        Debug.DrawRay(transform.position, nextPos, Color.blue, 0.8f);

        // �R���C�_�[���I��
        SwitchCollider(true);

        // Ray �̓��ː�ɕʂ̃V���{��������ꍇ�ɂ� => �G�l�~�[�݂̂Ƃ肠�������O�B�A�C�e���̏�ɃG�l�~�[�����悤�ɂȂ�̂�
        if (hit.collider!=null)
        {
            // �ړ������I��
            return;
        }

        // Ray ���q�b�g���A���ꂪ�G�l�~�[�ł���Ȃ�
        if (hit.collider!=null&&hit.collider.TryGetComponent(out EnemySymbol enemySymbol))
        {
            // �ړ������I��
            return;
        }

        // �ړ��ł���^�C�����^�C���}�b�v�̍��W�ɕϊ����Ċm�F(�v���C���[�Ɠ�����@)
        Vector3Int tilePos = tilemapCollider.WorldToCell(transform.position + nextPos);

        // Grid �̃R���C�_�[�łȂ����(�v���C���[�Ɠ�����@)
        if (tilemapCollider.GetColliderType(tilePos) != Tile.ColliderType.Grid)
        {
            // �ړ�
            transform.DOMove(transform.position + nextPos, moveDuration).SetEase(Ease.Linear);
        }
    }

    /// <summary>
    /// �ړ���������̏������W�ɕϊ�
    /// </summary>
    /// <param name="nextDirection"></param>
    /// <returns></returns>
    private Vector3 GetMoveDirection(MoveDirectionType nextDirection)
    {
        // switch ���̏ȗ��L�@(case : break �̑���� => ���g���B�Ō�� _ => �� default: break �Ɠ���)
        return nextDirection switch
        {
            MoveDirectionType.Up => new Vector2(0, 1),
            MoveDirectionType.Down => new Vector2(0, -1),
            MoveDirectionType.Left => new Vector2(-1, 0),
            MoveDirectionType.Right => new Vector2(1, 0),
            _ => Vector2.zero
        };
    }

    /// <summary>
    /// �R���C�_�[�̃I���I�t�؂�ւ�
    /// </summary>
    /// <param name="isSwicth"></param>
    public void SwitchCollider(bool isSwitch)
    {
        boxCol.enabled = isSwitch;
    }
    protected override void DestroySymbol()
    {
        base.DestroySymbol();

    }

}

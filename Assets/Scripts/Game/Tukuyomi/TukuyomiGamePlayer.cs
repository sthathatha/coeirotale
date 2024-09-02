using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

/// <summary>
/// ����݂�����@�v���C���[
/// </summary>
public class TukuyomiGamePlayer : MonoBehaviour
{
    #region �萔

    public const float PLAYER_SIZE = 30f;
    private const float FREE_MOVE_SPEED = 200f;

    #endregion

    #region �����o�[

    public TukuyomiGameSystem system;
    public TukuyomiGamePlayField playField;

    #endregion

    #region �ϐ�

    /// <summary>
    /// �ړ����[�h
    /// </summary>
    public enum MoveMode
    {
        /// <summary>�ړ��s��</summary>
        Disable = 0,
        /// <summary>���R�ړ�</summary>
        Free,
        /// <summary>�}�X�ڈړ�</summary>
        Cell,
    }
    /// <summary>�ړ����[�h</summary>
    public MoveMode moveMode { get; set; } = MoveMode.Disable;

    /// <summary>�ˌ����[�h</summary>
    public bool ShotEnable { get; set; } = false;

    private Vector2 _cellLocation;
    /// <summary>
    /// �Z���ʒu
    /// </summary>
    public Vector2 CellLocation
    {
        get { return _cellLocation; }
        set
        {
            _cellLocation = value;
            transform.localPosition = playField.GetCellPosition(_cellLocation);
        }
    }

    #endregion

    /// <summary>
    /// �X�V
    /// </summary>
    private void Update()
    {
        var input = InputManager.GetInstance();

        if (moveMode == MoveMode.Free)
        {
            // �t���[�ړ��̏ꍇ
            var moveVec = Vector3.zero;
            var ymove = false;
            var xmove = false;
            if (input.GetKey(InputManager.Keys.Up))
            {
                moveVec.y = FREE_MOVE_SPEED;
                ymove = true;
            }
            else if (input.GetKey(InputManager.Keys.Down))
            {
                moveVec.y = -FREE_MOVE_SPEED;
                ymove = true;
            }
            if (input.GetKey(InputManager.Keys.Right))
            {
                moveVec.x = FREE_MOVE_SPEED;
                xmove = true;
            }
            else if (input.GetKey(InputManager.Keys.Left))
            {
                moveVec.x = -FREE_MOVE_SPEED;
                xmove = true;
            }
            if (xmove && ymove) moveVec *= 0.8f;
            moveVec *= Time.deltaTime;

            transform.localPosition = playField.ClampFreeField(transform.localPosition + moveVec, PLAYER_SIZE / 2f);
        }
        else if (moveMode == MoveMode.Cell)
        {
            // �Z���ړ��̏ꍇ
            var nextPos = CellLocation;
            if (input.GetKeyPress(InputManager.Keys.Up)) nextPos.y++;
            else if (input.GetKeyPress(InputManager.Keys.Down)) nextPos.y--;
            if (input.GetKeyPress(InputManager.Keys.Right)) nextPos.x++;
            else if (input.GetKeyPress(InputManager.Keys.Left)) nextPos.x--;

            if (nextPos != CellLocation)
            {
                CellLocation = playField.ClampCellField(nextPos);
                if (system.AEnemyIsInField(CellLocation))
                {
                    // ���ɓ˂����񂾂玩��������
                    system.PlayerDamage();
                    gameObject.SetActive(false);
                }
                if (system.AKinPlayMode)
                {
                    // ���̎��S�[����ʒm
                    if (system.AKinGoal == CellLocation)
                    {
                        system.ALeachKinGoal();
                    }
                }
            }
        }

        if (ShotEnable && input.GetKeyPress(InputManager.Keys.South))
        {
            // �V���b�g�쐬
            system.CreateShot(transform.localPosition, new Vector3(0, 500f), TukuyomiGameShot.ShotType.Player);
        }
    }

    /// <summary>
    /// �U���q�b�g
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �����̃V���b�g�͖���
        var shot = collision.GetComponent<TukuyomiGameShot>();
        if (shot != null) { return; }

        // ���������U���͏���
        collision.enabled = false;

        // �V�X�e���ɒʒm
        system.PlayerDamage();
    }
}

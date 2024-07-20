using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �{�X���b�V���@�U���{�[��
/// </summary>
public class PierreGameBallB : MonoBehaviour
{
    #region �萔

    #region enum

    public enum AttackType : int
    {
        Player = 0,
        Enemy,
    }

    #endregion

    #endregion

    public SpriteRenderer model;

    public AttackType attacktype { get; private set; }

    /// <summary>�ړ����x</summary>
    private Vector3 speed;

    /// <summary>�U����</summary>
    private int power;

    /// <summary></summary>
    private bool destroyFlag = false;

    #region ���

    /// <summary>
    /// �X�V����
    /// </summary>
    void Update()
    {
        // ��ɓ���������Y���]�܂��͏�����
        if (transform.localPosition.y >= PierreGameSystemB.FIELD_MAX_Y)
        {
            //speed.y = -Mathf.Abs(speed.y);
            if (speed.y > 0f)
            {
                Destroy(gameObject);
                return;
            }
        }

        // �ړ�
        transform.localPosition += speed * Time.deltaTime;

        // ���݂�Y���W�ŕ`�揇�X�V
        var so = Mathf.FloorToInt(100 - transform.position.y);
        model.sortingOrder = so;

        // 
        if (transform.localPosition.x > Constant.SCREEN_WIDTH * 0.7f ||
            transform.localPosition.y < Constant.SCREEN_WIDTH * -0.7f)
        {
            destroyFlag = true;
        }

        if (destroyFlag)
        {
            Destroy(gameObject);
        }
    }

    #endregion

    #region �g�p�@�\

    /// <summary>
    /// �����㏉���ݒ�
    /// </summary>
    /// <param name="atk">�U�������i�G�E�����j</param>
    /// <param name="startPos">�J�n�ʒu</param>
    /// <param name="spd">���x</param>
    /// <param name="col">�F</param>
    /// <param name="pow">�U����</param>
    public void SetParam(AttackType atk, Vector3 startPos, Vector3 spd, Color col, int pow)
    {
        transform.localPosition = startPos;
        attacktype = atk;
        speed = spd;
        model.color = col;
        power = pow;
    }

    /// <summary>
    /// �U����
    /// </summary>
    /// <returns></returns>
    public int GetPower() { return power; }

    /// <summary>
    /// 
    /// </summary>
    public void DestroyWait() { destroyFlag = true; }

    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F131�@�u���b�N
/// </summary>
public class F131Block : ActionEventBase
{
    #region �萔

    /// <summary>1���̍��W�T�C�Y</summary>
    public const float BLOCK_SIZE = 64f;

    /// <summary>1�}�X����̂ɂ����鎞��</summary>
    protected const float SLIP_BLOCK_TIME = 0.1f;

    #endregion

    #region �ϐ�

    /// <summary>���W�Ǘ�</summary>
    protected Vector2Int location;

    /// <summary>���W�ړ��v�Z</summary>
    protected DeltaVector3 localPos = new DeltaVector3();

    /// <summary>�V�X�e��</summary>
    protected F131System system;

    #endregion

    #region ���

    /// <summary>
    /// �J�n��
    /// </summary>
    public override void Start()
    {
        base.Start();

        system = fieldScript as F131System;
        //�z�u�ʒu������W�v�Z
        location.x = Mathf.RoundToInt(transform.localPosition.x / BLOCK_SIZE);
        location.y = Mathf.RoundToInt(transform.localPosition.y / BLOCK_SIZE);
        transform.localPosition = CalcLocPosition(location);
    }

    /// <summary>
    /// �u���b�N�C�x���g
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Exec()
    {
        yield break;
    }

    #endregion

    #region �p�u���b�N

    /// <summary>
    /// ���W�擾
    /// </summary>
    /// <returns></returns>
    public Vector2Int GetLocation() { return location; }

    #endregion

    #region �v���C�x�[�g

    /// <summary>
    /// ���W����localPosition���v�Z
    /// </summary>
    /// <param name="loc"></param>
    /// <returns></returns>
    protected static Vector3 CalcLocPosition(Vector2Int loc)
    {
        var ret = new Vector3();
        ret.x = loc.x * BLOCK_SIZE;
        ret.y = loc.y * BLOCK_SIZE;
        return ret;
    }

    #endregion
}

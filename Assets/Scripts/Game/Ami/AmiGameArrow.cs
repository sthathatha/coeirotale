using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���
/// </summary>
public class AmiGameArrow : MonoBehaviour
{
    /// <summary>�������牟���^�C�~���O�܂ł̎���</summary>
    public const float DELAY_TIME = 1f;

    /// <summary>X�ʒu</summary>
    private float _x;
    /// <summary>Y�ʒu</summary>
    private DeltaFloat _y;

    /// <summary>�|�[�Y�Ƃ鎞��</summary>
    private float _weight;

    /// <summary>
    /// �R���X�g���N�^
    /// </summary>
    public AmiGameArrow()
    {
        _y = new DeltaFloat();
        _y.Set(1f);
    }

    /// <summary>
    /// �J�n��
    /// </summary>
    void Start()
    {
        _x = transform.localPosition.x;
        _y.Set(transform.localPosition.y);

        //
        var endY = -180f;
        var time = DELAY_TIME * (transform.localPosition.y - endY) / transform.localPosition.y;
        _y.MoveTo(endY, time, DeltaFloat.MoveType.LINE);
    }

    /// <summary>
    /// �t���[������
    /// </summary>
    void Update()
    {
        _y.Update(Time.deltaTime);
        transform.localPosition = new Vector3(_x, _y.Get(), 0);
    }

    /// <summary>
    /// ���݈ʒu�̐�Βl
    /// </summary>
    /// <returns></returns>
    public float GetNowPositionAbs()
    {
        return Mathf.Abs(_y.Get());
    }

    /// <summary>
    /// ���쒆
    /// </summary>
    /// <returns></returns>
    public bool IsActive()
    {
        return _y.Get() > 0f || _y.IsActive();
    }

    /// <summary>
    /// �|�[�Y���Ԑݒ�
    /// </summary>
    /// <param name="w"></param>
    public void SetWeight(float w) { _weight = w; }
    /// <summary>
    /// �|�[�Y���Ԏ擾
    /// </summary>
    /// <returns></returns>
    public float GetWeight() { return _weight; }

    /// <summary>
    /// �ŏ����班���i�񂾏��
    /// </summary>
    /// <param name="ofs"></param>
    public void AddOffset(float ofs)
    {
        _y.AddOffset(ofs);
    }
}

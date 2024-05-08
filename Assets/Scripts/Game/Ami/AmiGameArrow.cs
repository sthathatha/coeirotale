using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���
/// </summary>
public class AmiGameArrow : MonoBehaviour
{
    /// <summary>�J�n�ʒuY</summary>
    public const float START_Y = 720f;
    /// <summary>�I���ʒuY</summary>
    public const float END_Y = -180f;
    /// <summary>�������牟���^�C�~���O�܂ł̎���</summary>
    public const float DELAY_TIME = 1f;
    /// <summary>�I��Y�܂œ�������</summary>
    private const float MOVE_TIME = DELAY_TIME * (START_Y - END_Y) / START_Y;

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
    }

    /// <summary>
    /// �J�n��
    /// </summary>
    void Start()
    {
        _x = transform.localPosition.x;
    }

    /// <summary>
    /// Time.realTimeSinceStartup���R���X�g���N�^�Ŏg���Ȃ��炵���̂ŊO����Ă�
    /// </summary>
    public void ForceStart()
    {
        _y.Set(START_Y);
        _y.MoveTo(END_Y, MOVE_TIME, DeltaFloat.MoveType.LINE);
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

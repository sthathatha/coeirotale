using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����݂�����@�O������݂����
/// </summary>
public class TukuyomiGameTukuyomiA : MonoBehaviour
{
    #region �萔

    public enum AnimType
    {
        /// <summary>�炪�ς��O</summary>
        Before = 0,
        /// <summary>��~</summary>
        Stop,
        /// <summary>�ҋ@���[�v</summary>
        Idle,
        /// <summary>���U��</summary>
        Down,
    }

    #endregion

    #region �����o�[

    public Animator face;
    public Animator body;
    public SpriteRenderer leg;

    #endregion

    /// <summary>
    /// ������
    /// </summary>
    void Start()
    {
        PlayAnim(AnimType.Before);
    }

    #region ���\�b�h

    /// <summary>
    /// �A�j���[�V�����Đ�
    /// </summary>
    /// <param name="animType"></param>
    public void PlayAnim(AnimType animType)
    {
        face.Play(animType switch
        {
            AnimType.Before => "before",
            AnimType.Stop => "stop",
            AnimType.Down => "down",
            _ => "idle",
        });
        body.Play(animType switch
        {
            AnimType.Before => "stop",
            AnimType.Stop => "stop",
            AnimType.Down => "down",
            _ => "idle",
        });
    }

    /// <summary>
    /// �ړ�
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="time"></param>
    public void Move(Vector3 pos, float time = -1f)
    {
        if (time <= 0f)
        {
            transform.localPosition = pos;
        }
        else
        {
            StartCoroutine(MoveCoroutine(pos, time));
        }
    }

    /// <summary>
    /// �ړ�
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    private IEnumerator MoveCoroutine(Vector3 pos, float time)
    {
        var p = new DeltaVector3();
        p.Set(transform.localPosition);
        p.MoveTo(pos, time, DeltaFloat.MoveType.DECEL);
        while (p.IsActive())
        {
            yield return null;
            p.Update(Time.deltaTime);

            transform.localPosition = p.Get();
        }
    }

    #endregion
}

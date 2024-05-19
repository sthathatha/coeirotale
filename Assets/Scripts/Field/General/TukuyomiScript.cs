using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// �ǔ��^����݂����
/// </summary>
public class TukuyomiScript : CharacterScript
{
    #region �萔

    /// <summary>���̋����܂Œǂ�������</summary>
    const float TRACE_DISTANCE = 100f;

    /// <summary>�ǂ������鑬�x</summary>
    const float TRACE_SPEED = 180f;

    #endregion

    #region �����o�[

    /// <summary>�ǔ��Ώ�</summary>
    public GameObject playerObject;

    #endregion

    #region �ϐ�

    /// <summary>
    /// ���샂�[�h
    /// </summary>
    public enum TukuyomiMode : int
    {
        /// <summary>�ǔ�</summary>
        Trace = 0,
        /// <summary>�ʏ�NPC</summary>
        NPC,
    }
    /// <summary>���샂�[�h</summary>
    private TukuyomiMode mode = TukuyomiMode.Trace;

    #endregion

    #region ���

    /// <summary>
    /// �J�n��
    /// </summary>
    protected override void Start()
    {
        base.Start();

        // ��{�͒ǔ����[�h
        SetMode(TukuyomiMode.Trace);
    }

    /// <summary>
    /// �t���[������
    /// </summary>
    protected override void Update()
    {
        if (mode == TukuyomiMode.Trace)
        {

            // �ǔ��ΏۂƎ����̋���Vector
            var distance = playerObject.transform.position - transform.position;

            var absoluteDist = distance.magnitude;
            if (absoluteDist > TRACE_DISTANCE)
            {
                // ���ȏ㗣��Ă�����ǔ��ړ�
                // ���ꗦ
                var distanceRate = absoluteDist / TRACE_DISTANCE;
                // ���x�{��
                var speedRate = 0f;

                if (distanceRate > 2f)
                {
                    // �{�ȏ㗣��Ă�����葬
                    speedRate = 1f;
                }
                else if (distanceRate > 1.0625f)
                {
                    // ������ƂȂ�
                    speedRate = distanceRate - 1f;
                }

                if (speedRate > 0.0625f)
                {
                    PlayVectorAnim(distance);

                    var moveDelta = distance * TRACE_SPEED * speedRate / absoluteDist * Time.deltaTime;
                    transform.position += moveDelta;
                }
            }
        }

        base.Update();
    }

    /// <summary>
    /// �����n�߃I�[�o�[���C�h
    /// </summary>
    /// <param name="vec"></param>
    protected override void WalkStartAnim(Vector3 vec)
    {
        base.WalkStartAnim(vec);
        PlayVectorAnim(vec);
    }

    #endregion

    #region �p�u���b�N���\�b�h
    /// <summary>
    /// ���샂�[�h�ύX
    /// </summary>
    /// <param name="m"></param>
    public void SetMode(TukuyomiMode m)
    {
        mode = m;

        var col = GetComponent<Collider2D>();
        if (col != null)
        {
            if (m == TukuyomiMode.Trace)
            {
                col.enabled = false;
            }
            else
            {
                col.enabled = true;
            }
        }
    }

    /// <summary>
    /// �v���C���[�̉��ɏ�����
    /// </summary>
    public void InitTrace()
    {
        if (mode == TukuyomiMode.Trace)
        {
            transform.position = playerObject.transform.position + new Vector3(0, TRACE_DISTANCE, 0);
        }
    }

    /// <summary>
    /// �����I�Ɍ����ύX
    /// </summary>
    /// <param name="dir"></param>
    public void SetDirection(Constant.Direction dir)
    {
        PlayVectorAnim(dir switch
        {
            Constant.Direction.Up => new Vector3(0, 1, 0),
            Constant.Direction.Down => new Vector3(0, -1, 0),
            Constant.Direction.Left => new Vector3(-1, 0, 0),
            _ => new Vector3(1, 0, 0),
        });
    }

    #endregion

    #region �v���C�x�[�g���\�b�h

    /// <summary>
    /// �����ɑΉ�����A�j���[�V�����Đ�
    /// </summary>
    /// <param name="distance"></param>
    private void PlayVectorAnim(Vector3 distance)
    {
        // ��~�̎��͉������Ȃ�
        if (distance.sqrMagnitude < 0.0001f) { return; }

        if (Mathf.Abs(distance.x) > Mathf.Abs(distance.y))
        {
            if (distance.x > 0)
            {
                modelAnim.Play("right");
            }
            else
            {
                modelAnim.Play("left");
            }
        }
        else
        {
            if (distance.y > 0)
            {
                modelAnim.Play("up");
            }
            else
            {
                modelAnim.Play("down");
            }
        }
    }

    #endregion
}

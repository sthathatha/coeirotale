using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TukuyomiScript : CharacterScript
{
    #region �萔
    /// <summary>���̋����܂Œǂ�������</summary>
    const float TRACE_DISTANCE = 100f;

    /// <summary>�ǂ������鑬�x</summary>
    const float TRACE_SPEED = 180f;
    #endregion

    /// <summary>�ǔ��Ώ�</summary>
    public GameObject playerObject;

    /// <summary>
    /// ������
    /// </summary>
    override protected void Start()
    {
        transform.position = playerObject.transform.position + new Vector3(0, TRACE_DISTANCE, 0);

        base.Start();
    }

    /// <summary>
    /// �t���[������
    /// </summary>
    override protected void Update()
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
            } else if (distanceRate > 1.0625f)
            {
                // ������ƂȂ�
                speedRate = distanceRate - 1f;
            }

            if (speedRate > 0.0625f)
            {
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

                var moveDelta = distance * TRACE_SPEED * speedRate / absoluteDist * Time.deltaTime;
                transform.position += moveDelta;
            }
        }

        base.Update();
    }
}

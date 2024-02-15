using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TukuyomiScript : CharacterScript
{
    #region 定数
    /// <summary>この距離まで追いかける</summary>
    const float TRACE_DISTANCE = 100f;

    /// <summary>追いかける速度</summary>
    const float TRACE_SPEED = 180f;
    #endregion

    /// <summary>追尾対象</summary>
    public GameObject playerObject;

    /// <summary>
    /// 初期化
    /// </summary>
    override protected void Start()
    {
        transform.position = playerObject.transform.position + new Vector3(0, TRACE_DISTANCE, 0);

        base.Start();
    }

    /// <summary>
    /// フレーム処理
    /// </summary>
    override protected void Update()
    {
        // 追尾対象と自分の距離Vector
        var distance = playerObject.transform.position - transform.position;

        var absoluteDist = distance.magnitude;
        if (absoluteDist > TRACE_DISTANCE)
        {
            // 一定以上離れていたら追尾移動
            // 離れ率
            var distanceRate = absoluteDist / TRACE_DISTANCE;
            // 速度倍率
            var speedRate = 0f;

            if (distanceRate > 2f)
            {
                // 倍以上離れていたら定速
                speedRate = 1f;
            } else if (distanceRate > 1.0625f)
            {
                // ちょっとなら
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

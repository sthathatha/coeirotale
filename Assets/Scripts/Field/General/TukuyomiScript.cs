using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 追尾型つくよみちゃん
/// </summary>
public class TukuyomiScript : CharacterScript
{
    #region 定数

    /// <summary>この距離まで追いかける</summary>
    const float TRACE_DISTANCE = 100f;

    /// <summary>追いかける速度</summary>
    const float TRACE_SPEED = 180f;

    #endregion

    #region メンバー

    /// <summary>追尾対象</summary>
    public GameObject playerObject;

    #endregion

    #region 変数

    /// <summary>
    /// 動作モード
    /// </summary>
    public enum TukuyomiMode : int
    {
        /// <summary>追尾</summary>
        Trace = 0,
        /// <summary>通常NPC</summary>
        NPC,
    }
    /// <summary>動作モード</summary>
    private TukuyomiMode mode = TukuyomiMode.Trace;

    #endregion

    #region 基底

    /// <summary>
    /// 開始時
    /// </summary>
    protected override void Start()
    {
        base.Start();

        // 基本は追尾モード
        SetMode(TukuyomiMode.Trace);
    }

    /// <summary>
    /// フレーム処理
    /// </summary>
    protected override void Update()
    {
        if (mode == TukuyomiMode.Trace)
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
                }
                else if (distanceRate > 1.0625f)
                {
                    // ちょっとなら
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
    /// 歩き始めオーバーライド
    /// </summary>
    /// <param name="vec"></param>
    protected override void WalkStartAnim(Vector3 vec)
    {
        base.WalkStartAnim(vec);
        PlayVectorAnim(vec);
    }

    #endregion

    #region パブリックメソッド
    /// <summary>
    /// 動作モード変更
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
    /// プレイヤーの横に初期化
    /// </summary>
    public void InitTrace()
    {
        if (mode == TukuyomiMode.Trace)
        {
            transform.position = playerObject.transform.position + new Vector3(0, TRACE_DISTANCE, 0);
        }
    }

    /// <summary>
    /// 強制的に向き変更
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

    #region プライベートメソッド

    /// <summary>
    /// 向きに対応するアニメーション再生
    /// </summary>
    /// <param name="distance"></param>
    private void PlayVectorAnim(Vector3 distance)
    {
        // 停止の時は何もしない
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

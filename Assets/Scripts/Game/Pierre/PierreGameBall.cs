using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 転がるボール
/// </summary>
public class PierreGameBall : PierreGameRoadObject
{
    #region 定数
    public enum BallType : int
    {
        /// <summary>通常</summary>
        Normal = 0,
        /// <summary>左右に動く</summary>
        Drift,
        /// <summary>変速</summary>
        SpeedGear,
    }
    /// <summary>タイプ</summary>
    private BallType type;

    /// <summary>基本スピード</summary>
    private const float SPEEDX = 400f;
    #endregion

    #region メンバー
    /// <summary>ボール</summary>
    public SpriteRenderer image = null;
    #endregion

    #region 変数
    /// <summary>ヒット後外に出る</summary>
    private bool goOut = false;

    /// <summary>速度</summary>
    private Vector2 velocity;
    /// <summary>変速</summary>
    private DeltaFloat gear = null;
    /// <summary>加速中</summary>
    private bool accel = true;
    #endregion

    /// <summary>
    /// フレーム処理
    /// </summary>
    override public void Update()
    {
        base.Update();

        // 変速ボールの判定
        if (type == BallType.SpeedGear)
        {
            gear.Update(Time.deltaTime);
            velocity.x = gear.Get();

            if (!gear.IsActive())
            {
                if (accel)
                {
                    gear.MoveTo(SPEEDX * 0.5f, 0.3f, DeltaFloat.MoveType.BOTH);
                    accel = false;
                }
                else
                {
                    gear.MoveTo(SPEEDX * 1.5f, 0.3f, DeltaFloat.MoveType.BOTH);
                    accel = true;
                }
            }
        }

        // 位置更新
        transform.Translate(new Vector3(velocity.x, 0, 0) * Time.deltaTime);

        if (goOut)
        {
            // 退場フェーズ
            transform.Translate(new Vector3(0, -SPEEDX / 2 * Time.deltaTime, 0));
        }
        else if (type == BallType.Drift)
        {
            // 退場以外で左右移動ボールの計算
            SetFarPosition(GetFarPosition() + velocity.y * Time.deltaTime);
            if (GetFarPosition() >= ROAD_FAR_MAX || GetFarPosition() <= -ROAD_FAR_MAX)
            {
                velocity.y *= -1f;
            }
        }

        // 右まで行ったら消える
        if (transform.localPosition.x > Constant.SCREEN_WIDTH * 0.6f)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// ヒット後外に出る
    /// </summary>
    public void GoOut()
    {
        goOut = true;
        GetComponent<Collider2D>().enabled = false;
    }

    /// <summary>
    /// タイプ指定
    /// </summary>
    /// <param name="_type"></param>
    public void SetBallType(BallType _type)
    {
        type = _type;

        switch (type)
        {
            case BallType.Normal: // 通常
                velocity = new Vector2(SPEEDX, 0);
                break;
            case BallType.Drift: // 左右移動
                var pm = Util.RandomInt(0, 1) == 0 ? 1 : -1;
                velocity = new Vector2(SPEEDX * 0.8f, SPEEDX * Util.RandomFloat(0.3f, 0.5f) * pm);
                break;
            default: // 変速
                velocity = new Vector2(SPEEDX, 0);
                gear = new DeltaFloat();
                gear.Set(SPEEDX * 1f);
                accel = false;
                break;
        }
        image.color = CalcBallColor(type);
    }

    /// <summary>
    /// ボールの色
    /// </summary>
    /// <param name="_type"></param>
    /// <returns></returns>
    public static Color CalcBallColor(BallType _type)
    {
        return _type switch
        {
            BallType.Normal => Color.cyan,
            BallType.Drift => Color.yellow,
            _ => Color.magenta,
        };
    }
}

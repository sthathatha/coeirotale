using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ボスラッシュ　攻撃ボール
/// </summary>
public class PierreGameBallB : MonoBehaviour
{
    #region 定数

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

    /// <summary>移動速度</summary>
    private Vector3 speed;

    /// <summary>攻撃力</summary>
    private int power;

    /// <summary></summary>
    private bool destroyFlag = false;

    #region 基底

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        // 上に当たったらY反転または消える
        if (transform.localPosition.y >= PierreGameSystemB.FIELD_MAX_Y)
        {
            //speed.y = -Mathf.Abs(speed.y);
            if (speed.y > 0f)
            {
                Destroy(gameObject);
                return;
            }
        }

        // 移動
        transform.localPosition += speed * Time.deltaTime;

        // 現在のY座標で描画順更新
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

    #region 使用機能

    /// <summary>
    /// 生成後初期設定
    /// </summary>
    /// <param name="atk">攻撃属性（敵・味方）</param>
    /// <param name="startPos">開始位置</param>
    /// <param name="spd">速度</param>
    /// <param name="col">色</param>
    /// <param name="pow">攻撃力</param>
    public void SetParam(AttackType atk, Vector3 startPos, Vector3 spd, Color col, int pow)
    {
        transform.localPosition = startPos;
        attacktype = atk;
        speed = spd;
        model.color = col;
        power = pow;
    }

    /// <summary>
    /// 攻撃力
    /// </summary>
    /// <returns></returns>
    public int GetPower() { return power; }

    /// <summary>
    /// 
    /// </summary>
    public void DestroyWait() { destroyFlag = true; }

    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ピエール２人の共通処理
/// </summary>
public class PierreGameBPierreBase : MonoBehaviour
{
    /// <summary>システム</summary>
    public PierreGameSystemB system;

    /// <summary>腕</summary>
    public SpriteRenderer spr_arm;
    /// <summary>体</summary>
    public SpriteRenderer spr_body;
    /// <summary>足元のボール</summary>
    public SpriteRenderer spr_underBall;

    /// <summary>
    /// 開始時
    /// </summary>
    virtual protected void Start()
    {
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    virtual protected void Update()
    {
        // 現在のY座標で描画順序更新
        var so = Mathf.FloorToInt(100 - transform.position.y);

        spr_underBall.sortingOrder = so;
        spr_body.sortingOrder = so;
        spr_arm.sortingOrder = so + 1;
    }

    /// <summary>
    /// 当たり判定
    /// </summary>
    /// <param name="collision"></param>
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (system.State != PierreGameSystemB.GameState.PLAY) return;

        var ball = collision.GetComponent<PierreGameBallB>();
        OnBallHit(ball);
    }

    /// <summary>
    /// 派生先用
    /// </summary>
    /// <param name="ball"></param>
    protected virtual void OnBallHit(PierreGameBallB ball) { }

    /// <summary>
    /// 位置を取得
    /// </summary>
    /// <returns></returns>
    public Vector3 GetPos() { return transform.position; }
}

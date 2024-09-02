using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// つくよみちゃん戦　前半つくよみちゃん
/// </summary>
public class TukuyomiGameTukuyomiA : MonoBehaviour
{
    #region 定数

    public enum AnimType
    {
        /// <summary>顔が変わる前</summary>
        Before = 0,
        /// <summary>停止</summary>
        Stop,
        /// <summary>待機ループ</summary>
        Idle,
        /// <summary>下攻撃</summary>
        Down,
    }

    #endregion

    #region メンバー

    public Animator face;
    public Animator body;
    public SpriteRenderer leg;

    #endregion

    /// <summary>
    /// 初期化
    /// </summary>
    void Start()
    {
        PlayAnim(AnimType.Before);
    }

    #region メソッド

    /// <summary>
    /// アニメーション再生
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
    /// 移動
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
    /// 移動
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

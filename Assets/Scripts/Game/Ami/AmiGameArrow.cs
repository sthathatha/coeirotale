using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 矢印
/// </summary>
public class AmiGameArrow : MonoBehaviour
{
    /// <summary>開始位置Y</summary>
    public const float START_Y = 720f;
    /// <summary>終了位置Y</summary>
    public const float END_Y = -180f;
    /// <summary>生成から押すタイミングまでの時間</summary>
    public const float DELAY_TIME = 1f;
    /// <summary>終了Yまで動く時間</summary>
    private const float MOVE_TIME = DELAY_TIME * (START_Y - END_Y) / START_Y;

    /// <summary>X位置</summary>
    private float _x;
    /// <summary>Y位置</summary>
    private DeltaFloat _y;

    /// <summary>ポーズとる時間</summary>
    private float _weight;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public AmiGameArrow()
    {
        _y = new DeltaFloat();
    }

    /// <summary>
    /// 開始時
    /// </summary>
    void Start()
    {
        _x = transform.localPosition.x;
    }

    /// <summary>
    /// Time.realTimeSinceStartupがコンストラクタで使えないらしいので外から呼ぶ
    /// </summary>
    public void ForceStart()
    {
        _y.Set(START_Y);
        _y.MoveTo(END_Y, MOVE_TIME, DeltaFloat.MoveType.LINE);
    }

    /// <summary>
    /// フレーム処理
    /// </summary>
    void Update()
    {
        _y.Update(Time.deltaTime);
        transform.localPosition = new Vector3(_x, _y.Get(), 0);
    }

    /// <summary>
    /// 現在位置の絶対値
    /// </summary>
    /// <returns></returns>
    public float GetNowPositionAbs()
    {
        return Mathf.Abs(_y.Get());
    }

    /// <summary>
    /// 動作中
    /// </summary>
    /// <returns></returns>
    public bool IsActive()
    {
        return _y.Get() > 0f || _y.IsActive();
    }

    /// <summary>
    /// ポーズ時間設定
    /// </summary>
    /// <param name="w"></param>
    public void SetWeight(float w) { _weight = w; }
    /// <summary>
    /// ポーズ時間取得
    /// </summary>
    /// <returns></returns>
    public float GetWeight() { return _weight; }

    /// <summary>
    /// 最初から少し進んだ状態
    /// </summary>
    /// <param name="ofs"></param>
    public void AddOffset(float ofs)
    {
        _y.AddOffset(ofs);
    }
}

using UnityEngine;

/// <summary>
/// 時間変化する値
/// </summary>
public class DeltaFloat
{
    /// <summary>
    /// 変化タイプ
    /// </summary>
    public enum MoveType : int
    {
        /// <summary>直線</summary>
        LINE = 0,
        /// <summary>加速</summary>
        ACCEL,
        /// <summary>減速</summary>
        DECEL,
        /// <summary>加減速</summary>
        BOTH,
    }

    private MoveType moveType;
    private float startTime;
    private float endTime;

    private float beforeValue;
    private float afterValue;
    private bool active;

    public DeltaFloat()
    {
        startTime = 0;
        endTime = 0;

        active = false;
    }

    /// <summary>
    /// 移動中
    /// </summary>
    /// <returns></returns>
    public bool IsActive() { return active; }

    /// <summary>
    /// 現在値
    /// </summary>
    /// <returns></returns>
    public float Get()
    {
        if (IsActive() == false)
        {
            return afterValue;
        }

        var nowTime = Time.realtimeSinceStartup - startTime;
        if (nowTime > endTime)
        {
            active = false;
            return afterValue;
        }

        var timePer = nowTime / endTime;
        float valPer = timePer;
        switch (moveType)
        {
            case MoveType.ACCEL:
                valPer = Util.SinCurve(timePer, Constant.SinCurveType.Accel);
                break;
            case MoveType.DECEL:
                valPer = Util.SinCurve(timePer, Constant.SinCurveType.Decel);
                break;
            case MoveType.BOTH:
                valPer = Util.SinCurve(timePer, Constant.SinCurveType.Both);
                break;
        }

        return Util.CalcBetweenFloat(valPer, beforeValue, afterValue);
    }

    /// <summary>
    /// 即時変更
    /// </summary>
    /// <param name="val"></param>
    public void Set(float val)
    {
        beforeValue = val;
        afterValue = val;
        endTime = 0;
        active = false;
    }

    /// <summary>
    /// 指定時間で値を変える
    /// </summary>
    /// <param name="_val"></param>
    /// <param name="_time"></param>
    /// <param name="_moveType"></param>
    public void MoveTo(float _val, float _time, MoveType _moveType)
    {
        beforeValue = Get();

        startTime = Time.realtimeSinceStartup;

        afterValue = _val;
        endTime = _time;

        active = true;
        moveType = _moveType;
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="deltaTime"></param>
    public void Update(float deltaTime)
    {
        if (!IsActive())
        {
            return;
        }

        var nowTime = Time.realtimeSinceStartup - startTime;
        if (nowTime > endTime)
        {
            active = false;
            return;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F131　ブロック
/// </summary>
public class F131Block : ActionEventBase
{
    #region 定数

    /// <summary>1個分の座標サイズ</summary>
    public const float BLOCK_SIZE = 64f;

    /// <summary>1マス滑るのにかかる時間</summary>
    protected const float SLIP_BLOCK_TIME = 0.1f;

    #endregion

    #region 変数

    /// <summary>座標管理</summary>
    protected Vector2Int location;

    /// <summary>座標移動計算</summary>
    protected DeltaVector3 localPos = new DeltaVector3();

    /// <summary>システム</summary>
    protected F131System system;

    #endregion

    #region 基底

    /// <summary>
    /// 開始時
    /// </summary>
    public override void Start()
    {
        base.Start();

        system = fieldScript as F131System;
        //配置位置から座標計算
        location.x = Mathf.RoundToInt(transform.localPosition.x / BLOCK_SIZE);
        location.y = Mathf.RoundToInt(transform.localPosition.y / BLOCK_SIZE);
        transform.localPosition = CalcLocPosition(location);
    }

    /// <summary>
    /// ブロックイベント
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Exec()
    {
        yield break;
    }

    #endregion

    #region パブリック

    /// <summary>
    /// 座標取得
    /// </summary>
    /// <returns></returns>
    public Vector2Int GetLocation() { return location; }

    #endregion

    #region プライベート

    /// <summary>
    /// 座標からlocalPositionを計算
    /// </summary>
    /// <param name="loc"></param>
    /// <returns></returns>
    protected static Vector3 CalcLocPosition(Vector2Int loc)
    {
        var ret = new Vector3();
        ret.x = loc.x * BLOCK_SIZE;
        ret.y = loc.y * BLOCK_SIZE;
        return ret;
    }

    #endregion
}

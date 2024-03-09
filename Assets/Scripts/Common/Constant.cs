using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 定数
/// </summary>
public class Constant
{
    /// <summary>
    /// サインカーブ
    /// </summary>
    public enum SinCurveType : int
    {
        /// <summary>加速</summary>
        Accel = 0,
        /// <summary>減速</summary>
        Decel,
        /// <summary>加減速</summary>
        Both,
    }

    public const float SCREEN_WIDTH = 1280f;


}

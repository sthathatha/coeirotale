using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F101　草原入口
/// </summary>
public class F101System : MainScriptBase
{
    /// <summary>ツタフラグ　0未発見　1発見　2悠同行　3処理済み</summary>
    public const string PLANT_FLG = "F101Plant";

    #region 基底

    /// <summary>
    /// BGM
    /// </summary>
    /// <returns></returns>
    public override Tuple<SoundManager.FieldBgmType, AudioClip> GetBgm()
    {
        return new Tuple<SoundManager.FieldBgmType, AudioClip>(SoundManager.FieldBgmType.Common1, null);
    }

    #endregion
}

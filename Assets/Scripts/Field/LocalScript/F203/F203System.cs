using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 203　ラストフロア
/// </summary>
public class F203System : MainScriptBase
{
    #region 基底

    /// <summary>
    /// BGM
    /// </summary>
    /// <returns></returns>
    public override Tuple<SoundManager.FieldBgmType, AudioClip> GetBgm()
    {
        return new Tuple<SoundManager.FieldBgmType, AudioClip>(SoundManager.FieldBgmType.Common2, null);
    }

    #endregion
}

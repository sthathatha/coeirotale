using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F121　サーカス入口
/// </summary>
public class F121System : MainScriptBase
{
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

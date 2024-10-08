using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// サンプル
/// </summary>
public class SampleSceneField2 : MainScriptBase
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

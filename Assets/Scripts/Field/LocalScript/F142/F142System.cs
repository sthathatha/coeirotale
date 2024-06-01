using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F142@‹³‰ïŠOŠÏ
/// </summary>
public class F142System : MainScriptBase
{
    #region Šî’ê

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

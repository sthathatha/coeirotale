using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F151�@���H
/// </summary>
public class F151System : MainScriptBase
{
    #region ���

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

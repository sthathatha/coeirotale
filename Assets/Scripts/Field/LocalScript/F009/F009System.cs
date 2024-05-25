using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F009 ã‚Ì•ªŠò“¹
/// </summary>
public class F009System : MainScriptBase
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

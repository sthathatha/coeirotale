using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F009 ��̕���
/// </summary>
public class F009System : MainScriptBase
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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F152@¬t“ú˜a@ŠOŠÏ
/// </summary>
public class F152System : MainScriptBase
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

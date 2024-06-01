using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F111@‹´‚ÌC—
/// </summary>
public class F111System : MainScriptBase
{
    public const string BRIDGE_FLG = "BridgeRepair";

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

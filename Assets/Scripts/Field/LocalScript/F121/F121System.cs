using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F121　サーカス入口
/// </summary>
public class F121System : MainScriptBase
{
    /// <summary>鍵のフラグ　0:未発見　1:扉調べる　2:鍵発見　3:解錠済み</summary>
    public const string KEY_FLG = "F121KeyFlg";

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

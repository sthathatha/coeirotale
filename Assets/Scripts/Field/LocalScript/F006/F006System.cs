using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class F006System : MainScriptBase
{
    public const string AFTER_OPEN_TALK_FLG = "F006AfterTalk";

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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F202　瓦礫の塔
/// </summary>
public class F202System : MainScriptBase
{
    public const string SHOW_START = "F202Start";

    #region 基底

    /// <summary>
    /// BGM
    /// </summary>
    /// <returns></returns>
    public override Tuple<SoundManager.FieldBgmType, AudioClip> GetBgm()
    {
        return new Tuple<SoundManager.FieldBgmType, AudioClip>(SoundManager.FieldBgmType.Common2, null);
    }

    /// <summary>
    /// フェードイン後
    /// </summary>
    /// <returns></returns>
    public override IEnumerator AfterFadeIn(bool init)
    {
        yield return base.AfterFadeIn(init);

        if (Global.GetSaveData().GetGameDataInt(SHOW_START) < 1)
        {
            // 開始イベント
            GetComponent<F202Start>().ExecEvent();
        }
    }
    #endregion
}

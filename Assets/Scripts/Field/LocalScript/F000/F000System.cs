using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// フィールド000
/// </summary>
public class F000System : MainScriptBase
{
    #region 基底

    /// <summary>
    /// BGM
    /// </summary>
    /// <returns></returns>
    public override Tuple<SoundManager.FieldBgmType, AudioClip> GetBgm()
    {
        return new Tuple<SoundManager.FieldBgmType, AudioClip>(SoundManager.FieldBgmType.Common1, null);
    }

    /// <summary>
    /// フェードイン直前処理
    /// </summary>
    /// <returns></returns>
    public override IEnumerator BeforeFadeIn()
    {
        yield return base.BeforeFadeIn();

        if (Global.GetSaveData().GetGameDataInt("Tutorial") <= 0)
        {
            var tukuyomi = tukuyomiObj.GetComponent<TukuyomiScript>();
            tukuyomi.SetMode(TukuyomiScript.TukuyomiMode.NPC);

            tukuyomi.SetPosition(new Vector3(0, 700, 0));
        }
    }

    /// <summary>
    /// フェードイン後処理
    /// </summary>
    /// <returns></returns>
    public override IEnumerator AfterFadeIn()
    {
        yield return base.AfterFadeIn();

        if (Global.GetSaveData().GetGameDataInt("Tutorial") <= 0)
        {
            var ev = GetComponent<F000Tutorial>();
            ev.ExecEvent();
        }
    }

    #endregion
}

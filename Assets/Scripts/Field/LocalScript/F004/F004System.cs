using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F004
/// </summary>
public class F004System : MainScriptBase
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

        if (Global.GetSaveData().GetGameDataInt("Tutorial") < 3)
        {
            var tukuyomi = tukuyomiObj.GetComponent<TukuyomiScript>();
            tukuyomi.SetMode(TukuyomiScript.TukuyomiMode.NPC);

            tukuyomi.SetPosition(playerObj.transform.position + new Vector3(100, 0, 0));
            tukuyomi.SetDirection(Constant.Direction.Right);
        }
    }

    /// <summary>
    /// フェードイン後処理
    /// </summary>
    /// <returns></returns>
    public override IEnumerator AfterFadeIn()
    {
        yield return base.AfterFadeIn();

        if (Global.GetSaveData().GetGameDataInt("Tutorial") < 3)
        {
            Global.GetSaveData().SetGameData("Tutorial", 3);

            var ev = GetComponent<F004Tutorial>();
            ev.ExecEvent();
        }
    }

    #endregion
}

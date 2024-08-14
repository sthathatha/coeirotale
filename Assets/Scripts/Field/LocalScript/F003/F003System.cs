using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// フィールド003
/// </summary>
public class F003System : MainScriptBase
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
    public override IEnumerator BeforeInitFadeIn()
    {
        yield return base.BeforeInitFadeIn();

        if (Global.GetSaveData().GetGameDataInt("Tutorial") < 2)
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
    public override IEnumerator AfterFadeIn(bool init)
    {
        yield return base.AfterFadeIn(init);

        if (Global.GetSaveData().GetGameDataInt("Tutorial") < 2)
        {
            Global.GetSaveData().SetGameData("Tutorial", 2);

            var ev = GetComponent<F003Tutorial>();
            ev.ExecEvent();
        }
    }

    #endregion
}

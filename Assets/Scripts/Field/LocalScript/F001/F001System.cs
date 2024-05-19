using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// フィールド001
/// </summary>
public class F001System : MainScriptBase
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

        if (Global.GetSaveData().GetGameDataInt("Tutorial") <= 999)
        {
            var tukuyomi = tukuyomiObj.GetComponent<TukuyomiScript>();
            tukuyomi.SetMode(TukuyomiScript.TukuyomiMode.NPC);

            var gp = SearchGeneralPosition(2);
            tukuyomi.SetPosition(gp.GetPosition());
            tukuyomi.SetDirection(Constant.Direction.Down);
        }
    }

    #endregion
}

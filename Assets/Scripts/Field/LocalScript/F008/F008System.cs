using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F008
/// </summary>
public class F008System : MainScriptBase
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

    /// <summary>
    /// �t�F�[�h�C�����O����
    /// </summary>
    /// <returns></returns>
    public override IEnumerator BeforeFadeIn()
    {
        yield return base.BeforeFadeIn();

        if (Global.GetSaveData().GetGameDataInt("Tutorial") < 6)
        {
            var tukuyomi = tukuyomiObj.GetComponent<TukuyomiScript>();
            tukuyomi.SetMode(TukuyomiScript.TukuyomiMode.NPC);

            var pos = SearchGeneralPosition(2);
            tukuyomi.SetPosition(pos.GetPosition() + new Vector3(100, 0, 0));
            tukuyomi.SetDirection(Constant.Direction.Up);
        }
    }

    /// <summary>
    /// �t�F�[�h�C���㏈��
    /// </summary>
    /// <returns></returns>
    public override IEnumerator AfterFadeIn()
    {
        yield return base.AfterFadeIn();

        if (Global.GetSaveData().GetGameDataInt("Tutorial") < 6)
        {
            Global.GetSaveData().SetGameData("Tutorial", 1000);

            var ev = GetComponent<F008Tutorial>();
            ev.ExecEvent();
        }
    }

    #endregion
}

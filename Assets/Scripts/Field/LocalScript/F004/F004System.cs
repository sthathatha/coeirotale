using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F004
/// </summary>
public class F004System : MainScriptBase
{
    public const string MSG5_SHOWN = "F004Msg5Show";
    public const string MSG6_SHOWN = "F004Msg6Show";

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
    public override IEnumerator BeforeInitFadeIn()
    {
        yield return base.BeforeInitFadeIn();

        if (Global.GetSaveData().GetGameDataInt("Tutorial") < 3)
        {
            var tukuyomi = tukuyomiObj.GetComponent<TukuyomiScript>();
            tukuyomi.SetMode(TukuyomiScript.TukuyomiMode.NPC);

            tukuyomi.SetPosition(playerObj.transform.position + new Vector3(100, 0, 0));
            tukuyomi.SetDirection(Constant.Direction.Right);
        }
    }

    /// <summary>
    /// �t�F�[�h�C���㏈��
    /// </summary>
    /// <returns></returns>
    public override IEnumerator AfterFadeIn(bool init)
    {
        yield return base.AfterFadeIn(init);

        var save = Global.GetSaveData();

        // �`���[�g���A��
        if (save.GetGameDataInt("Tutorial") < 3)
        {
            save.SetGameData("Tutorial", 3);

            var ev = GetComponent<F004Tutorial>();
            ev.ExecEvent();
        }

        // �T�l�ځA�U�l�ړ|�������̉�b
        var clearCnt = save.GetABossClearCount();
        if (save.GetGameDataInt(MSG5_SHOWN) == 0 && clearCnt == 5 ||
            save.GetGameDataInt(MSG6_SHOWN) == 0 && clearCnt == 6)
        {
            GetComponent<F004Clear>().ExecEvent();
        }
    }

    #endregion
}

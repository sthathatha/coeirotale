using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 205　ラスボス部屋
/// </summary>
public class F205System : MainScriptBase
{
    public const string LAST_BATTLE_SHOWN = "F205Shown";

    public AudioClip bgm_help;

    #region 基底

    /// <summary>
    /// 初期化時
    /// </summary>
    /// <returns></returns>
    public override IEnumerator BeforeInitFadeIn()
    {
        yield return base.BeforeInitFadeIn();

        var ev = GetComponent<F205Start>();
        var skip = false;
        // 見た後はスキップチェック
        if (Global.GetSaveData().GetGameDataInt(LAST_BATTLE_SHOWN) == 1)
        {
            var msg = ManagerSceneScript.GetInstance().GetMessageWindow();
            var dialog = ManagerSceneScript.GetInstance().GetDialogWindow();

            msg.Open();
            msg.StartMessage(MessageWindow.Face.None, StringFieldMessage.F205_Lose_Skip_Dialog);
            yield return msg.WaitForMessageEnd();
            yield return dialog.OpenDialog();
            skip = dialog.GetResult() == DialogWindow.Result.Yes;
            msg.Close();
        }

        ev.InitScene(skip);
    }

    /// <summary>
    /// フェードイン前
    /// </summary>
    /// <returns></returns>
    public override IEnumerator BeforeFadeIn()
    {
        yield return base.BeforeFadeIn();

        var ev = GetComponent<F205Start>();
        ev.BackFromGame();
    }

    /// <summary>
    /// フェードイン後
    /// </summary>
    /// <returns></returns>
    public override IEnumerator AfterFadeIn(bool init)
    {
        yield return base.AfterFadeIn(init);

        if (init)
        {
            // 開始時イベント開始
            var ev = GetComponent<F205Start>();
            ev.ExecEvent();
        }
    }

    #endregion
}

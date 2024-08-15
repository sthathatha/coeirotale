using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F210　つくよみちゃんの戦闘
/// </summary>
public class F210VoiceGetTukuyomi : ActionEventBase
{
    public const string TALK_FLG = "F210TukuTalk";

    public F210GetEffect getEff;

    public AudioClip voice_before_1_tukuyomi;
    public AudioClip voice_before_2_tukuyomi;
    public AudioClip voice_before_3_tukuyomi;
    public AudioClip voice_before_4_mana;
    public AudioClip voice_before_5_tukuyomi;
    public AudioClip voice_before_6_ami;
    public AudioClip voice_before_7_mati;
    public AudioClip voice_before_8_tukuyomi;
    public AudioClip voice_before_9_pierre;
    public AudioClip voice_before_10_matuka;
    public AudioClip voice_before_11_menderu;
    public AudioClip voice_before_12_tukuyomi;
    public AudioClip voice_start_13_tukuyomi;
    public AudioClip voice_cancel_14_tukuyomi;
    public AudioClip voice_lose_15_tukuyomi;
    public AudioClip voice_retry_16_tukuyomi;
    public AudioClip voice_win_17_tukuyomi;
    public AudioClip voice_win_18_mana;
    public AudioClip voice_win_19_menderu;
    public AudioClip voice_win_20_matuka;
    public AudioClip voice_win_21_mati;
    public AudioClip voice_win_22_pierre;
    public AudioClip voice_win_23_ami;
    public AudioClip voice_win_24_tukuyomi;
    public AudioClip voice_win_25_tukuyomi;

    public AudioClip voice_reko_1;
    public AudioClip voice_reko_2;
    public AudioClip voice_reko_3;

    /// <summary>
    /// イベント
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Exec()
    {
        var save = Global.GetSaveData();
        var manager = ManagerSceneScript.GetInstance();
        var msg = manager.GetMessageWindow();
        var dlg = manager.GetDialogWindow();

        msg.Open();
        if (save.GetGameDataInt(TALK_FLG) == 1)
        {
            msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F210_TukuyomiRetry_Tukuyomi, voice_retry_16_tukuyomi);
            yield return msg.WaitForMessageEnd();
        }
        else
        {
            msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F210_TukuyomiBefore_1_Tukuyomi, voice_before_1_tukuyomi);
            yield return msg.WaitForMessageEnd();
            yield return dlg.OpenDialog();
            if (dlg.GetResult() != DialogWindow.Result.Yes)
            {
                msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F210_TukuyomiBefore_2_Tukuyomi, voice_before_2_tukuyomi);
                yield return msg.WaitForMessageEnd();
                msg.Close();
                yield break;
            }
            save.SetGameData(TALK_FLG, 1);

            msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F210_TukuyomiBefore_3_Tukuyomi, voice_before_3_tukuyomi);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Mana0, StringFieldMessage.F210_TukuyomiBefore_4_Mana, voice_before_4_mana);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F210_TukuyomiBefore_5_Tukuyomi, voice_before_5_tukuyomi);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Ami0, StringFieldMessage.F210_TukuyomiBefore_6_Ami, voice_before_6_ami);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Mati0, StringFieldMessage.F210_TukuyomiBefore_7_Mati, voice_before_7_mati);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F210_TukuyomiBefore_8_Tukuyomi, voice_before_8_tukuyomi);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Pierre0, StringFieldMessage.F210_TukuyomiBefore_9_Pierre, voice_before_9_pierre);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Matuka0, StringFieldMessage.F210_TukuyomiBefore_10_Matuka, voice_before_10_matuka);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Menderu0, StringFieldMessage.F210_TukuyomiBefore_11_Menderu, voice_before_11_menderu);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F210_TukuyomiBefore_12_Tukuyomi, voice_before_12_tukuyomi);
            yield return msg.WaitForMessageEnd();

        }
        // 確認
        yield return dlg.OpenDialog();
        if (dlg.GetResult() != DialogWindow.Result.Yes)
        {
            msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F210_TukuyomiBefore_14_Tukuyomi, voice_cancel_14_tukuyomi);
            yield return msg.WaitForMessageEnd();
            msg.Close();
            yield break;
        }
        msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F210_TukuyomiBefore_13_Tukuyomi, voice_start_13_tukuyomi);
        yield return msg.WaitForMessageEnd();
        msg.Close();

        // 戦闘開始
        manager.StartGame("GameSceneTukuyomi");
        yield return new WaitUntil(() => manager.SceneState == ManagerSceneScript.State.Main);
        msg.Open();
        if (!Global.GetTemporaryData().gameWon)
        {
            // 負け
            msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F210_TukuyomiLose_Tukuyomi, voice_lose_15_tukuyomi);
            yield return msg.WaitForMessageEnd();
            msg.Close();
            yield break;
        }

        // 勝ったあと
        msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F210_TukuyomiWin_1_Tukuyomi, voice_win_17_tukuyomi);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Mana0, StringFieldMessage.F210_TukuyomiWin_2_Mana, voice_win_18_mana);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Menderu0, StringFieldMessage.F210_TukuyomiWin_3_Menderu, voice_win_19_menderu);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Matuka0, StringFieldMessage.F210_TukuyomiWin_4_Matuka, voice_win_20_matuka);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F210_TukuyomiWin_5_Reko);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Mati0, StringFieldMessage.F210_TukuyomiWin_6_Mati, voice_win_21_mati);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Pierre0, StringFieldMessage.F210_TukuyomiWin_7_Pierre, voice_win_22_pierre);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Ami0, StringFieldMessage.F210_TukuyomiWin_8_Ami, voice_win_23_ami);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F210_TukuyomiWin_9_Reko);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F210_TukuyomiWin_10_Tukuyomi, voice_win_24_tukuyomi);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F210_TukuyomiWin_11_Reko);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F210_TukuyomiWin_12_Tukuyomi, voice_win_25_tukuyomi);
        yield return msg.WaitForMessageEnd();
        msg.Close();

        // 手に入れるエフェクト
        yield return getEff.PlayEffect(new Color(0.7f, 0.8f, 1f), fieldScript.playerObj.transform.localPosition);
        yield return new WaitForSeconds(1f);

        // 手に入れたあとの会話イベント
        Global.GetTemporaryData().ending_select_voice = F210System.VOICE_TUKUYOMI;
        fieldScript.GetComponent<F210VoiceGetSuccess>().ExecEvent();
    }
}

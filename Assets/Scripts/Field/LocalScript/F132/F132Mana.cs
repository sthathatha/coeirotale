using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F132MANA
/// </summary>
public class F132Mana : ActionEventBase
{
    #region メンバー

    public AudioClip voice_new1;
    public AudioClip voice_new3;
    public AudioClip voice_new5;
    public AudioClip voice_new7;
    public AudioClip voice_new9;

    public AudioClip voice_lose1;

    public AudioClip voice_retry1;

    public AudioClip voice_win1;
    public AudioClip voice_win2;
    public AudioClip voice_win3;
    public AudioClip voice_win4;

    #endregion

    /// <summary>負けカウント</summary>
    private int loseCount = 0;

    /// <summary>
    /// 開始時
    /// </summary>
    public override void Start()
    {
        base.Start();

        if (Global.GetSaveData().GetGameDataInt(F132System.MANA_WIN_FLG) == 1)
        {
            // クリア済みで居なくなる
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 実行
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Exec()
    {
        var manager = ManagerSceneScript.GetInstance();
        var msg = manager.GetMessageWindow();
        var save = Global.GetSaveData();

        msg.Open();
        // 開始会話
        if (save.GetGameDataInt(F132System.MANA_MEET_FLG) == 1)
        {
            msg.StartMessage(MessageWindow.Face.Mana0, StringFieldMessage.F132_Retry1_Mana, voice_retry1);
            yield return msg.WaitForMessageEnd();
        }
        else
        {
            save.SetGameData(F132System.MANA_MEET_FLG, 1);
            msg.StartMessage(MessageWindow.Face.Mana0, StringFieldMessage.F132_New1_Mana, voice_new1);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F132_New2_Reko, null);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Mana0, StringFieldMessage.F132_New3_Mana, voice_new3);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F132_New4_Reko, null);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F132_New5_Tukuyomi, voice_new5);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F132_New6_Reko, null);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Mana0, StringFieldMessage.F132_New7_Mana, voice_new7);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F132_New8_Reko, null);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F132_New9_Tukuyomi, voice_new9);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F132_New10_Reko, null);
            yield return msg.WaitForMessageEnd();
        }
        msg.Close();

        // 戦闘
        Global.GetTemporaryData().loseCount = loseCount;
        manager.StartGame("GameSceneManaA");
        yield return new WaitUntil(() => manager.SceneState == ManagerSceneScript.State.Main);

        msg.Open();
        if (Global.GetTemporaryData().gameWon)
        {
            save.SetGameData(F132System.MANA_WIN_FLG, 1);
            //勝利
            msg.StartMessage(MessageWindow.Face.Mana0, StringFieldMessage.F132_Win1_Mana, voice_win1);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Mana0, StringFieldMessage.F132_Win2_Mana, voice_win2);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F132_Win3_Tukuyomi, voice_win3);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Mana0, StringFieldMessage.F132_Win4_Mana, voice_win4);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F132_Win5_Reko, null);
            yield return msg.WaitForMessageEnd();
            msg.Close();
            manager.LoadMainScene("Field004", 4);
        }
        else
        {
            //負け
            if (loseCount < 100) loseCount++;

            msg.StartMessage(MessageWindow.Face.Mana0, StringFieldMessage.F132_Lose1_Mana, voice_lose1);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F132_Lose2_Reko, null);
            yield return msg.WaitForMessageEnd();
            msg.Close();
        }
    }
}

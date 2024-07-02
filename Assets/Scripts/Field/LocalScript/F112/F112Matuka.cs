using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F112まつかりすく
/// </summary>
public class F112Matuka : ActionEventBase
{
    #region メンバー

    public AudioClip voice_new1;
    public AudioClip voice_new3;
    public AudioClip voice_new5;
    public AudioClip voice_new7;
    public AudioClip voice_new9;
    public AudioClip voice_new10;
    public AudioClip voice_new12;
    public AudioClip voice_new13;
    public AudioClip voice_new15;
    public AudioClip voice_new16;

    public AudioClip voice_lose1;
    public AudioClip voice_lose3;

    public AudioClip voice_retry1;

    public AudioClip voice_win1;

    #endregion

    /// <summary>負けカウント</summary>
    private int loseCount = 0;

    /// <summary>
    /// 開始時
    /// </summary>
    public override void Start()
    {
        base.Start();

        if (Global.GetSaveData().GetGameDataInt(F112System.MATUKA_WIN_FLG) == 1)
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
        if (save.GetGameDataInt(F112System.MATUKA_MEET_FLG) == 1)
        {
            msg.StartMessage(MessageWindow.Face.Matuka0, StringFieldMessage.F112_Retry1_Matuka, voice_retry1);
            yield return msg.WaitForMessageEnd();
        }
        else
        {
            save.SetGameData(F112System.MATUKA_MEET_FLG, 1);
            msg.StartMessage(MessageWindow.Face.Matuka0, StringFieldMessage.F112_New1_Matuka, voice_new1);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F112_New2_Reko, null);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Matuka0, StringFieldMessage.F112_New3_Matuka, voice_new3);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F112_New4_Reko, null);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F112_New5_Tukuyomi, voice_new5);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F112_New6_Reko, null);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Matuka0, StringFieldMessage.F112_New7_Matuka, voice_new7);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F112_New8_Reko, null);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F112_New9_Tukuyomi, voice_new9);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Matuka0, StringFieldMessage.F112_New10_Matuka, voice_new10);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F112_New11_Reko, null);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F112_New12_Tukuyomi, voice_new12);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Matuka0, StringFieldMessage.F112_New13_Matuka, voice_new13);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F112_New14_Reko, null);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Matuka0, StringFieldMessage.F112_New15_Matuka, voice_new15);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F112_New16_Tukuyomi, voice_new16);
            yield return msg.WaitForMessageEnd();
        }
        msg.Close();

        // 戦闘
        Global.GetTemporaryData().loseCount = loseCount;
        manager.StartGame("GameSceneMatukaA");
        yield return new WaitUntil(() => manager.SceneState == ManagerSceneScript.State.Main);

        msg.Open();
        if (Global.GetTemporaryData().gameWon)
        {
            save.SetGameData(F112System.MATUKA_WIN_FLG, 1);
            //勝利
            msg.StartMessage(MessageWindow.Face.Matuka0, StringFieldMessage.F112_Win1_Matuka, voice_win1);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F112_Win2_Reko, null);
            yield return msg.WaitForMessageEnd();
            msg.Close();
            manager.LoadMainScene("Field004", 4);
        }
        else
        {
            //負け
            if (loseCount < 100) loseCount++;

            msg.StartMessage(MessageWindow.Face.Matuka0, StringFieldMessage.F112_Lose1_Matuka, voice_lose1);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F112_Lose2_Reko, null);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F112_Lose3_Tukuyomi, voice_lose3);
            yield return msg.WaitForMessageEnd();
            msg.Close();
        }
    }
}

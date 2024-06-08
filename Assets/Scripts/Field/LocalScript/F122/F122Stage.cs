using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F122　サーカスステージ
/// </summary>
public class F122Stage : ActionEventBase
{
    #region 定数

    #endregion

    #region メンバー

    /// <summary>ピエール</summary>
    public GameObject pierre;

    /// <summary>スポットライト</summary>
    public GameObject spotlight;

    /// <summary>ドラムロールSE</summary>
    public AudioClip drumrollSe;
    /// <summary>ジャンSE</summary>
    public AudioClip janSe;

    public AudioClip voiceNew1;
    public AudioClip voiceNew2;
    public AudioClip voiceNew3;
    public AudioClip voiceNew4;
    public AudioClip voiceNew5;
    public AudioClip voiceRetry1;
    public AudioClip voiceLose1;
    public AudioClip voiceWin1;
    public AudioClip voiceWin2;
    public AudioClip voiceWin3;
    public AudioClip voiceWin4;
    public AudioClip voiceWin5;

    #endregion

    /// <summary>
    /// 開始時
    /// </summary>
    public override void Start()
    {
        base.Start();

        if (Global.GetSaveData().GetGameDataInt(F122System.F122_PIERRE_PHASE) != 1)
        {
            pierre.SetActive(false);
            spotlight.SetActive(false);
        }
    }

    /// <summary>
    /// 実行
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Exec()
    {
        // クリア済み
        if (Global.GetSaveData().GetGameDataInt(F122System.F122_PIERRE_PHASE) >= 2)
        {
            yield break;
        }

        var manager = ManagerSceneScript.GetInstance();
        var msg = manager.GetMessageWindow();
        var sound = manager.soundMan;

        msg.Open();
        if (Global.GetSaveData().GetGameDataInt(F122System.F122_PIERRE_PHASE) == 0)
        {
            msg.StartMessage(MessageWindow.Face.Pierre0, StringFieldMessage.F122_New1_Pierre, voiceNew1);
            yield return msg.WaitForMessageEnd();
            msg.Close();
            manager.FadeOutNoWait();
            var drum = sound.PlaySELoop(drumrollSe);
            yield return new WaitForSeconds(2f);
            sound.StopLoopSE(drum);
            sound.PlaySE(janSe);
            manager.FadeInNoWait();
            pierre.SetActive(true);
            spotlight.SetActive(true);
            yield return new WaitForSeconds(1f);
            msg.Open();
            msg.StartMessage(MessageWindow.Face.Pierre0, StringFieldMessage.F122_New2_Pierre, voiceNew2);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F122_New3_Reko);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Pierre0, StringFieldMessage.F122_New4_Pierre, voiceNew3);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F122_New5_Reko);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Pierre0, StringFieldMessage.F122_New6_Pierre, voiceNew4);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F122_New7_Reko);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F122_New8_Tukuyomi, voiceNew5);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F122_New9_Reko);
            yield return msg.WaitForMessageEnd();

            Global.GetSaveData().SetGameData(F122System.F122_PIERRE_PHASE, 1);
        }
        else if (Global.GetSaveData().GetGameDataInt(F122System.F122_PIERRE_PHASE) == 1)
        {
            msg.StartMessage(MessageWindow.Face.Pierre0, StringFieldMessage.F122_Retry1_Pierre, voiceRetry1);
            yield return msg.WaitForMessageEnd();
        }
        msg.Close();

        manager.StartGame("GameScenePierreA");
        yield return new WaitUntil(() => manager.SceneState == ManagerSceneScript.State.Main);

        msg.Open();
        if (Global.GetTemporaryData().gameWon)
        {
            // 勝ち
            msg.StartMessage(MessageWindow.Face.Pierre0, StringFieldMessage.F122_Win1_Pierre, voiceWin1);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F122_Win2_Reko);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Pierre0, StringFieldMessage.F122_Win3_Pierre, voiceWin2);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F122_Win4_Reko);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F122_Win5_Tukuyomi, voiceWin3);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Pierre0, StringFieldMessage.F122_Win6_Pierre, voiceWin4);
            yield return msg.WaitForMessageEnd();
            msg.Close();
            yield return manager.FadeOut();
            pierre.SetActive(false);
            spotlight.SetActive(false);
            yield return manager.FadeIn();
            msg.Open();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F122_Win7_Reko);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F122_Win8_Tukuyomi, voiceWin5);
            yield return msg.WaitForMessageEnd();
            msg.Close();

            Global.GetSaveData().SetGameData(F122System.F122_PIERRE_PHASE, 2);
            manager.LoadMainScene("Field004", 4);
            yield break;
        }
        else
        {
            // 負け
            msg.StartMessage(MessageWindow.Face.Pierre0, StringFieldMessage.F122_Lose1_Pierre, voiceLose1);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F122_Lose2_Reko);
            yield return msg.WaitForMessageEnd();
            msg.Close();
        }
    }
}

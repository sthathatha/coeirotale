using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ã≥âÔÉÅÉìÉfÉã
/// </summary>
public class F143Menderu : ActionEventBase
{
    public AudioClip voice1;
    public AudioClip voice2;
    public AudioClip voice3;
    public AudioClip voice4;
    public AudioClip voice5;
    public AudioClip voice6;
    public AudioClip voice7;
    public AudioClip voice8;
    public AudioClip voice9;
    public AudioClip voice10;
    public AudioClip voice11;
    public AudioClip voice12;
    public AudioClip voice13;

    public AudioClip voiceWin1;
    public AudioClip voiceWin2;
    public AudioClip voiceWin3;

    public AudioClip voiceLose1;
    public AudioClip voiceLose2;

    public AudioClip voiceRetry;

    /// <summary>
    /// é¿çs
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Exec()
    {
        var manager = ManagerSceneScript.GetInstance();
        var msg = manager.GetMessageWindow();
        var chr = GetComponent<F143MenderuCharacter>();

        chr.SetDirection(Constant.Direction.Down);

        msg.Open();
        if (Global.GetSaveData().GetGameDataInt(F143System.MENDERU_MEET_FLG) == 0)
        {
            msg.StartMessage(MessageWindow.Face.Menderu0, StringFieldMessage.F143_New1_Menderu, voice1);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Menderu0, StringFieldMessage.F143_New2_Menderu, voice2);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F143_New3_Tukuyomi, voice3);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Menderu0, StringFieldMessage.F143_New4_Menderu, voice4);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F143_New5_Reko);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Menderu0, StringFieldMessage.F143_New6_Menderu, voice5);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Menderu0, StringFieldMessage.F143_New7_Menderu, voice6);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F143_New8_Tukuyomi, voice7);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Menderu0, StringFieldMessage.F143_New9_Menderu, voice8);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Menderu0, StringFieldMessage.F143_New10_Menderu, voice9);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F143_New11_Tukuyomi, voice10);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Menderu0, StringFieldMessage.F143_New12_Menderu, voice11);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F143_New13_Reko);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F143_New14_Reko);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Menderu0, StringFieldMessage.F143_New15_Menderu, voice12);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F143_New16_Tukuyomi, voice13);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F143_New17_Reko);
            yield return msg.WaitForMessageEnd();

            Global.GetSaveData().SetGameData(F143System.MENDERU_MEET_FLG, 1);
        }
        else
        {
            msg.StartMessage(MessageWindow.Face.Menderu0, StringFieldMessage.F143_Retry_Menderu, voiceRetry);
            yield return msg.WaitForMessageEnd();
        }
        msg.Close();

        var tmp = Global.GetTemporaryData();
        tmp.bossRush = false;
        manager.StartGame("GameSceneMenderuA");
        yield return new WaitWhile(() => manager.SceneState != ManagerSceneScript.State.Main);

        if (tmp.gameWon)
        {
            // èüÇ¡ÇΩÇ∆Ç´
            msg.Open();

            msg.StartMessage(MessageWindow.Face.Menderu0, StringFieldMessage.F143_Win1_Menderu, voiceWin1);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F143_Win2_Tukuyomi, voiceWin2);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Menderu0, StringFieldMessage.F143_Win3_Menderu, voiceWin3);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F143_Win4_Reko);
            yield return msg.WaitForMessageEnd();

            msg.Close();

            Global.GetSaveData().SetGameData(F143System.MENDERU_WIN_FLG, 1);
            manager.LoadMainScene("Field004", 4);
            yield break;
        }
        else
        {
            // ïâÇØÇΩÇ∆Ç´
            msg.Open();

            msg.StartMessage(MessageWindow.Face.Menderu0, StringFieldMessage.F143_Lose1_Menderu, voiceLose1);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F143_Lose2_Reko);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F143_Lose3_Tukuyomi, voiceLose2);
            yield return msg.WaitForMessageEnd();

            msg.Close();
        }
    }
}

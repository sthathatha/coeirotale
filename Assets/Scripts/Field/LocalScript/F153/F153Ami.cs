using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F153���t���a�@���t���A�~
/// </summary>
public class F153Ami : ActionEventBase
{
    #region �����o�[

    public AudioClip voice_new1;
    public AudioClip voice_new2;
    public AudioClip voice_new3;
    public AudioClip voice_new4;
    public AudioClip voice_new6;
    public AudioClip voice_new8;
    public AudioClip voice_new9;

    public AudioClip voice_lose1;
    public AudioClip voice_lose3;

    public AudioClip voice_retry1;

    public AudioClip voice_win1;

    #endregion

    private int loseCount = 0;

    /// <summary>
    /// �J�n��
    /// </summary>
    public override void Start()
    {
        base.Start();

        if (Global.GetSaveData().GetGameDataInt(F153System.AMI_WIN_FLG) == 1)
        {
            // �N���A�ς݂ŋ��Ȃ��Ȃ�
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// ���s
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Exec()
    {
        var manager = ManagerSceneScript.GetInstance();
        var msg = manager.GetMessageWindow();
        var save = Global.GetSaveData();

        msg.Open();
        // �J�n��b
        if (save.GetGameDataInt(F153System.AMI_MEET_FLG) == 1)
        {
            msg.StartMessage(MessageWindow.Face.Ami0, StringFieldMessage.F153_Retry1_Ami, voice_retry1);
            yield return msg.WaitForMessageEnd();
        }
        else
        {
            save.SetGameData(F153System.AMI_MEET_FLG, 1);
            msg.StartMessage(MessageWindow.Face.Ami0, StringFieldMessage.F153_New1_Ami, voice_new1);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F153_New2_Tukuyomi, voice_new2);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F153_New3_Tukuyomi, voice_new3);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Ami0, StringFieldMessage.F153_New4_Ami, voice_new4);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F153_New5_Reko, null);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Ami0, StringFieldMessage.F153_New6_Ami, voice_new6);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F153_New7_Reko, null);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Ami0, StringFieldMessage.F153_New8_Ami, voice_new8);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F153_New9_Tukuyomi, voice_new9);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F153_New10_Reko, null);
            yield return msg.WaitForMessageEnd();
        }
        msg.Close();

        // �퓬
        Global.GetTemporaryData().loseCount = loseCount;
        manager.StartGame("GameSceneAmiA");
        yield return new WaitUntil(() => manager.SceneState == ManagerSceneScript.State.Main);

        msg.Open();
        if (Global.GetTemporaryData().gameWon)
        {
            save.SetGameData(F153System.AMI_WIN_FLG, 1);
            //����
            msg.StartMessage(MessageWindow.Face.Ami0, StringFieldMessage.F153_Win1_Ami, voice_win1);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F153_Win2_Reko, null);
            yield return msg.WaitForMessageEnd();
            msg.Close();
            manager.LoadMainScene("Field004", 4);
        }
        else
        {
            //����
            if (loseCount < 100) loseCount++;

            msg.StartMessage(MessageWindow.Face.Ami0, StringFieldMessage.F153_Lose1_Ami, voice_lose1);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F153_Lose2_Reko, null);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F153_Lose3_Tukuyomi, voice_lose3);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F153_Lose4_Reko, null);
            yield return msg.WaitForMessageEnd();
            msg.Close();
        }
    }
}

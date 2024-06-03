using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F121 ����
/// </summary>
public class F121KeyBlock : ActionEventBase
{
    /// <summary>���t���O�̃Z�[�u�f�[�^</summary>
    public const string KEY_FLG = "PierreKeyPhase";

    /// <summary>�v���C���[</summary>
    public PlayerScript player;

    public AudioClip voice1;
    public AudioClip voice2;

    public AudioClip openSe;

    /// <summary>
    /// �J�n��
    /// </summary>
    public override void Start()
    {
        base.Start();

        // �����ς݂͊J�n���ɏ���
        if (Global.GetSaveData().GetGameDataInt(KEY_FLG) >= 2)
        {
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// ���s
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Exec()
    {
        // ���������Ă��Ȃ��ꍇ���b�Z�[�W
        if (Global.GetSaveData().GetGameDataInt(KEY_FLG) == 0)
        {
            var msg = ManagerSceneScript.GetInstance().GetMessageWindow();
            msg.Open();

            msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F121_1_Tukuyomi, voice1);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F121_2_Reko);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F121_3_Tukuyomi, voice2);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F121_4_Reko);
            yield return msg.WaitForMessageEnd();
            msg.Close();

            yield break;
        }

        // �����Ă�����J����
        Global.GetSaveData().SetGameData(KEY_FLG, 2);
        ManagerSceneScript.GetInstance().soundMan.PlaySE(openSe);
        player.RemoveActionEvent(this);
        gameObject.SetActive(false);
    }
}

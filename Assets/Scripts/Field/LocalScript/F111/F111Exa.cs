using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �G�O�U�@���̏C��
/// </summary>
public class F111Exa : ActionEventBase
{
    public GameObject bridge_Broken;
    public GameObject bridge_Repair;
    public GameObject logs;
    public GameObject chair;

    public AudioClip voice_new1;
    public AudioClip voice_new2;
    public AudioClip voice_new3;
    public AudioClip voice_1;
    public AudioClip se_Repair1;
    public AudioClip se_Repair2;

    /// <summary>
    /// �J�n��
    /// </summary>
    public override void Start()
    {
        base.Start();

        if (Global.GetSaveData().GetGameDataInt(F111System.BRIDGE_FLG) >= 2)
        {
            bridge_Broken.SetActive(false);
            bridge_Repair.SetActive(true);
            logs.SetActive(false);
            chair.SetActive(false);
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// ��b
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Exec()
    {
        var msg = ManagerSceneScript.GetInstance().GetMessageWindow();
        var sound = ManagerSceneScript.GetInstance().soundMan;

        // �ŏ�
        if (Global.GetSaveData().GetGameDataInt(F111System.BRIDGE_FLG) == 0)
        {
            msg.Open();
            msg.StartMessage(MessageWindow.Face.Exa0, StringFieldMessage.F111_New1_Exa);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F111_New2_Tukuyomi, voice_new1);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Exa0, StringFieldMessage.F111_New3_Exa);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Exa0, StringFieldMessage.F111_New4_Exa);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F111_New5_Tukuyomi, voice_new2);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Exa0, StringFieldMessage.F111_New6_Exa);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Exa0, StringFieldMessage.F111_New7_Exa);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Exa0, StringFieldMessage.F111_New8_Exa);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F111_New9_Reko);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F111_New10_Tukuyomi, voice_new3);
            yield return msg.WaitForMessageEnd();
            msg.Close();

            Global.GetSaveData().SetGameData(F111System.BRIDGE_FLG, 1);
            yield break;
        }

        // �C���ς݂Řb��������
        if (Global.GetSaveData().GetGameDataInt(F111System.BRIDGE_FLG) >= 2)
        {
            msg.Open();
            msg.StartMessage(MessageWindow.Face.Exa0, StringFieldMessage.F111_5_Exa);
            yield return msg.WaitForMessageEnd();
            msg.Close();
            yield break;
        }

        // ���[�v��n��
        msg.Open();
        msg.StartMessage(MessageWindow.Face.Exa0, StringFieldMessage.F111_1_Exa);
        yield return msg.WaitForMessageEnd();
        if (Global.GetSaveData().GetGameDataInt(F151BoardSource.BOARD_USE_FLG) >= 1)
        {
            msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F111_2_Tukuyomi, voice_1);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Exa0, StringFieldMessage.F111_3_Exa);
            yield return msg.WaitForMessageEnd();
            msg.Close();

            // �t�F�[�h�A�E�g����SE�Đ��A�ޗ��Ƌ��̕\���؂�ւ�
            yield return ManagerSceneScript.GetInstance().FadeOut();
            sound.PlaySE(se_Repair1);
            yield return new WaitForSeconds(2.8f);
            sound.PlaySE(se_Repair2);
            yield return new WaitForSeconds(0.5f);
            bridge_Broken.SetActive(false);
            bridge_Repair.SetActive(true);
            logs.SetActive(false);
            Global.GetSaveData().SetGameData(F111System.BRIDGE_FLG, 2);

            yield return ManagerSceneScript.GetInstance().FadeIn();

            msg.Open();
            msg.StartMessage(MessageWindow.Face.Exa0, StringFieldMessage.F111_4_Exa);
            yield return msg.WaitForMessageEnd();
        }
        msg.Close();
    }
}

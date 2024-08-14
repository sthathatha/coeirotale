using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F210�@�������炤�C�x���g
/// </summary>
public class F210VoiceGet : ActionEventBase
{
    /// <summary>�L����</summary>
    public int index;

    /// <summary>
    /// �C�x���g
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Exec()
    {
        var manager = ManagerSceneScript.GetInstance();
        var msg = manager.GetMessageWindow();
        var dlg = manager.GetDialogWindow();
        var reko = fieldScript.playerObj.GetComponent<PlayerScript>();
        var tukuyomi = fieldScript.tukuyomiObj.GetComponent<TukuyomiScript>();

        msg.Open();
        if (index == F210System.VOICE_TUKUYOMI)
        {
            // ����݂����͐퓬����
        }
        else
        {
            if (index == F210System.VOICE_AMI)// �A�~
                msg.StartMessage(MessageWindow.Face.Ami0, StringFieldMessage.F210_Get_Ami_1);
            else if (index == F210System.VOICE_MENDERU)// �����f��
                msg.StartMessage(MessageWindow.Face.Menderu0, StringFieldMessage.F210_Get_Menderu_1);
            else if (index == F210System.VOICE_MATUKA)// �܂��肷��
                msg.StartMessage(MessageWindow.Face.Matuka0, StringFieldMessage.F210_Get_Matuka_1);
            else if (index == F210System.VOICE_MATI)// �}�`
                msg.StartMessage(MessageWindow.Face.Mati0, StringFieldMessage.F210_Get_Mati_1);
            else if (index == F210System.VOICE_PIERRE)// �s�G�[��
                msg.StartMessage(MessageWindow.Face.Pierre0, StringFieldMessage.F210_Get_Pierre_1);
            else if (index == F210System.VOICE_MANA)// MANA
                msg.StartMessage(MessageWindow.Face.Mana0, StringFieldMessage.F210_Get_Mana_1);
            yield return msg.WaitForMessageEnd();
            yield return dlg.OpenDialog();
            if (dlg.GetResult() != DialogWindow.Result.Yes) yield break;

            // �G�t�F�N�g

            if (index == F210System.VOICE_AMI)// �A�~
                msg.StartMessage(MessageWindow.Face.Ami0, StringFieldMessage.F210_Get_Ami_2);
            else if (index == F210System.VOICE_MENDERU)// �����f��
                msg.StartMessage(MessageWindow.Face.Menderu0, StringFieldMessage.F210_Get_Menderu_2);
            else if (index == F210System.VOICE_MATUKA)// �܂��肷��
                msg.StartMessage(MessageWindow.Face.Matuka0, StringFieldMessage.F210_Get_Matuka_2);
            else if (index == F210System.VOICE_MATI)// �}�`
                msg.StartMessage(MessageWindow.Face.Mati0, StringFieldMessage.F210_Get_Mati_2);
            else if (index == F210System.VOICE_PIERRE)// �s�G�[��
                msg.StartMessage(MessageWindow.Face.Pierre0, StringFieldMessage.F210_Get_Pierre_2);
            else if (index == F210System.VOICE_MANA)// MANA
                msg.StartMessage(MessageWindow.Face.Mana0, StringFieldMessage.F210_Get_Mana_2);
            yield return msg.WaitForMessageEnd();
            msg.Close();
        }

        // ����
        yield return new WaitForSeconds(1f);
        msg.Open();

        msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F210_Exit_1_Reko, null);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Mana0, StringFieldMessage.F210_Exit_2_Tukuyomi, null);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Mana0, StringFieldMessage.F210_Exit_3_Reko, null);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Mana0, StringFieldMessage.F210_Exit_4_Tukuyomi, null);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Mana0, StringFieldMessage.F210_Exit_5_Mana, null);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Mana0, StringFieldMessage.F210_Exit_6_Matuka, null);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Mana0, StringFieldMessage.F210_Exit_7_Pierre, null);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Mana0, StringFieldMessage.F210_Exit_8_Menderu, null);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Mana0, StringFieldMessage.F210_Exit_9_Ami, null);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Mana0, StringFieldMessage.F210_Exit_10_Mati, null);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Mana0, StringFieldMessage.F210_Exit_11_Reko, null);
        yield return msg.WaitForMessageEnd();

        

        msg.Close();
        yield return new WaitForSeconds(1f);
        Global.GetTemporaryData().ending_select_voice = index;
        fieldScript.GetComponent<F210ExitEvent>().ExecEvent();
    }
}

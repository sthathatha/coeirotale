using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F006�@�G����b
/// </summary>
public class F006Eraps : SimpleMessageEvent
{

    public AudioClip voice1;

    /// <summary>
    /// ���b�Z�[�W
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    protected override bool ShowMessage(MessageWindow msg, int index)
    {
        var save = Global.GetSaveData();

        if (save.GetGameDataInt(F201System.DUNGEON_OPEN_FLG) >= 1)
        {
            if (save.GetGameDataInt(F006System.AFTER_OPEN_TALK_FLG) >= 1)
            { // �}�b�v�\����2��ڈȍ~�͍Ō�̃Z���t�̂�
                index += 5;
            }

            switch (index)
            {
                case 0:
                    msg.StartMessage(MessageWindow.Face.Eraps0, StringFieldMessage.F006_Opened_1_Eraps);
                    return true;
                case 1:
                    msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F006_Opened_2_Reko);
                    return true;
                case 2:
                    msg.StartMessage(MessageWindow.Face.Eraps0, StringFieldMessage.F006_Opened_3_Eraps);
                    return true;
                case 3:
                    msg.StartMessage(MessageWindow.Face.Eraps0, StringFieldMessage.F006_Opened_4_Eraps);
                    return true;
                case 4:
                    msg.StartMessage(MessageWindow.Face.Eraps0, StringFieldMessage.F006_Opened_5_Eraps);
                    return true;
                case 5:
                    msg.StartMessage(MessageWindow.Face.Eraps0, StringFieldMessage.F006_Opened_6_Eraps);
                    save.SetGameData(F006System.AFTER_OPEN_TALK_FLG, 1);
                    return true;
            }
        }
        else
        {
            if (viewCount > 0)
            {
                // 2��ڈȍ~�͍ŏ���1��΂�
                index++;
            }

            switch (index)
            {
                case 0:
                    msg.StartMessage(MessageWindow.Face.Eraps0, StringFieldMessage.F006_1_ErapsNew);
                    return true;
                case 1:
                    msg.StartMessage(MessageWindow.Face.Eraps0, StringFieldMessage.F006_2_Eraps);
                    return true;
                case 2:
                    msg.StartMessage(MessageWindow.Face.Eraps0, StringFieldMessage.F006_3_Eraps);
                    return true;
                case 3:
                    msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F006_4_Reko);
                    return true;
                case 4:
                    msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F006_5_Tukuyomi, voice1);
                    return true;
                case 5:
                    msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F006_6_Reko);
                    return true;
            }
        }

        return false;
    }
}

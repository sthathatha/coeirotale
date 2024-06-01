using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F006　エラ会話
/// </summary>
public class F006Eraps : SimpleMessageEvent
{

    public AudioClip voice1;

    /// <summary>
    /// メッセージ
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    protected override bool ShowMessage(MessageWindow msg, int index)
    {
        if (viewCount > 0)
        {
            // 2回目以降は最初の1個飛ばす
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

        return false;
    }
}

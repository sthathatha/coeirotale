using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F141立て札
/// </summary>
public class F141Board : SimpleMessageEvent
{
    /// <summary>
    /// メッセージ表示
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    protected override bool ShowMessage(MessageWindow msg, int index)
    {
        switch (index)
        {
            case 0:
                msg.StartMessage(MessageWindow.Face.None, StringFieldMessage.F141_Board1);
                return true;
            case 1:
                msg.StartMessage(MessageWindow.Face.None, StringFieldMessage.F141_Board2);
                return true;
            case 2:
                msg.StartMessage(MessageWindow.Face.None, StringFieldMessage.F141_Board3);
                return true;
            case 3:
                msg.StartMessage(MessageWindow.Face.None, StringFieldMessage.F141_Board4);
                return true;
        }

        return false;
    }
}

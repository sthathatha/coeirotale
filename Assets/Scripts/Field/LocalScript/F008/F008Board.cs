using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F008看板
/// </summary>
public class F008Board : SimpleMessageEvent
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
                msg.StartMessage(MessageWindow.Face.None2, StringFieldMessage.F008_Board);
                return true;
        }

        return false;
    }
}

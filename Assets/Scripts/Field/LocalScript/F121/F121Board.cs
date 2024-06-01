using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F121　サーカス前看板
/// </summary>
public class F121Board : SimpleMessageEvent
{
    public int id;

    /// <summary>
    /// メッセージ
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    protected override bool ShowMessage(MessageWindow msg, int index)
    {
        if (index == 0)
        {
            msg.StartMessage(MessageWindow.Face.None, id switch
            {
                0 => StringFieldMessage.F121_Board1,
                1 => StringFieldMessage.F121_Board2,
                2 => StringFieldMessage.F121_Board3,
                _ => StringFieldMessage.F121_Board4,
            });

            return true;
        }

        return false;
    }
}

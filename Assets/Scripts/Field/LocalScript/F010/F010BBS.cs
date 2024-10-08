using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F010@ºÌªòÌÅÂ
/// </summary>
public class F010BBS : SimpleMessageEvent
{
    /// <summary>}bvàID</summary>
    public int id = 0;

    /// <summary>
    /// ÅÂ
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    protected override bool ShowMessage(MessageWindow msg, int index)
    {
        if (index != 0)
        {
            return false;
        }

        switch (id)
        {
            case 0:
                msg.StartMessage(MessageWindow.Face.None, StringFieldMessage.F010_Board1);
                break;
            case 1:
                msg.StartMessage(MessageWindow.Face.None, StringFieldMessage.F010_Board2);
                break;
            case 2:
                msg.StartMessage(MessageWindow.Face.None, StringFieldMessage.F010_Board3);
                break;
        }

        return true;
    }
}

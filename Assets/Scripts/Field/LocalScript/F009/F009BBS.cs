using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F009�@��̕���̊Ŕ�
/// </summary>
public class F009BBS : SimpleMessageEvent
{
    /// <summary>�}�b�v��ID</summary>
    public int id = 0;

    /// <summary>
    /// �Ŕ�
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
                msg.StartMessage(MessageWindow.Face.None, StringFieldMessage.F009_Board1);
                break;
            case 1:
                msg.StartMessage(MessageWindow.Face.None, StringFieldMessage.F009_Board2);
                break;
            case 2:
                msg.StartMessage(MessageWindow.Face.None, StringFieldMessage.F009_Board3);
                break;
        }

        return true;
    }
}

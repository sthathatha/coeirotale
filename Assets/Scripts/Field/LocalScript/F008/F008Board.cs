using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F008�Ŕ�
/// </summary>
public class F008Board : SimpleMessageEvent
{
    /// <summary>
    /// ���b�Z�[�W�\��
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

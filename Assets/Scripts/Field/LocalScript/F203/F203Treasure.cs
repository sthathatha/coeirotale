using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F203�@�_����
/// </summary>
public class F203Treasure : SimpleMessageEvent
{
    public AudioClip voice1;
    public AudioClip voice2;

    /// <summary>
    /// ��
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    protected override bool ShowMessage(MessageWindow msg, int index)
    {
        if (Global.GetSaveData().GetGameDataInt(F204System.WALL_OPEN_FLG) >= 1)
        {
            // �Ō�̕ǊJ�������Ƃ͈�l
            if (index == 0)
            {
                msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F203_TreasureB_1_Reko);
                return true;
            }
        }
        else
        {
            // ����݂����Ɖ�b
            switch (index)
            {
                case 0:
                    msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F203_TreasureA_1_Tukuyomi, voice1);
                    return true;
                case 1:
                    msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F203_TreasureA_2_Reko);
                    return true;
                case 2:
                    msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F203_TreasureA_3_Tukuyomi, voice2);
                    return true;
                case 3:
                    msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F203_TreasureA_4_Reko);
                    return true;
            }
        }

        return false;
    }
}

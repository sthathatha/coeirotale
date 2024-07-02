using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F202��b�C�x���g
/// </summary>
public class F202Characters : SimpleMessageEvent
{
    public int characterId;
    public AudioClip voice1;

    /// <summary>
    /// �J�n��
    /// </summary>
    public override void Start()
    {
        base.Start();

        // �Ō�̕ǂ��J���Ə�����
        if (Global.GetSaveData().GetGameDataInt(F204System.WALL_OPEN_FLG) >= 1)
        {
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// ��b
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    protected override bool ShowMessage(MessageWindow msg, int index)
    {
        if (index == 0)
        {
            switch (characterId)
            {
                case 0: // �܂��肷��
                    msg.StartMessage(MessageWindow.Face.Matuka0, StringFieldMessage.F202_Matuka_1, voice1);
                    break;
                case 1: // �s�G�[��
                    msg.StartMessage(MessageWindow.Face.Pierre0, StringFieldMessage.F202_Pierre_1, voice1);
                    break;
                case 2: // �����f��
                    msg.StartMessage(MessageWindow.Face.Menderu0, StringFieldMessage.F202_Menderu_1, voice1);
                    break;
            }
            return true;
        }

        return false;
    }
}

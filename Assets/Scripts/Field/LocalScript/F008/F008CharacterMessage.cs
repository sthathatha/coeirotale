using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F008　バリア前　マチ
/// </summary>
public class F008CharacterMessage : SimpleMessageEvent
{
    public string charaName;
    public AudioClip voice0 = null;

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
            switch (charaName)
            {
                case "drows":
                    msg.StartMessage(MessageWindow.Face.Drows0, StringFieldMessage.F008_Normal_Drows, voice0);
                    break;
                case "exa":
                    msg.StartMessage(MessageWindow.Face.Exa0, StringFieldMessage.F008_Normal_Exa, voice0);
                    break;
                case "worra":
                    msg.StartMessage(MessageWindow.Face.Worra0, StringFieldMessage.F008_Normal_Worra, voice0);
                    break;
                case "koob":
                    msg.StartMessage(MessageWindow.Face.Koob0, StringFieldMessage.F008_Normal_Koob, voice0);
                    break;
                case "you":
                    msg.StartMessage(MessageWindow.Face.You0, StringFieldMessage.F008_Normal_You, voice0);
                    break;

                case "mati":
                    msg.StartMessage(MessageWindow.Face.Mati0, StringFieldMessage.F008_Normal_Mati, voice0);
                    break;
                case "matuka":
                    msg.StartMessage(MessageWindow.Face.Matuka0, StringFieldMessage.F008_Normal_Matuka, voice0);
                    break;
                case "pierre":
                    msg.StartMessage(MessageWindow.Face.Pierre0, StringFieldMessage.F008_Normal_Pierre, voice0);
                    break;
                case "mana":
                    msg.StartMessage(MessageWindow.Face.Mana0, StringFieldMessage.F008_Normal_Mana, voice0);
                    break;
                case "menderu":
                    msg.StartMessage(MessageWindow.Face.Menderu0, StringFieldMessage.F008_Normal_Menderu, voice0);
                    break;
                case "ami":
                    msg.StartMessage(MessageWindow.Face.Ami0, StringFieldMessage.F008_Normal_Ami, voice0);
                    break;
            }

            return true;
        }

        return false;
    }
}

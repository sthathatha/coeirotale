using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F004　ステージクリアで戻ってきた時の会話
/// </summary>
public class F004Clear : EventBase
{
    #region メンバー

    public AudioClip voice5_1;
    public AudioClip voice5_3;
    public AudioClip voice5_5;
    public AudioClip voice5_7;

    public AudioClip voice6_2;

    #endregion

    protected override IEnumerator Exec()
    {
        var msg = ManagerSceneScript.GetInstance().GetMessageWindow();
        var count = Global.GetSaveData().GetABossClearCount();

        msg.Open();
        if (count == 5)
        {
            msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F004_Clear5_1_Tukuyomi, voice5_1);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F004_Clear5_2_Reko);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F004_Clear5_3_Tukuyomi, voice5_3);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F004_Clear5_4_Reko);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F004_Clear5_5_Tukuyomi, voice5_5);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F004_Clear5_6_Reko);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F004_Clear5_7_Tukuyomi, voice5_7);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F004_Clear5_8_Reko);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F004_Clear5_9_Reko);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F004_Clear5_10_Reko);
            yield return msg.WaitForMessageEnd();
            Global.GetSaveData().SetGameData(F004System.MSG5_SHOWN, 1);
        }
        else
        {
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F004_Clear6_1_Reko);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F004_Clear6_2_Tukuyomi, voice6_2);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F004_Clear6_3_Reko);
            yield return msg.WaitForMessageEnd();
            Global.GetSaveData().SetGameData(F004System.MSG6_SHOWN, 1);
        }
        msg.Close();
    }
}

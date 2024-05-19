using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class F001Tukuyomi : ActionEventBase
{
    #region ボイス

    public AudioClip serif0;
    public AudioClip serif1;
    public AudioClip serif2;

    #endregion

    /// <summary>
    /// 実行
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Exec()
    {
        var msg = ManagerSceneScript.GetInstance().GetMessageWindow();

        msg.Open();
        msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.Tutorial001_01Tukuyomi, serif0);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.Tutorial001_02Reko);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.Tutorial001_03Tukuyomi, serif1);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.Tutorial001_04Reko);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.Tutorial001_05Tukuyomi, serif2);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.Tutorial001_06Reko);
        yield return msg.WaitForMessageEnd();

        msg.Close();

    }
}

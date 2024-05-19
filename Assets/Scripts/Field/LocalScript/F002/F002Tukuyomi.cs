using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class F002Tukuyomi : ActionEventBase
{
    #region ボイス

    public AudioClip serif0;
    public AudioClip serif1;

    #endregion

    /// <summary>
    /// 実行
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Exec()
    {
        var msg = ManagerSceneScript.GetInstance().GetMessageWindow();

        msg.Open();
        msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.Tutorial002_01Tukuyomi, serif0);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.Tutorial002_02Reko);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.Tutorial002_03Tukuyomi, serif1);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.Tutorial002_04Reko);
        yield return msg.WaitForMessageEnd();

        msg.Close();

    }
}

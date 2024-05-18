using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F000開始時オープニング会話
/// </summary>
public class F000StartEvent : EventBase
{
    #region メンバー

    /// <summary>つくよみちゃん</summary>
    public TukuyomiScript tukuyomiObj;

    #region ボイス

    public AudioClip serif0;
    public AudioClip serif1;
    public AudioClip serif2;
    public AudioClip serif3;
    public AudioClip serif4;
    public AudioClip serif5;
    public AudioClip serif6;
    public AudioClip serif7;
    public AudioClip serif8;
    public AudioClip serif9;
    public AudioClip serif10;
    public AudioClip serif11;

    #endregion

    #endregion

    /// <summary>
    /// イベント内容
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Exec()
    {
        var msg = ManagerSceneScript.GetInstance().GetMessageWindow();

        yield return new WaitForSeconds(1f);
        tukuyomiObj.WalkTo(new Vector3(0, 100, 0));

        msg.Open();
        msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.Tutorial000_01Reko);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.Tutorial000_02Tukuyomi, serif0);
        yield return msg.WaitForMessageEnd();
        //msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.Tutorial000_03Reko);
        //yield return msg.WaitForMessageEnd();
        //msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.Tutorial000_04Tukuyomi, serif1);
        //yield return msg.WaitForMessageEnd();
        //msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.Tutorial000_05Tukuyomi, serif2);
        //yield return msg.WaitForMessageEnd();
        //msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.Tutorial000_06Tukuyomi, serif3);
        //yield return msg.WaitForMessageEnd();
        //msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.Tutorial000_07Tukuyomi, serif4);
        //yield return msg.WaitForMessageEnd();
        //msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.Tutorial000_08Tukuyomi, serif5);
        //yield return msg.WaitForMessageEnd();
        //msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.Tutorial000_09Reko);
        //yield return msg.WaitForMessageEnd();
        //msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.Tutorial000_10Tukuyomi, serif6);
        //yield return msg.WaitForMessageEnd();
        //msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.Tutorial000_11Tukuyomi, serif7);
        //yield return msg.WaitForMessageEnd();
        //msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.Tutorial000_12Reko);
        //yield return msg.WaitForMessageEnd();
        //msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.Tutorial000_13Tukuyomi, serif8);
        //yield return msg.WaitForMessageEnd();
        //msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.Tutorial000_14Tukuyomi, serif9);
        //yield return msg.WaitForMessageEnd();
        //msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.Tutorial000_15Reko);
        //yield return msg.WaitForMessageEnd();
        //msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.Tutorial000_16Tukuyomi, serif10);
        //yield return msg.WaitForMessageEnd();

        //msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.Tutorial000_17Tukuyomi, serif11);
        //yield return msg.WaitForMessageEnd();
        //msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.Tutorial000_18Reko);
        //yield return msg.WaitForMessageEnd();

        msg.Close();

        Global.GetSaveData().SetGameData("Tutorial", 1);

        tukuyomiObj.SetMode(TukuyomiScript.TukuyomiMode.Trace);
    }
}

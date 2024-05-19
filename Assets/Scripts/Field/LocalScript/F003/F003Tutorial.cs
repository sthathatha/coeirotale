using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F003チュートリアル
/// </summary>
public class F003Tutorial : EventBase
{
    public TukuyomiScript tukuyomi;
    public PlayerScript player;

    #region ボイス

    public AudioClip serif0;
    public AudioClip serif1;
    public AudioClip serif2;
    public AudioClip serif3;
    public AudioClip serif4;

    #endregion

    /// <summary>
    /// 実行
    /// </summary>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    protected override IEnumerator Exec()
    {
        var msg = ManagerSceneScript.GetInstance().GetMessageWindow();

        var posCenter = fieldScript.SearchGeneralPosition(2);
        tukuyomi.WalkTo(posCenter.GetPosition() + new Vector3(100, 0, 0));
        yield return new WaitForSeconds(0.4f);
        player.WalkTo(posCenter.GetPosition());
        yield return new WaitForSeconds(0.5f);

        msg.Open();
        msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.Tutorial003_01Tukuyomi, serif0);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.Tutorial003_02Reko);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.Tutorial003_03Tukuyomi, serif1);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.Tutorial003_04Reko);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.Tutorial003_05Tukuyomi, serif2);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.Tutorial003_06Reko);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.Tutorial003_07Tukuyomi, serif3);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.Tutorial003_08Reko);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.Tutorial003_09Tukuyomi, serif4);
        yield return msg.WaitForMessageEnd();

        msg.Close();

        var posRight = fieldScript.SearchGeneralPosition(1).GetPosition() + new Vector3(1000,0,0);
        tukuyomi.WalkTo(posRight);
        yield return new WaitForSeconds(0.5f);
        player.WalkTo(posRight);
        yield return new WaitForSeconds(1f);

        // 自動で次のマップへ
        ManagerSceneScript.GetInstance().LoadMainScene("Field004", 0);
    }
}

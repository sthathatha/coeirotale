using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F004チュートリアル
/// </summary>
public class F004Tutorial : EventBase
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

        var posCenter = fieldScript.SearchGeneralPosition(4);
        tukuyomi.WalkTo(posCenter.GetPosition() + new Vector3(100, 0, 0));
        yield return new WaitForSeconds(0.4f);
        player.WalkTo(posCenter.GetPosition());
        yield return new WaitForSeconds(0.5f);

        msg.Open();
        msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.Tutorial004_01Tukuyomi, serif0);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.Tutorial004_02Tukuyomi, serif1);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.Tutorial004_03Reko);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.Tutorial004_04Tukuyomi, serif2);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.Tutorial004_05Tukuyomi, serif3);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.Tutorial004_06Reko);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.Tutorial004_07Tukuyomi, serif4);
        yield return msg.WaitForMessageEnd();

        msg.Close();

        var posRight = fieldScript.SearchGeneralPosition(1).GetPosition() + new Vector3(1000, 0, 0);
        tukuyomi.WalkTo(posRight);
        yield return new WaitForSeconds(0.5f);
        player.WalkTo(posRight);
        yield return new WaitForSeconds(1f);

        // 自動で次のマップへ
        ManagerSceneScript.GetInstance().LoadMainScene("Field005", 0);
    }
}

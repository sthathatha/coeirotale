using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F008チュートリアル
/// </summary>
public class F008Tutorial : EventBase
{
    public TukuyomiScript tukuyomi;
    public PlayerScript player;

    #region ボイス

    public AudioClip serif0;
    public AudioClip serif1;
    public AudioClip serif2;
    public AudioClip serif3;
    public AudioClip serif4;
    public AudioClip serif5;

    #endregion

    /// <summary>
    /// 実行
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Exec()
    {
        var msg = ManagerSceneScript.GetInstance().GetMessageWindow();

        var posCenter = fieldScript.SearchGeneralPosition(2);
        player.WalkTo(posCenter.GetPosition());
        yield return new WaitForSeconds(0.5f);

        msg.Open();
        msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.Tutorial008_01Tukuyomi, serif0);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.Tutorial008_02Reko);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.Tutorial008_03Tukuyomi, serif1);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.Tutorial008_04Reko);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.Tutorial008_05Tukuyomi, serif2);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.Tutorial008_06Reko);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.Tutorial008_07Tukuyomi, serif3);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.Tutorial008_08Reko);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.Tutorial008_09Reko);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.Tutorial008_10Tukuyomi, serif4);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.Tutorial008_11Reko);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.Tutorial008_12Tukuyomi, serif5);
        yield return msg.WaitForMessageEnd();

        msg.Close();

        tukuyomi.WalkTo(tukuyomi.transform.position + new Vector3(0, -1000, 0));
        player.WalkTo(player.transform.position + new Vector3(0, -1000, 0));
        yield return new WaitForSeconds(1f);

        // 自動で次のマップへ
        ManagerSceneScript.GetInstance().LoadMainScene("Field004", 1);
    }
}

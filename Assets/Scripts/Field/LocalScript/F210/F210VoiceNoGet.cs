using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F210 声を貰わずクリアする
/// </summary>
public class F210VoiceNoGet : AreaEventBase
{
    public PlayerScript reko;

    /// <summary>
    /// イベント
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Exec()
    {
        var manager = ManagerSceneScript.GetInstance();
        var msg = manager.GetMessageWindow();
        var dlg = manager.GetDialogWindow();
        var reko = fieldScript.playerObj.GetComponent<PlayerScript>();

        msg.Open();
        msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F210_Not_1_Tukuyomi, null);
        yield return msg.WaitForMessageEnd();
        yield return dlg.OpenDialog();
        if (dlg.GetResult() != DialogWindow.Result.Yes)
        {
            // キャンセル
            reko.WalkTo(reko.transform.localPosition - new Vector3(0, 64f));
            yield return new WaitWhile(() => reko.IsWalking());
            yield break;
        }

        // 会話してエンディングへ
        msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F210_Not_2_Reko, null);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F210_Not_3_Tukuyomi, null);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Menderu0, StringFieldMessage.F210_Not_4_Menderu, null);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Mati0, StringFieldMessage.F210_Not_5_Mati, null);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Matuka0, StringFieldMessage.F210_Not_6_Matuka, null);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Pierre0, StringFieldMessage.F210_Not_7_Pierre, null);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Mana0, StringFieldMessage.F210_Not_8_Mana, null);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Ami0, StringFieldMessage.F210_Not_9_Ami, null);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F210_Not_10_Reko, null);
        yield return msg.WaitForMessageEnd();

        // 消える
        reko.SetCameraEnable(false);

        yield return new WaitForSeconds(1f);
        Global.GetTemporaryData().ending_select_voice = -1;
        fieldScript.GetComponent<F210ExitEvent>().ExecEvent();
    }
}

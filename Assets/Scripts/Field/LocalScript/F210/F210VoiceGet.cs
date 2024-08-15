using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F210　声をもらうイベント
/// </summary>
public class F210VoiceGet : ActionEventBase
{
    /// <summary>キャラ</summary>
    public int index;

    public F210GetEffect getEff;

    public AudioClip voice_dialog;
    public AudioClip voice_get;
    public AudioClip voice_reko_1;
    public AudioClip voice_reko_2;
    public AudioClip voice_reko_3;

    /// <summary>
    /// イベント
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Exec()
    {
        var manager = ManagerSceneScript.GetInstance();
        var msg = manager.GetMessageWindow();
        var dlg = manager.GetDialogWindow();

        msg.Open();
        if (index == F210System.VOICE_AMI)// アミ
            msg.StartMessage(MessageWindow.Face.Ami0, StringFieldMessage.F210_Get_Ami_1, voice_dialog);
        else if (index == F210System.VOICE_MENDERU)// メンデル
            msg.StartMessage(MessageWindow.Face.Menderu0, StringFieldMessage.F210_Get_Menderu_1, voice_dialog);
        else if (index == F210System.VOICE_MATUKA)// まつかりすく
            msg.StartMessage(MessageWindow.Face.Matuka0, StringFieldMessage.F210_Get_Matuka_1, voice_dialog);
        else if (index == F210System.VOICE_MATI)// マチ
            msg.StartMessage(MessageWindow.Face.Mati0, StringFieldMessage.F210_Get_Mati_1, voice_dialog);
        else if (index == F210System.VOICE_PIERRE)// ピエール
            msg.StartMessage(MessageWindow.Face.Pierre0, StringFieldMessage.F210_Get_Pierre_1, voice_dialog);
        else if (index == F210System.VOICE_MANA)// MANA
            msg.StartMessage(MessageWindow.Face.Mana0, StringFieldMessage.F210_Get_Mana_1, voice_dialog);
        yield return msg.WaitForMessageEnd();
        yield return dlg.OpenDialog();
        if (dlg.GetResult() != DialogWindow.Result.Yes)
        {
            msg.Close();
            yield break;
        }

        if (index == F210System.VOICE_AMI)// アミ
            msg.StartMessage(MessageWindow.Face.Ami0, StringFieldMessage.F210_Get_Ami_2, voice_get);
        else if (index == F210System.VOICE_MENDERU)// メンデル
            msg.StartMessage(MessageWindow.Face.Menderu0, StringFieldMessage.F210_Get_Menderu_2, voice_get);
        else if (index == F210System.VOICE_MATUKA)// まつかりすく
            msg.StartMessage(MessageWindow.Face.Matuka0, StringFieldMessage.F210_Get_Matuka_2, voice_get);
        else if (index == F210System.VOICE_MATI)// マチ
            msg.StartMessage(MessageWindow.Face.Mati0, StringFieldMessage.F210_Get_Mati_2, voice_get);
        else if (index == F210System.VOICE_PIERRE)// ピエール
            msg.StartMessage(MessageWindow.Face.Pierre0, StringFieldMessage.F210_Get_Pierre_2, voice_get);
        else if (index == F210System.VOICE_MANA)// MANA
            msg.StartMessage(MessageWindow.Face.Mana0, StringFieldMessage.F210_Get_Mana_2, voice_get);
        yield return msg.WaitForMessageEnd();
        msg.Close();

        // 手に入れるエフェクト
        var color = index switch
        {
            F210System.VOICE_AMI => new Color(1f, 0.6f, 0.9f),
            F210System.VOICE_MENDERU => new Color(0f, 0.5f, 0.2f),
            F210System.VOICE_MATUKA => new Color(0.6f, 0.6f, 0.6f),
            F210System.VOICE_MATI => new Color(0f, 0.5f, 1f),
            F210System.VOICE_PIERRE => new Color(0.6f, 1f, 0f),
            _ => new Color(1f, 0.8f, 0.4f),
        };
        yield return getEff.PlayEffect(color, fieldScript.playerObj.transform.localPosition);
        yield return new WaitForSeconds(1f);

        // 手に入れたあとの会話イベント
        Global.GetTemporaryData().ending_select_voice = index;
        fieldScript.GetComponent<F210VoiceGetSuccess>().ExecEvent();
    }
}

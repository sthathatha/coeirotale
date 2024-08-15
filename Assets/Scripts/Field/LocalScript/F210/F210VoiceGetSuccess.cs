using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F210　声を受け取ったあと退場するまで
/// </summary>
public class F210VoiceGetSuccess : EventBase
{
    public F210VoiceGet getEventAmi;
    public F210VoiceGet getEventMenderu;
    public F210VoiceGet getEventMatuka;
    public F210VoiceGet getEventMati;
    public F210VoiceGet getEventPierre;
    public F210VoiceGet getEventMana;
    public F210VoiceGetTukuyomi getEventTukuyomi;

    public AudioClip voice_2_tukuyomi;
    public AudioClip voice_4_tukuyomi;
    public AudioClip voice_5_mana;
    public AudioClip voice_6_matuka;
    public AudioClip voice_7_pierre;
    public AudioClip voice_8_menderu;
    public AudioClip voice_9_ami;
    public AudioClip voice_10_mati;

    private bool pushEnd = false;

    /// <summary>
    /// 再生
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Exec()
    {
        var manager = ManagerSceneScript.GetInstance();
        var msg = manager.GetMessageWindow();
        var dlg = manager.GetDialogWindow();
        var reko = fieldScript.playerObj.GetComponent<PlayerScript>();
        var tukuyomi = fieldScript.tukuyomiObj.GetComponent<TukuyomiScript>();

        // 主人公の声選択用
        AudioClip vReko1 = null;
        AudioClip vReko2 = null;
        AudioClip vReko3 = null;
        if (Global.GetTemporaryData().ending_select_voice == F210System.VOICE_TUKUYOMI)
        {
            vReko1 = getEventTukuyomi.voice_reko_1;
            vReko2 = getEventTukuyomi.voice_reko_2;
            vReko3 = getEventTukuyomi.voice_reko_3;
        }
        else
        {
            var voiceEv = Global.GetTemporaryData().ending_select_voice switch
            {
                F210System.VOICE_AMI => getEventAmi,
                F210System.VOICE_MENDERU => getEventMenderu,
                F210System.VOICE_MATUKA => getEventMatuka,
                F210System.VOICE_MATI => getEventMati,
                F210System.VOICE_PIERRE => getEventPierre,
                _ => getEventMana,
            };
            vReko1 = voiceEv.voice_reko_1;
            vReko2 = voiceEv.voice_reko_2;
            vReko3 = voiceEv.voice_reko_3;
        }

        msg.Open();
        msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F210_Exit_1_Reko, vReko1);
        yield return msg.WaitForMessageEnd();

        // つくよみちゃんが押し出し始める
        reko.SetCameraEnable(false);
        tukuyomi.SetCameraEnable(true);
        StartCoroutine(PlayerPushCoroutine());
        msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F210_Exit_2_Tukuyomi, voice_2_tukuyomi);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F210_Exit_3_Reko, vReko2);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F210_Exit_4_Tukuyomi, voice_4_tukuyomi);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Mana0, StringFieldMessage.F210_Exit_5_Mana, voice_5_mana);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Matuka0, StringFieldMessage.F210_Exit_6_Matuka, voice_6_matuka);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Pierre0, StringFieldMessage.F210_Exit_7_Pierre, voice_7_pierre);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Menderu0, StringFieldMessage.F210_Exit_8_Menderu, voice_8_menderu);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Ami0, StringFieldMessage.F210_Exit_9_Ami, voice_9_ami);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Mati0, StringFieldMessage.F210_Exit_10_Mati, voice_10_mati);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F210_Exit_11_Reko, vReko3);
        yield return msg.WaitForMessageEnd();
        msg.Close();

        // 押し出し待ち
        yield return new WaitUntil(() => pushEnd);
        // プレイヤーフェードアウト
        reko.GetComponent<ModelUtil>().FadeOut(1f);
        yield return new WaitForSeconds(2f);

        fieldScript.GetComponent<F210ExitEvent>().ExecEvent();
    }

    /// <summary>
    /// プレイヤーを上に押していくコルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator PlayerPushCoroutine()
    {
        var reko = fieldScript.playerObj.GetComponent<PlayerScript>();
        var tukuyomi = fieldScript.tukuyomiObj.GetComponent<TukuyomiScript>();

        var tmpPos = reko.transform.localPosition;
        if (tmpPos.x > 0f)
        {
            // 右から押す
            tukuyomi.WalkTo(tmpPos + new Vector3(60f, 0));
            yield return new WaitWhile(() => tukuyomi.IsWalking());
            tmpPos.x = 0f;
            reko.SlideTo(tmpPos, 1f);
            tukuyomi.WalkTo(tmpPos + new Vector3(60f, 0));
        }
        else
        {
            // 左から押す
            tukuyomi.WalkTo(tmpPos + new Vector3(-60f, 0));
            yield return new WaitWhile(() => tukuyomi.IsWalking());
            tmpPos.x = 0f;
            reko.SlideTo(tmpPos, 1f);
            tukuyomi.WalkTo(tmpPos + new Vector3(-60f, 0));
        }
        yield return new WaitWhile(() => tukuyomi.IsWalking());

        // 下から押す
        tukuyomi.WalkTo(tmpPos + new Vector3(0, -60f));
        yield return new WaitWhile(() => tukuyomi.IsWalking());
        tmpPos = fieldScript.SearchGeneralPosition(1).GetPosition();
        reko.SlideTo(tmpPos, 1f);
        tukuyomi.WalkTo(tmpPos + new Vector3(0, -60f));
        yield return new WaitForSeconds(1f);

        // 他のキャラ上むく
        getEventAmi.GetComponent<CharacterScript>().SetDirection(Constant.Direction.Up);
        getEventMenderu.GetComponent<CharacterScript>().SetDirection(Constant.Direction.Up);
        getEventMatuka.GetComponent<CharacterScript>().SetDirection(Constant.Direction.Up);
        getEventMati.GetComponent<CharacterScript>().SetDirection(Constant.Direction.Up);
        getEventPierre.GetComponent<CharacterScript>().SetDirection(Constant.Direction.Up);
        getEventMana.GetComponent<CharacterScript>().SetDirection(Constant.Direction.Up);

        yield return new WaitWhile(() => tukuyomi.IsWalking());

        pushEnd = true;
        yield break;
    }
}

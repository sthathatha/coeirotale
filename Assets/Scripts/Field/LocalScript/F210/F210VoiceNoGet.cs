using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F210 声を貰わずクリアする
/// </summary>
public class F210VoiceNoGet : AreaEventBase
{
    public CharacterScript chr_ami;
    public CharacterScript chr_menderu;
    public CharacterScript chr_matuka;
    public CharacterScript chr_mati;
    public CharacterScript chr_pierre;
    public CharacterScript chr_mana;

    public AudioClip voice_1_tukuyomi;
    public AudioClip voice_2_tukuyomi;
    public AudioClip voice_3_menderu;
    public AudioClip voice_4_mati;
    public AudioClip voice_5_matuka;
    public AudioClip voice_6_pierre;
    public AudioClip voice_7_mana;
    public AudioClip voice_8_ami;

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
        var tukuyomi = fieldScript.tukuyomiObj.GetComponent<TukuyomiScript>();

        tukuyomi.SetDirection(Constant.Direction.Up);
        msg.Open();
        msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F210_Not_1_Tukuyomi, voice_1_tukuyomi);
        yield return msg.WaitForMessageEnd();
        yield return dlg.OpenDialog();
        if (dlg.GetResult() != DialogWindow.Result.Yes)
        {
            tukuyomi.SetDirection(Constant.Direction.Down);
            msg.Close();
            // キャンセル
            reko.WalkTo(reko.transform.localPosition - new Vector3(0, 64f));
            yield return new WaitWhile(() => reko.IsWalking());
            yield break;
        }

        // 会話してエンディングへ
        msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F210_Not_2_Reko, null);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F210_Not_3_Tukuyomi, voice_2_tukuyomi);
        yield return msg.WaitForMessageEnd();
        chr_menderu.SetDirection(Constant.Direction.Up);
        msg.StartMessage(MessageWindow.Face.Menderu0, StringFieldMessage.F210_Not_4_Menderu, voice_3_menderu);
        yield return msg.WaitForMessageEnd();
        chr_mati.SetDirection(Constant.Direction.Up);
        msg.StartMessage(MessageWindow.Face.Mati0, StringFieldMessage.F210_Not_5_Mati, voice_4_mati);
        yield return msg.WaitForMessageEnd();
        chr_matuka.SetDirection(Constant.Direction.Up);
        msg.StartMessage(MessageWindow.Face.Matuka0, StringFieldMessage.F210_Not_6_Matuka, voice_5_matuka);
        yield return msg.WaitForMessageEnd();
        chr_pierre.SetDirection(Constant.Direction.Up);
        msg.StartMessage(MessageWindow.Face.Pierre0, StringFieldMessage.F210_Not_7_Pierre, voice_6_pierre);
        yield return msg.WaitForMessageEnd();
        chr_mana.SetDirection(Constant.Direction.Up);
        msg.StartMessage(MessageWindow.Face.Mana0, StringFieldMessage.F210_Not_8_Mana, voice_7_mana);
        yield return msg.WaitForMessageEnd();
        chr_ami.SetDirection(Constant.Direction.Up);
        msg.StartMessage(MessageWindow.Face.Ami0, StringFieldMessage.F210_Not_9_Ami, voice_8_ami);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F210_Not_10_Reko, null);
        yield return msg.WaitForMessageEnd();
        msg.Close();

        // 消える
        reko.SetCameraEnable(false);
        reko.GetComponent<ModelUtil>().FadeOut(1f);
        yield return new WaitForSeconds(1f);

        // カメラつくよみちゃんに移動
        var cam = manager.mainCam;
        var camPos = new DeltaVector3();
        camPos.Set(cam.transform.position);
        camPos.MoveTo(tukuyomi.transform.localPosition, 1f, DeltaFloat.MoveType.LINE);
        while (camPos.IsActive())
        {
            yield return null;
            camPos.Update(Time.deltaTime);
            cam.SetTargetPos(new Vector2(camPos.Get().x, camPos.Get().y));
        }
        tukuyomi.SetCameraEnable(true);

        Global.GetTemporaryData().ending_select_voice = -1;
        fieldScript.GetComponent<F210ExitEvent>().ExecEvent();
    }
}

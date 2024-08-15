using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F210　退場したあと
/// </summary>
public class F210ExitEvent : EventBase
{
    public TukuyomiScript tukuyomi;
    public CharacterScript ami;
    public CharacterScript menderu;
    public CharacterScript matuka;
    public CharacterScript mati;
    public CharacterScript pierre;
    public CharacterScript mana;
    public GameObject sirowani0;
    public GameObject sirowani1;

    public AudioClip voice_1_tukuyomi;
    public AudioClip voice_2_matuka;
    public AudioClip voice_3_menderu;
    public AudioClip voice_4_mana;
    public AudioClip voice_5_mati;
    public AudioClip voice_6_mati;
    public AudioClip voice_7_pierre;
    public AudioClip voice_8_matuka;
    public AudioClip voice_9_mana;
    public AudioClip voice_10_menderu;
    public AudioClip voice_11_ami;
    public AudioClip voice_12_tukuyomi;
    public AudioClip voice_13_menderu;
    public AudioClip voice_14_ami;
    public AudioClip voice_15_tukuyomi;

    public AudioClip voice_sirowani;

    /// <summary>
    /// 実行
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Exec()
    {
        var manager = ManagerSceneScript.GetInstance();
        var sound = manager.soundMan;
        var cam = manager.mainCam;
        var msg = manager.GetMessageWindow();
        var p3 = fieldScript.SearchGeneralPosition(3);
        var p2 = fieldScript.SearchGeneralPosition(2);

        tukuyomi.SetCameraEnable(true);
        tukuyomi.WalkTo(p3.GetPosition());

        msg.Open();
        msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F210_Exit2_1_Tukuyomi, voice_1_tukuyomi);
        yield return msg.WaitForMessageEnd();
        if (Global.GetTemporaryData().ending_select_voice >= 0)
        {
            // 受取なし以外
            msg.StartMessage(MessageWindow.Face.Matuka0, StringFieldMessage.F210_Exit2_2_Matuka, voice_2_matuka);
            yield return msg.WaitForMessageEnd();
        }
        msg.StartMessage(MessageWindow.Face.Menderu0, StringFieldMessage.F210_Exit2_3_Menderu, voice_3_menderu);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Mana0, StringFieldMessage.F210_Exit2_4_Mana, voice_4_mana);
        yield return msg.WaitForMessageEnd();
        mati.SetDirection(Constant.Direction.Down);
        msg.StartMessage(MessageWindow.Face.Mati0, StringFieldMessage.F210_Exit2_5_Mati, voice_5_mati);
        yield return msg.WaitForMessageEnd();
        msg.Close();

        tukuyomi.SetDirection(Constant.Direction.Down);
        ami.SetDirection(Constant.Direction.Down);
        menderu.SetDirection(Constant.Direction.Down);
        matuka.SetDirection(Constant.Direction.Down);
        pierre.SetDirection(Constant.Direction.Down);
        mana.SetDirection(Constant.Direction.Down);
        // シロワニさん表示
        yield return manager.FadeOut();
        tukuyomi.SetCameraEnable(false);
        cam.SetTargetPos(p2.GetPosition());
        cam.Immediate();
        yield return new WaitForSeconds(0.5f);
        yield return manager.FadeIn();
        yield return new WaitForSeconds(0.5f);
        var tmpSirowani = new DeltaVector3();
        tmpSirowani.Set(sirowani1.transform.localPosition);
        tmpSirowani.MoveTo(p2.GetPosition(), 2.5f, DeltaFloat.MoveType.DECEL);
        while (tmpSirowani.IsActive())
        {
            yield return null;
            sirowani1.transform.localPosition = tmpSirowani.Get();
        }
        yield return new WaitForSeconds(1f);
        sound.PlayVoice(voice_sirowani);
        sirowani1.SetActive(false);
        sirowani0.SetActive(true);
        yield return new WaitForSeconds(1f);

        // 戻って来る
        yield return manager.FadeOut();
        cam.SetTargetPos(tukuyomi.transform.localPosition);
        cam.Immediate();
        tukuyomi.SetCameraEnable(true);
        yield return new WaitForSeconds(0.5f);
        yield return manager.FadeIn();

        msg.Open();
        msg.StartMessage(MessageWindow.Face.Mati0, StringFieldMessage.F210_Exit2_6_Mati, voice_6_mati);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Pierre0, StringFieldMessage.F210_Exit2_7_Pierre, voice_7_pierre);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Matuka0, StringFieldMessage.F210_Exit2_8_Matuka, voice_8_matuka);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Mana0, StringFieldMessage.F210_Exit2_9_Mana, voice_9_mana);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Menderu0, StringFieldMessage.F210_Exit2_10_Menderu, voice_10_menderu);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Ami0, StringFieldMessage.F210_Exit2_11_Ami, voice_11_ami);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F210_Exit2_12_Tukuyomi, voice_12_tukuyomi);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Menderu0, StringFieldMessage.F210_Exit2_13_Menderu, voice_13_menderu);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Ami0, StringFieldMessage.F210_Exit2_14_Ami, voice_14_ami);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F210_Exit2_15_Tukuyomi, voice_15_tukuyomi);
        yield return msg.WaitForMessageEnd();

        msg.Close();

        yield return new WaitForSeconds(1f);

        manager.LoadMainScene("EndingScene", 0);
    }
}

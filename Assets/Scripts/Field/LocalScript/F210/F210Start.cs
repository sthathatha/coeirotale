using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

/// <summary>
/// F210　開始時会話
/// </summary>
public class F210Start : EventBase
{
    public AudioClip voice_1_tukuyomi;
    public AudioClip voice_2_tukuyomi;
    public AudioClip voice_3_tukuyomi;
    public AudioClip voice_4_mana;
    public AudioClip voice_5_ami;
    public AudioClip voice_6_matuka;
    public AudioClip voice_7_menderu;
    public AudioClip voice_8_pierre;
    public AudioClip voice_9_mati;
    public AudioClip voice_10_tukuyomi;

    /// <summary>
    /// 会話
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Exec()
    {
        var manager = ManagerSceneScript.GetInstance();
        var msg = manager.GetMessageWindow();
        Global.GetSaveData().SetGameData(F210System.FIRST_EVENT_FLG, 1);

        msg.Open();
        msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F210_Start_1_Tukuyomi, voice_1_tukuyomi);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F210_Start_2_Reko);
        yield return msg.WaitForMessageEnd();
        StartCoroutine(CameraCenterCoroutine());
        msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F210_Start_3_Tukuyomi, voice_2_tukuyomi);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F210_Start_4_Tukuyomi, voice_3_tukuyomi);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F210_Start_5_Reko);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Mana0, StringFieldMessage.F210_Start_6_Mana, voice_4_mana);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Ami0, StringFieldMessage.F210_Start_7_Ami, voice_5_ami);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Matuka0, StringFieldMessage.F210_Start_8_Matuka, voice_6_matuka);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Menderu0, StringFieldMessage.F210_Start_9_Menderu, voice_7_menderu);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Pierre0, StringFieldMessage.F210_Start_10_Pierre, voice_8_pierre);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Mati0, StringFieldMessage.F210_Start_11_Mati, voice_9_mati);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F210_Start_12_Reko);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F210_Start_13_Tukuyomi, voice_10_tukuyomi);
        yield return msg.WaitForMessageEnd();
        msg.Close();
    }

    /// <summary>
    /// カメラをプレイヤーに戻す
    /// </summary>
    /// <returns></returns>
    private IEnumerator CameraCenterCoroutine()
    {
        var cam = ManagerSceneScript.GetInstance().mainCam;
        var p = new DeltaVector3();
        p.Set(cam.transform.position);
        p.MoveTo(fieldScript.playerObj.transform.position, 2f, DeltaFloat.MoveType.LINE);
        while (p.IsActive())
        {
            yield return null;
            p.Update(Time.deltaTime);
            cam.SetTargetPos(new Vector2(p.Get().x, p.Get().y));
        }

        fieldScript.playerObj.GetComponent<PlayerScript>().SetCameraEnable(true);
    }
}

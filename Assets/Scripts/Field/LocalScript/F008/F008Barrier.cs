using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// F008バリア壁
/// </summary>
public class F008Barrier : ActionEventBase
{
    #region メンバー

    public AudioClip voice2;
    public AudioClip voice4;
    public AudioClip voice5;
    public AudioClip voice6;
    public AudioClip voice7;
    public AudioClip voice8;
    public AudioClip voice9;
    public AudioClip voice10;
    public AudioClip voice11;
    public AudioClip voice13;
    public AudioClip voice14;
    public AudioClip voice_shout;
    public AudioClip voice16;
    public AudioClip voice17;
    public AudioClip voice18;
    public AudioClip voice19;
    public AudioClip voice20;

    #endregion

    /// <summary>
    /// 実行
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Exec()
    {
        var save = Global.GetSaveData();
        if (save.GetGameDataInt(F102System.MATI_WIN_FLG) < 1 ||
            save.GetGameDataInt(F112System.MATUKA_WIN_FLG) < 1 ||
            save.GetGameDataInt(F122System.F122_PIERRE_PHASE) < 2 ||
            save.GetGameDataInt(F132System.MANA_WIN_FLG) < 1 ||
            save.GetGameDataInt(F143System.MENDERU_WIN_FLG) < 1 ||
            save.GetGameDataInt(F153System.AMI_WIN_FLG) < 1)
        {
            yield break;
        }

        var manager = ManagerSceneScript.GetInstance();
        var msg = manager.GetMessageWindow();
        var system = fieldScript as F008System;
        var player = fieldScript.playerObj.GetComponent<PlayerScript>();
        var tukuyomi = fieldScript.tukuyomiObj.GetComponent<TukuyomiScript>();

        var pos2 = fieldScript.SearchGeneralPosition(2);
        tukuyomi.SetMode(TukuyomiScript.TukuyomiMode.NPC);
        tukuyomi.WalkTo(pos2.GetPosition(), afterDir: "down");
        var pos3 = fieldScript.SearchGeneralPosition(3);
        player.WalkTo(pos3.GetPosition());
        yield return new WaitWhile(() => player.IsWalking() || tukuyomi.IsWalking());
        yield return new WaitForSeconds(1f);

        msg.Open();
        msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F008_Break1_Reko, null);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F008_Break2_Tukuyomi, voice2);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F008_Break3_Reko, null);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Matuka0, StringFieldMessage.F008_Break4_Matuka, voice4);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Mati0, StringFieldMessage.F008_Break5_Mati, voice5);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Pierre0, StringFieldMessage.F008_Break6_Pierre, voice6);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Menderu0, StringFieldMessage.F008_Break7_Menderu, voice7);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Mana0, StringFieldMessage.F008_Break8_Mana, voice8);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Ami0, StringFieldMessage.F008_Break9_Ami, voice9);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Matuka0, StringFieldMessage.F008_Break10_Matuka, voice10);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F008_Break11_Tukuyomi, voice11);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F008_Break12_Reko, null);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Menderu0, StringFieldMessage.F008_Break13_Menderu, voice13);
        yield return msg.WaitForMessageEnd();

        system.Mati.SetDirection(Constant.Direction.Up);
        system.Matuka.SetDirection(Constant.Direction.Up);
        system.Pierre.SetDirection(Constant.Direction.Up);
        system.Mana.SetDirection(Constant.Direction.Up);
        system.Menderu.SetDirection(Constant.Direction.Up);
        system.Ami.SetDirection(Constant.Direction.Up);
        tukuyomi.SetDirection(Constant.Direction.Up);

        msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F008_Break14_Tukuyomi, voice14);
        yield return msg.WaitForMessageEnd();
        msg.Close();

        manager.soundMan.PlayVoice(voice_shout);
        yield return new WaitForSeconds(3f);

        var modelUtil = GetComponent<ModelUtil>();
        modelUtil.FadeOut(4f);

        // 消えたあとのメッセージ
        msg.Open();
        msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F008_Break15_Reko, null);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F008_Break16_Tukuyomi, voice16);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Mana0, StringFieldMessage.F008_Break17_Mana, voice17);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Matuka0, StringFieldMessage.F008_Break18_Matuka, voice18);
        yield return msg.WaitForMessageEnd();
        if (modelUtil.IsFading())
        {
            msg.Close();
            yield return new WaitWhile(() => modelUtil.IsFading());
            msg.Open();
        }
        msg.StartMessage(MessageWindow.Face.Mati0, StringFieldMessage.F008_Break19_Mati, voice19);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Pierre0, StringFieldMessage.F008_Break20_Pierre, voice20);
        yield return msg.WaitForMessageEnd();
        msg.Close();

        // 上に歩いて消える
        fieldScript.StartCoroutine(WalkOutCoroutine());
        yield return new WaitForSeconds(2f);

        // 自分も消える
        tukuyomi.SetMode(TukuyomiScript.TukuyomiMode.Trace);
        Global.GetSaveData().SetGameData(F008System.BARRIER_PHASE, 1);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 6人上に歩いて消える
    /// </summary>
    /// <returns></returns>
    private IEnumerator WalkOutCoroutine()
    {
        var system = fieldScript as F008System;

        system.StartCoroutine(WalkOutOneCoroutine(system.Mati));
        yield return new WaitForSeconds(0.3f);
        system.StartCoroutine(WalkOutOneCoroutine(system.Matuka));
        system.StartCoroutine(WalkOutOneCoroutine(system.Pierre));
        yield return new WaitForSeconds(0.2f);
        system.StartCoroutine(WalkOutOneCoroutine(system.Menderu));
        yield return new WaitForSeconds(0.1f);
        system.StartCoroutine(WalkOutOneCoroutine(system.Ami));
        system.StartCoroutine(WalkOutOneCoroutine(system.Mana));
    }

    /// <summary>
    /// 上に歩いて消える
    /// </summary>
    /// <param name="chr"></param>
    /// <returns></returns>
    private IEnumerator WalkOutOneCoroutine(CharacterScript chr)
    {
        var pos4 = fieldScript.SearchGeneralPosition(4);
        var pos = chr.transform.position;
        pos.y = pos4.GetPosition().y;

        chr.WalkTo(pos);
        yield return new WaitWhile(() => chr.IsWalking());
        chr.gameObject.SetActive(false);
    }
}

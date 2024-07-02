using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F204　マチと会話で進む
/// </summary>
public class F204Mati : ActionEventBase
{
    #region メンバー

    public F204IceWall iceWall;

    public F204Characters ami;
    public F204Characters mana;
    public F204Characters menderu;
    public F204Characters mati;
    public F204Characters matuka;
    public F204Characters pierre;

    public AudioClip voice1;
    public AudioClip voice3;
    public AudioClip voice4;
    public AudioClip voice5;
    public AudioClip voice7;
    public AudioClip voice9;
    public AudioClip voice11;
    public AudioClip voice12;

    #endregion

    /// <summary>
    /// 実行
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Exec()
    {
        var msg = ManagerSceneScript.GetInstance().GetMessageWindow();
        var pos3 = fieldScript.SearchGeneralPosition(3);
        var pos5 = fieldScript.SearchGeneralPosition(5);
        var player = fieldScript.playerObj.GetComponent<PlayerScript>();
        var tukuyomi = fieldScript.tukuyomiObj.GetComponent<TukuyomiScript>();

        msg.Open();
        msg.StartMessage(MessageWindow.Face.Mati0, StringFieldMessage.F204_Open_1_Mati, voice1);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F204_Open_2_Reko);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Mana0, StringFieldMessage.F204_Open_3_Mana, voice3);
        yield return msg.WaitForMessageEnd();
        msg.Close();

        tukuyomi.SetMode(TukuyomiScript.TukuyomiMode.NPC);
        tukuyomi.WalkTo(pos3.GetPosition(), afterDir: "down");
        matuka.StartWalk();
        yield return new WaitForSeconds(0.3f);
        pierre.StartWalk();
        mana.StartWalk();
        yield return new WaitForSeconds(0.3f);
        player.WalkTo(pos5.GetPosition());
        ami.StartWalk();
        yield return new WaitForSeconds(0.3f);
        menderu.StartWalk();
        yield return new WaitWhile(() => menderu.IsWalking());
        yield return new WaitForSeconds(0.5f);

        yield return iceWall.MoveRightCoroutine();

        msg.Open();
        msg.StartMessage(MessageWindow.Face.Ami0, StringFieldMessage.F204_Open_4_Ami, voice4);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Menderu0, StringFieldMessage.F204_Open_5_Menderu, voice5);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F204_Open_6_Reko);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Pierre0, StringFieldMessage.F204_Open_7_Pierre, voice7);
        StartCoroutine(iceWall.MoveLeftCoroutine());
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F204_Open_8_Reko);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F204_Open_9_Tukuyomi, voice9);
        StartCoroutine(iceWall.MoveRightCoroutine());
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F204_Open_10_Reko);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F204_Open_11_Tukuyomi, voice11);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Matuka0, StringFieldMessage.F204_Open_12_Matuka, voice12);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F204_Open_13_Reko);
        yield return msg.WaitForMessageEnd();
        msg.Close();

        Global.GetSaveData().SetGameData(F204System.WALL_OPEN_FLG, 1);
    }
}

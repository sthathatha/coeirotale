using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// F008バリア壁
/// </summary>
public class F008Barrier : ActionEventBase
{
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
        msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F008_Break1_Reko);
        yield return msg.WaitForMessageEnd();
        //todo:会話と叫び

        msg.Close();
        yield return new WaitForSeconds(1f);
        var modelUtil = GetComponent<ModelUtil>();
        modelUtil.FadeOut(4f);
        yield return new WaitForSeconds(1f);

        // 消えたあとのメッセージ


        // 上に歩いて消える
        fieldScript.StartCoroutine(WalkOutCoroutine());
        yield return new WaitForSeconds(2f);

        // 自分も消える
        yield return new WaitWhile(() => modelUtil.IsFading());
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

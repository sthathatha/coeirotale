using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F153小春日和　ウーラ
/// </summary>
public class F153Worra : ActionEventBase
{
    /// <summary></summary>
    public AudioClip voice1;

    /// <summary>
    /// 開始時
    /// </summary>
    public override void Start()
    {
        base.Start();

        if (Global.GetSaveData().GetGameDataInt(F131System.ICE_YOU_FLG) == 2 ||
            Global.GetSaveData().GetGameDataInt(F201System.DUNGEON_OPEN_FLG) >= 1)
        {
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 実行
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Exec()
    {
        var manager = ManagerSceneScript.GetInstance();
        var msg = manager.GetMessageWindow();

        if (Global.GetSaveData().GetGameDataInt(F131System.ICE_YOU_FLG) != 1)
        {
            msg.Open();
            msg.StartMessage(MessageWindow.Face.Worra0, StringFieldMessage.F153_Worra_Def1_Worra);
            yield return msg.WaitForMessageEnd();
            msg.Close();

            yield break;
        }

        // 悠捕獲に行く
        msg.Open();
        msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F153_Worra_Ice1_Tukuyomi, voice1);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Worra0, StringFieldMessage.F153_Worra_Ice2_Worra);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F153_Worra_Ice3_Reko);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F153_Worra_Ice4_Reko);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Worra0, StringFieldMessage.F153_Worra_Ice5_Worra);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Worra0, StringFieldMessage.F153_Worra_Ice6_Worra);
        yield return msg.WaitForMessageEnd();
        msg.Close();

        yield return manager.FadeOut();
        var obj = GetComponent<ObjectBase>();
        obj.model.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        yield return manager.FadeIn();
        Global.GetSaveData().SetGameData(F131System.ICE_YOU_FLG, 2);
    }
}

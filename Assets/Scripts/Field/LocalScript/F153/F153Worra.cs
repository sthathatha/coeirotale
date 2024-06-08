using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F153è¨ètì˙òaÅ@ÉEÅ[Éâ
/// </summary>
public class F153Worra : ActionEventBase
{
    /// <summary></summary>
    public AudioClip voice1;

    /// <summary>
    /// äJénéû
    /// </summary>
    public override void Start()
    {
        base.Start();

        if (Global.GetSaveData().GetGameDataInt(F131System.ICE_YOU_FLG) == 2)
        {
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// é¿çs
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

        // óIïﬂälÇ…çsÇ≠
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

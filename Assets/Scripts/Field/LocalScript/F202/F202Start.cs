using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F202äJénéû
/// </summary>
public class F202Start : EventBase
{
    public AudioClip voice1;

    /// <summary>
    /// é¿çs
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Exec()
    {
        var msg = ManagerSceneScript.GetInstance().GetMessageWindow();

        msg.Open();
        msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F202_Start_1_Reko);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F202_Start_2_Tukuyomi, voice1);
        yield return msg.WaitForMessageEnd();
        msg.Close();

        Global.GetSaveData().SetGameData(F202System.SHOW_START, 1);
    }
}

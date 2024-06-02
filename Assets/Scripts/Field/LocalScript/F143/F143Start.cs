using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 教会開始時
/// </summary>
public class F143Start : EventBase
{
    /// <summary>クー</summary>
    public CharacterScript koob;

    /// <summary>
    /// 実行
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Exec()
    {
        var msg = ManagerSceneScript.GetInstance().GetMessageWindow();

        var pos2 = fieldScript.SearchGeneralPosition(2);
        var pos3 = fieldScript.SearchGeneralPosition(3);
        koob.WalkTo(pos2.GetPosition());
        yield return new WaitForSeconds(1f);
        msg.Open();
        msg.StartMessage(MessageWindow.Face.Koob0, StringFieldMessage.F143_Fast1_Koob);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Koob0, StringFieldMessage.F143_Fast2_Koob);
        yield return msg.WaitForMessageEnd();
        msg.Close();

        koob.WalkTo(pos3.GetPosition());
        yield return new WaitWhile(() => koob.IsWalking());
        Destroy(koob.gameObject);

        Global.GetSaveData().SetGameData(F143System.F143_SHOW_FLG, 1);
    }
}

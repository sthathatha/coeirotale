using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// モンティホールクー
/// </summary>
public class F141Koob : ActionEventBase
{
    /// <summary>
    /// 実行
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Exec()
    {
        var failed = Global.GetSaveData().GetGameDataInt(F141System.FAIL_COUNT_SAVE);
        var msg = ManagerSceneScript.GetInstance().GetMessageWindow();
        var dialog = ManagerSceneScript.GetInstance().GetDialogWindow();

        msg.Open();
        msg.StartMessage(MessageWindow.Face.Koob0, StringFieldMessage.F141_1_Koob);
        yield return msg.WaitForMessageEnd();
        yield return dialog.OpenDialog();
        if (dialog.GetResult() != DialogWindow.Result.Yes)
        {
            msg.Close();
            yield break;
        }

        msg.StartMessage(MessageWindow.Face.Koob0, StringFieldMessage.F141_2_Koob);
        yield return msg.WaitForMessageEnd();
        if (failed >= 4)
        {
            msg.StartMessage(MessageWindow.Face.Koob0, StringFieldMessage.F141_3_Koob);
            yield return msg.WaitForMessageEnd();
        }
        if (failed >= 5)
        {
            msg.StartMessage(MessageWindow.Face.Koob0, StringFieldMessage.F141_4_Koob);
            yield return msg.WaitForMessageEnd();
        }
        msg.Close();
    }
}

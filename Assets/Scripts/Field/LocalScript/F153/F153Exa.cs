using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 小春日和　エグザ
/// </summary>
public class F153Exa : ActionEventBase
{
    /// <summary>
    /// 開始時
    /// </summary>
    public override void Start()
    {
        base.Start();

        if (Global.GetSaveData().GetGameDataInt(F111System.BRIDGE_FLG) < 2)
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

        msg.Open();

        if (Global.GetSaveData().GetGameDataInt(F101System.PLANT_FLG) == 1)
        {
            // 植物伐採
            msg.StartMessage(MessageWindow.Face.Exa0, StringFieldMessage.F153_Exa_Plant1_Exa);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Exa0, StringFieldMessage.F153_Exa_Plant2_Exa);
            yield return msg.WaitForMessageEnd();
        }
        else
        {
            // 通常
            msg.StartMessage(MessageWindow.Face.Exa0, StringFieldMessage.F153_Exa_Def1_Exa);
            yield return msg.WaitForMessageEnd();
        }


        msg.Close();

        yield break;
    }
}

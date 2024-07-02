using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// F153小春日和　悠
/// </summary>
public class F153You : ActionEventBase
{
    /// <summary>
    /// 開始時
    /// </summary>
    public override void Start()
    {
        base.Start();

        if (Global.GetSaveData().GetGameDataInt(F131System.ICE_YOU_FLG) < 3)
        {
            // 捕まえる前
            gameObject.SetActive(false);
            return;
        }

        if (Global.GetSaveData().GetGameDataInt(F101System.PLANT_FLG) == 2)
        {
            // 植物に連れ出されている最中
            gameObject.SetActive(false);
            return;
        }

        if (Global.GetSaveData().GetGameDataInt(F201System.DUNGEON_OPEN_FLG) >= 1)
        {
            // ラストダンジョンイベント後
            gameObject.SetActive(false);
            return;
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
            msg.StartMessage(MessageWindow.Face.You0, StringFieldMessage.F153_You_Plant1_You);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F153_You_Plant2_Reko);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.You0, StringFieldMessage.F153_You_Plant3_You);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Worra0, StringFieldMessage.F153_You_Plant4_Worra);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F153_You_Plant5_Reko);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.You0, StringFieldMessage.F153_You_Plant6_You);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Worra0, StringFieldMessage.F153_You_Plant7_Worra);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.You0, StringFieldMessage.F153_You_Plant8_You);
            yield return msg.WaitForMessageEnd();

            msg.Close();
            yield return manager.FadeOut();
            var obj = GetComponent<ObjectBase>();
            obj.model.SetActive(false);
            yield return new WaitForSeconds(0.1f);
            yield return manager.FadeIn();
            Global.GetSaveData().SetGameData(F101System.PLANT_FLG, 2);
        }
        else
        {
            msg.StartMessage(MessageWindow.Face.You0, StringFieldMessage.F153_You_Def1_You);
            yield return msg.WaitForMessageEnd();
            msg.Close();
        }
    }
}

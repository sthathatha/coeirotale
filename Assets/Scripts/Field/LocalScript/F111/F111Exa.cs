using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エグザ　橋の修理
/// </summary>
public class F111Exa : ActionEventBase
{
    public GameObject bridge_Broken;
    public GameObject bridge_Repair;
    public GameObject logs;
    public GameObject chair;

    public AudioClip voice_new1;
    public AudioClip voice_new2;
    public AudioClip voice_new3;
    public AudioClip voice_1;
    public AudioClip se_Repair1;
    public AudioClip se_Repair2;

    /// <summary>
    /// 開始時
    /// </summary>
    public override void Start()
    {
        base.Start();

        if (Global.GetSaveData().GetGameDataInt(F111System.BRIDGE_FLG) >= 2)
        {
            bridge_Broken.SetActive(false);
            bridge_Repair.SetActive(true);
            logs.SetActive(false);
            chair.SetActive(false);
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 会話
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Exec()
    {
        var msg = ManagerSceneScript.GetInstance().GetMessageWindow();
        var sound = ManagerSceneScript.GetInstance().soundMan;

        // 最初
        if (Global.GetSaveData().GetGameDataInt(F111System.BRIDGE_FLG) == 0)
        {
            msg.Open();
            msg.StartMessage(MessageWindow.Face.Exa0, StringFieldMessage.F111_New1_Exa);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F111_New2_Tukuyomi, voice_new1);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Exa0, StringFieldMessage.F111_New3_Exa);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Exa0, StringFieldMessage.F111_New4_Exa);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F111_New5_Tukuyomi, voice_new2);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Exa0, StringFieldMessage.F111_New6_Exa);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Exa0, StringFieldMessage.F111_New7_Exa);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Exa0, StringFieldMessage.F111_New8_Exa);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F111_New9_Reko);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F111_New10_Tukuyomi, voice_new3);
            yield return msg.WaitForMessageEnd();
            msg.Close();

            Global.GetSaveData().SetGameData(F111System.BRIDGE_FLG, 1);
            yield break;
        }

        // 修理済みで話しかける
        if (Global.GetSaveData().GetGameDataInt(F111System.BRIDGE_FLG) >= 2)
        {
            msg.Open();
            msg.StartMessage(MessageWindow.Face.Exa0, StringFieldMessage.F111_5_Exa);
            yield return msg.WaitForMessageEnd();
            msg.Close();
            yield break;
        }

        // ロープを渡す
        msg.Open();
        msg.StartMessage(MessageWindow.Face.Exa0, StringFieldMessage.F111_1_Exa);
        yield return msg.WaitForMessageEnd();
        if (Global.GetSaveData().GetGameDataInt(F151BoardSource.BOARD_USE_FLG) >= 1)
        {
            msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F111_2_Tukuyomi, voice_1);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Exa0, StringFieldMessage.F111_3_Exa);
            yield return msg.WaitForMessageEnd();
            msg.Close();

            // フェードアウトしてSE再生、材料と橋の表示切り替え
            yield return ManagerSceneScript.GetInstance().FadeOut();
            sound.PlaySE(se_Repair1);
            yield return new WaitForSeconds(2.8f);
            sound.PlaySE(se_Repair2);
            yield return new WaitForSeconds(0.5f);
            bridge_Broken.SetActive(false);
            bridge_Repair.SetActive(true);
            logs.SetActive(false);
            Global.GetSaveData().SetGameData(F111System.BRIDGE_FLG, 2);

            yield return ManagerSceneScript.GetInstance().FadeIn();

            msg.Open();
            msg.StartMessage(MessageWindow.Face.Exa0, StringFieldMessage.F111_4_Exa);
            yield return msg.WaitForMessageEnd();
        }
        msg.Close();
    }
}

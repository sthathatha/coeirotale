using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F153小春日和　小春音アミ
/// </summary>
public class F153Ami : ActionEventBase
{
    private int loseCount = 0;

    /// <summary>
    /// 開始時
    /// </summary>
    public override void Start()
    {
        base.Start();

        if (Global.GetSaveData().GetGameDataInt(F153System.AMI_WIN_FLG) == 1)
        {
            // クリア済みで居なくなる
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
        var save = Global.GetSaveData();

        msg.Open();
        // 開始会話
        if (save.GetGameDataInt(F153System.AMI_MEET_FLG) == 1)
        {
            msg.StartMessage(MessageWindow.Face.Ami0, StringFieldMessage.F153_Retry1_Ami, null);
            yield return msg.WaitForMessageEnd();
        }
        else
        {
            save.SetGameData(F153System.AMI_MEET_FLG, 1);
            msg.StartMessage(MessageWindow.Face.Ami0, StringFieldMessage.F153_New1_Ami, null);
            yield return msg.WaitForMessageEnd();
        }
        msg.Close();

        // 戦闘
        Global.GetTemporaryData().loseCount = loseCount;
        manager.StartGame("GameSceneAmiA");
        yield return new WaitUntil(() => manager.SceneState == ManagerSceneScript.State.Main);

        msg.Open();
        if (Global.GetTemporaryData().gameWon)
        {
            save.SetGameData(F153System.AMI_WIN_FLG, 1);
            //勝利
            msg.StartMessage(MessageWindow.Face.Ami0, StringFieldMessage.F153_Win1_Ami, null);
            yield return msg.WaitForMessageEnd();
            msg.Close();
            manager.LoadMainScene("Field004", 4);
        }
        else
        {
            //負け
            if (loseCount < 100) loseCount++;

            msg.StartMessage(MessageWindow.Face.Ami0, StringFieldMessage.F153_Lose1_Ami, null);
            yield return msg.WaitForMessageEnd();
            msg.Close();
        }
    }
}

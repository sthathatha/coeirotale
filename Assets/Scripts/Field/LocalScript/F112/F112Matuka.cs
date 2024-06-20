using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F112まつかりすく
/// </summary>
public class F112Matuka : ActionEventBase
{
    /// <summary>負けカウント</summary>
    private int loseCount = 0;

    /// <summary>
    /// 開始時
    /// </summary>
    public override void Start()
    {
        base.Start();

        if (Global.GetSaveData().GetGameDataInt(F112System.MATUKA_WIN_FLG) == 1)
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
        if (save.GetGameDataInt(F112System.MATUKA_MEET_FLG) == 1)
        {
            msg.StartMessage(MessageWindow.Face.Matuka0, StringFieldMessage.F112_Retry1_Matuka, null);
            yield return msg.WaitForMessageEnd();
        }
        else
        {
            save.SetGameData(F112System.MATUKA_MEET_FLG, 1);
            msg.StartMessage(MessageWindow.Face.Matuka0, StringFieldMessage.F112_New1_Matuka, null);
            yield return msg.WaitForMessageEnd();
        }
        msg.Close();

        // 戦闘
        Global.GetTemporaryData().loseCount = loseCount;
        manager.StartGame("GameSceneMatukaA");
        yield return new WaitUntil(() => manager.SceneState == ManagerSceneScript.State.Main);

        msg.Open();
        if (Global.GetTemporaryData().gameWon)
        {
            save.SetGameData(F112System.MATUKA_WIN_FLG, 1);
            //勝利
            msg.StartMessage(MessageWindow.Face.Matuka0, StringFieldMessage.F112_Win1_Matuka, null);
            yield return msg.WaitForMessageEnd();
            msg.Close();
            manager.LoadMainScene("Field004", 4);
        }
        else
        {
            //負け
            if (loseCount < 100) loseCount++;

            msg.StartMessage(MessageWindow.Face.Matuka0, StringFieldMessage.F112_Lose1_Matuka, null);
            yield return msg.WaitForMessageEnd();
            msg.Close();
        }
    }
}

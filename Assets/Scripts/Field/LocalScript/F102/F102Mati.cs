using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F102　マチ
/// </summary>
public class F102Mati : ActionEventBase
{
    /// <summary>負けカウント</summary>
    private int loseCount = 0;

    /// <summary>
    /// 開始時
    /// </summary>
    public override void Start()
    {
        base.Start();

        if (Global.GetSaveData().GetGameDataInt(F102System.MATI_WIN_FLG) == 1)
        {
            // クリア済みで居なくなる
            gameObject.SetActive(false);
        }
        else if (Global.GetSaveData().GetGameDataInt(F102System.MATI_MEET_FLG) == 1)
        {
            GetComponent<CharacterScript>().PlayAnim("down");
        } else
        {
            GetComponent<CharacterScript>().PlayAnim("up");
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
        if (save.GetGameDataInt(F102System.MATI_MEET_FLG) == 1)
        {
            msg.StartMessage(MessageWindow.Face.Mati0, StringFieldMessage.F102_Retry1_Mati, null);
            yield return msg.WaitForMessageEnd();
        }
        else
        {
            GetComponent<CharacterScript>().PlayAnim("down");
            save.SetGameData(F102System.MATI_MEET_FLG, 1);
            msg.StartMessage(MessageWindow.Face.Mati0, StringFieldMessage.F102_New1_Mati, null);
            yield return msg.WaitForMessageEnd();
        }
        msg.Close();

        // 戦闘
        Global.GetTemporaryData().loseCount = loseCount;
        manager.StartGame("GameSceneIkusautaA");
        yield return new WaitUntil(() => manager.SceneState == ManagerSceneScript.State.Main);

        msg.Open();
        if (Global.GetTemporaryData().gameWon)
        {
            save.SetGameData(F102System.MATI_WIN_FLG, 1);
            //勝利
            msg.StartMessage(MessageWindow.Face.Mati0, StringFieldMessage.F102_Win1_Mati, null);
            yield return msg.WaitForMessageEnd();
            msg.Close();
            manager.LoadMainScene("Field004", 4);
        }
        else
        {
            //負け
            if (loseCount < 100) loseCount++;

            msg.StartMessage(MessageWindow.Face.Mati0, StringFieldMessage.F102_Lose1_Mati, null);
            yield return msg.WaitForMessageEnd();
            msg.Close();
        }
    }
}

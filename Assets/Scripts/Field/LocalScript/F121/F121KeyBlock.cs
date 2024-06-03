using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F121 鍵穴
/// </summary>
public class F121KeyBlock : ActionEventBase
{
    /// <summary>鍵フラグのセーブデータ</summary>
    public const string KEY_FLG = "PierreKeyPhase";

    /// <summary>プレイヤー</summary>
    public PlayerScript player;

    public AudioClip voice1;
    public AudioClip voice2;

    public AudioClip openSe;

    /// <summary>
    /// 開始時
    /// </summary>
    public override void Start()
    {
        base.Start();

        // 解錠済みは開始時に消す
        if (Global.GetSaveData().GetGameDataInt(KEY_FLG) >= 2)
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
        // 鍵を持っていない場合メッセージ
        if (Global.GetSaveData().GetGameDataInt(KEY_FLG) == 0)
        {
            var msg = ManagerSceneScript.GetInstance().GetMessageWindow();
            msg.Open();

            msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F121_1_Tukuyomi, voice1);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F121_2_Reko);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F121_3_Tukuyomi, voice2);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F121_4_Reko);
            yield return msg.WaitForMessageEnd();
            msg.Close();

            yield break;
        }

        // 持っていたら開ける
        Global.GetSaveData().SetGameData(KEY_FLG, 2);
        ManagerSceneScript.GetInstance().soundMan.PlaySE(openSe);
        player.RemoveActionEvent(this);
        gameObject.SetActive(false);
    }
}

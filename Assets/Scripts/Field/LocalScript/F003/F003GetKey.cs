using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// サーカス入口の鍵を拾うオブジェクト
/// </summary>
public class F003GetKey : AreaActionEventBase
{
    /// <summary>プレイヤー</summary>
    public PlayerScript player;

    /// <summary>
    /// 開始時
    /// </summary>
    public override void Start()
    {
        base.Start();

        // 入手済みなら自分で消す
        if (Global.GetSaveData().GetGameDataInt(F121System.KEY_FLG) >= 2)
        {
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 実行
    /// </summary>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    protected override IEnumerator Exec()
    {
        // 鍵を入手
        Global.GetSaveData().SetGameData(F121System.KEY_FLG, 2);
        var msg = ManagerSceneScript.GetInstance().GetMessageWindow();

        msg.Open();
        msg.StartMessage(MessageWindow.Face.None2, StringFieldMessage.F003_GetKey);
        yield return msg.WaitForMessageEnd();

        msg.Close();

        player.RemoveAreaActionList(this);
        gameObject.SetActive(false);
    }
}

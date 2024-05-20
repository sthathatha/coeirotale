using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// メッセージのみのアクションイベント
/// </summary>
abstract public class SimpleMessageEvent : ActionEventBase
{
    /// <summary>
    /// 実行
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Exec()
    {
        var msg = ManagerSceneScript.GetInstance().GetMessageWindow();

        msg.Open();
        var idx = 0;
        while (true)
        {
            if (ShowMessage(msg, idx) == false) break;

            yield return msg.WaitForMessageEnd();
            ++idx;
        }

        msg.Close();
    }

    /// <summary>
    /// メッセージ表示
    /// </summary>
    /// <param name="msg">メッセージウィンドウ</param>
    /// <param name="index">0から順に呼ばれる</param>
    /// <returns>falseを返すと終了</returns>
    abstract protected bool ShowMessage(MessageWindow msg, int index);
}

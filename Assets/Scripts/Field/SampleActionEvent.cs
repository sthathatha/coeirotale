using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleActionEvent : ActionEventBase
{
    /// <summary>パラメータ</summary>
    public int param1;

    // テスト
    protected override IEnumerator Exec()
    {
        var manager = ManagerSceneScript.GetInstance();
        var window = manager.GetMessageWindow();

        //メッセージあ
        //window.Open();
        //window.StartMessage(MessageWindow.Face.Menderu0, "テスト文字列ああああああ");

        //yield return window.WaitForMessageEnd();
        //window.Close();

        //ゲームシーン呼び出し
        var sceneName = "";
        switch (param1)
        {
            case 0: sceneName = "GameSceneMenderuA";
                break;
            case 1: sceneName = "GameScenePierreA";
                break;
        }
        manager.StartGame(sceneName);
        yield return new WaitWhile(() => manager.SceneState != ManagerSceneScript.State.Main);

        //終わったらメッセージい
        //window.Open();
        //window.StartMessage(MessageWindow.Face.Menderu0, "テスト文字列いいいいい");

        //yield return window.WaitForMessageEnd();
        //window.Close();
    }
}

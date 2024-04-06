using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleGameScene : GameSceneScriptBase
{
    /// <summary>
    /// 初期化
    /// </summary>
    /// <returns></returns>
    override public IEnumerator Start()
    {
        yield return base.Start();
    }

    /// <summary>
    /// フレーム処理
    /// </summary>
    override public void Update()
    {
        base.Update();

        if (ManagerSceneScript.GetInstance().SceneState != ManagerSceneScript.State.Game) { return; }

        if (InputManager.GetInstance().GetKeyPress(InputManager.Keys.South))
        {
            // 終了してフィールドに戻る
            SetGameResult(false);
            ManagerSceneScript.GetInstance().ExitGame();
        }
    }
}

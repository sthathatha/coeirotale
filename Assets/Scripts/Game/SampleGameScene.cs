using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleGameScene : GameSceneScriptBase
{
    /// <summary>
    /// 初期化
    /// </summary>
    /// <returns></returns>
    public override IEnumerator Start()
    {
        yield return base.Start();
    }

    /// <summary>
    /// フェードイン後
    /// </summary>
    /// <returns></returns>
    public override IEnumerator AfterFadeIn()
    {
        var tutorial = ManagerSceneScript.GetInstance().GetMinigameTutorialWindow();
        var input = InputManager.GetInstance();

        yield return base.AfterFadeIn();
        yield return new WaitForSeconds(1f);

        // チュートリアル表示
        tutorial.SetTitle(StringMinigameMessage.MatiA_Title);
        tutorial.SetText(StringMinigameMessage.MatiA_Tutorial);
        yield return tutorial.Open();
        yield return new WaitUntil(() => input.GetKeyPress(InputManager.Keys.South));
        yield return tutorial.Close();

        yield return new WaitForSeconds(0.5f);
    }

    /// <summary>
    /// フレーム処理
    /// </summary>
    public override void Update()
    {
        base.Update();

        if (ManagerSceneScript.GetInstance().SceneState != ManagerSceneScript.State.Game) { return; }

        var input = InputManager.GetInstance();

        if (input.GetKeyPress(InputManager.Keys.South))
        {
            // 終了してフィールドに戻る
            SetGameResult(false);
            ManagerSceneScript.GetInstance().ExitGame();
        }
    }
}

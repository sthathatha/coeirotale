using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleGameScene : GameSceneScriptBase
{
    // Start is called before the first frame update
    override public IEnumerator Start()
    {
        yield return base.Start();
    }

    // Update is called once per frame
    override public void Update()
    {
        base.Update();

        if (ManagerSceneScript.GetInstance().SceneState != ManagerSceneScript.State.Game) { return; }

        if (InputManager.GetInstance().GetKeyPress(InputManager.Keys.South))
        {
            // 終了してフィールドに戻る
            ManagerSceneScript.GetInstance().ExitGame();
        }
    }
}

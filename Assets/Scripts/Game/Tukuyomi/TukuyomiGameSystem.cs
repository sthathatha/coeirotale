using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// つくよみちゃん戦
/// </summary>
public class TukuyomiGameSystem : GameSceneScriptBase
{
    public AudioClip bgm_scene1;

    /// <summary>
    /// フェードイン後
    /// </summary>
    /// <returns></returns>
    public override IEnumerator AfterFadeIn()
    {
        yield return base.AfterFadeIn();
        StartCoroutine(MainCoroutine());
    }

    /// <summary>
    /// メイン進行
    /// </summary>
    /// <returns></returns>
    private IEnumerator MainCoroutine()
    {
        var manager = ManagerSceneScript.GetInstance();
        manager.soundMan.StartGameBgm(bgm_scene1);

        //todo:テスト
        var input = InputManager.GetInstance();
        while (true)
        {
            yield return null;
            if (input.GetKeyPress(InputManager.Keys.South))
            {
                Global.GetTemporaryData().gameWon = true;
                break;
            }
            else if(input.GetKeyPress(InputManager.Keys.East))
            {
                Global.GetTemporaryData().gameWon = false;
                break;
            }
        }
        ManagerSceneScript.GetInstance().ExitGame();

    }
}

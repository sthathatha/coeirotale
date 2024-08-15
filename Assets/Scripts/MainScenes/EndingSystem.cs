using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エンディング
/// </summary>
public class EndingSystem : MainScriptBase
{
    public AudioClip endingBgm;

    /// <summary>
    /// 初期化
    /// </summary>
    /// <returns></returns>
    public override IEnumerator BeforeInitFadeIn()
    {
        yield return base.BeforeInitFadeIn();
        var cam = ManagerSceneScript.GetInstance().mainCam;
        cam.SetTargetPos(Vector2.zero);
    }

    /// <summary>
    /// フェードイン後
    /// </summary>
    /// <param name="init"></param>
    /// <returns></returns>
    public override IEnumerator AfterFadeIn(bool init)
    {
        yield return base.AfterFadeIn(init);
        Global.GetSaveData().system.clearFlag = 1;
        Global.GetSaveData().SaveSystemData();
        StartCoroutine(EndingCoroutine());
    }

    /// <summary>
    /// エンディング再生
    /// </summary>
    /// <returns></returns>
    private IEnumerator EndingCoroutine()
    {
        var manager = ManagerSceneScript.GetInstance();
        manager.soundMan.PlayFieldBgm(SoundManager.FieldBgmType.None, endingBgm);

        //todo:




        // 入力待ち
        var input = InputManager.GetInstance();
        yield return new WaitUntil(() => input.GetKeyPress(InputManager.Keys.South));
        manager.LoadMainScene("TitleScene", 0);
    }
}

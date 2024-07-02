using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F205�@�e�X�g�p
/// </summary>
public class F205TestBoss : ActionEventBase
{
    /// <summary>
    /// ���s
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Exec()
    {
        var manager = ManagerSceneScript.GetInstance();

        manager.StartGame("GameSceneBossA");
        yield return new WaitUntil(() => manager.SceneState == ManagerSceneScript.State.Main);
    }
}

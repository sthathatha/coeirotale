using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F205�@�{�X���b�V���e�X�g
/// </summary>
public class F205TestBossRush : ActionEventBase
{
    public bool allTest = false;

    /// <summary>
    /// ���s
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Exec()
    {
        var manager = ManagerSceneScript.GetInstance();
        Global.GetTemporaryData().bossRush = true;
        Global.GetTemporaryData().loseCount = 0;
        Global.GetTemporaryData().bossRushAmiWon = false;
        Global.GetTemporaryData().bossRushManaWon = false;
        Global.GetTemporaryData().bossRushMatukaWon = false;
        Global.GetTemporaryData().bossRushMatiWon = false;
        Global.GetTemporaryData().bossRushMenderuWon = false;
        Global.GetTemporaryData().bossRushPierreWon = false;

        if (allTest)
        {
            manager.StartGame("GameSceneAmiB");
        }
        else
        {
            manager.StartGame("GameSceneMatukaB");
        }
        yield return new WaitUntil(() => manager.SceneState == ManagerSceneScript.State.Main);

        Global.GetTemporaryData().bossRush = false;
    }
}

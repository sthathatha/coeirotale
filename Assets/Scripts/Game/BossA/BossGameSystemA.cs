using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ラスボス戦１
/// </summary>
public class BossGameSystemA : GameSceneScriptBase
{
    /// <summary>
    /// フェードイン後
    /// </summary>
    /// <returns></returns>
    public override IEnumerator AfterFadeIn()
    {
        yield return base.AfterFadeIn();
        yield return new WaitForSeconds(3f);

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F001ボタン
/// </summary>
public class F001Button : ActionEventBase
{
    /// <summary>壁</summary>
    public F001Wall wall;

    /// <summary>押した時のSE</summary>
    public AudioClip pressSE;

    /// <summary>
    /// 実行イベント
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Exec()
    {
        if (wall?.IsOpened() != false) yield break;

        var sound = ManagerSceneScript.GetInstance().soundMan;
        sound.PlaySE(pressSE);

        wall.Open();
    }
}

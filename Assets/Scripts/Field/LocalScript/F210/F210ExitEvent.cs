using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F210　退場したあと
/// </summary>
public class F210ExitEvent : EventBase
{
    public TukuyomiScript tukuyomi;

    /// <summary>
    /// 実行
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Exec()
    {
        var manager = ManagerSceneScript.GetInstance();

        tukuyomi.SetCameraEnable(true);

        yield return new WaitForSeconds(1f);

        manager.LoadMainScene("EndingScene", 0);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F153小春日和　クー
/// </summary>
public class F153Koob : ActionEventBase
{
    /// <summary>
    /// 開始時
    /// </summary>
    public override void Start()
    {
        base.Start();

    }

    /// <summary>
    /// 実行
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Exec()
    {
        var manager = ManagerSceneScript.GetInstance();
        var msg = manager.GetMessageWindow();

        yield break;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F141　ドア触ったとき
/// </summary>
public class F141DoorAction : ActionEventBase
{
    /// <summary>
    /// 実行
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Exec()
    {
        var system = fieldScript as F141System;
        yield return system.Knock(GetComponent<F141Door>());
    }
}

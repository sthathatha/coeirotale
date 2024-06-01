using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F141�@�h�A�G�����Ƃ�
/// </summary>
public class F141DoorAction : ActionEventBase
{
    /// <summary>
    /// ���s
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Exec()
    {
        var system = fieldScript as F141System;
        yield return system.Knock(GetComponent<F141Door>());
    }
}

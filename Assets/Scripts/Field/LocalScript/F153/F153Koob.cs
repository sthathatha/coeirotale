using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F153���t���a�@�N�[
/// </summary>
public class F153Koob : ActionEventBase
{
    /// <summary>
    /// �J�n��
    /// </summary>
    public override void Start()
    {
        base.Start();

    }

    /// <summary>
    /// ���s
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Exec()
    {
        var manager = ManagerSceneScript.GetInstance();
        var msg = manager.GetMessageWindow();

        yield break;
    }
}

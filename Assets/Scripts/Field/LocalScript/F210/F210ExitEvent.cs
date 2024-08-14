using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F210�@�ޏꂵ������
/// </summary>
public class F210ExitEvent : EventBase
{
    public TukuyomiScript tukuyomi;

    /// <summary>
    /// ���s
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

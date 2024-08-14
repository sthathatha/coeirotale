using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F210Å@ëﬁèÍÇµÇΩÇ†Ç∆
/// </summary>
public class F210ExitEvent : EventBase
{
    public TukuyomiScript tukuyomi;

    /// <summary>
    /// é¿çs
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F001�{�^��
/// </summary>
public class F001Button : ActionEventBase
{
    /// <summary>��</summary>
    public F001Wall wall;

    /// <summary>����������SE</summary>
    public AudioClip pressSE;

    /// <summary>
    /// ���s�C�x���g
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

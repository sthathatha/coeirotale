using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F002É{É^Éì
/// </summary>
public class F002Button : ActionEventBase
{
    /// <summary>SE</summary>
    public AudioClip pressSE;
    /// <summary>ï«ìÆÇ≠SE</summary>
    public AudioClip moveSE;

    /// <summary>ëŒè€ÇÃï«</summary>
    public F002Wall wall1 = null;
    /// <summary>ëŒè€ÇÃï«</summary>
    public F002Wall wall2 = null;
    /// <summary>ëŒè€ÇÃï«</summary>
    public F002Wall wall3 = null;
    /// <summary>ëŒè€ÇÃï«</summary>
    public F002Wall wall4 = null;

    /// <summary>
    /// é¿çs
    /// </summary>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    protected override IEnumerator Exec()
    {
        if (wall1?.IsMoving() == true ||
            wall2?.IsMoving() == true ||
            wall3?.IsMoving() == true ||
            wall4?.IsMoving() == true)
        {
            yield break;
        }

        var sound = ManagerSceneScript.GetInstance().soundMan;
        sound.PlaySE(pressSE);
        yield return new WaitForSeconds(0.5f);
        var se = sound.PlaySELoop(moveSE);

        wall1?.Toggle();
        wall2?.Toggle();
        wall3?.Toggle();
        wall4?.Toggle();

        StartCoroutine(SeWaitCoroutine(se));
    }

    private IEnumerator SeWaitCoroutine(AudioSource se)
    {
        yield return new WaitForSeconds(2f);
        ManagerSceneScript.GetInstance().soundMan.StopLoopSE(se, 0.5f);
    }
}

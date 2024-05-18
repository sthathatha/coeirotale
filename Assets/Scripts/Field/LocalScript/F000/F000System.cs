using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class F000System : MainScriptBase
{
    #region Šî’ê

    /// <summary>
    /// BGM
    /// </summary>
    /// <returns></returns>
    public override Tuple<SoundManager.FieldBgmType, AudioClip> GetBgm()
    {
        return new Tuple<SoundManager.FieldBgmType, AudioClip>(SoundManager.FieldBgmType.Common1, null);
    }

    public override IEnumerator AfterFadeIn()
    {
        yield return base.AfterFadeIn();

        //if (true)
        {
            var ev = GetComponent<F000StartEvent>();
            ev.ExecEvent();
        }
    }

    #endregion
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �T���v��
/// </summary>
public class SampleSceneField2 : MainScriptBase
{
    #region ���
    /// <summary>
    /// BGM
    /// </summary>
    /// <returns></returns>
    public override Tuple<SoundManager.FieldBgmType, AudioClip> GetBgm()
    {
        return new Tuple<SoundManager.FieldBgmType, AudioClip>(SoundManager.FieldBgmType.Common2, null);
    }
    #endregion
}

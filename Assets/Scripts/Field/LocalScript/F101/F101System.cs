using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F101�@��������
/// </summary>
public class F101System : MainScriptBase
{
    /// <summary>�c�^�t���O�@0�������@1�����@2�I���s�@3�����ς�</summary>
    public const string PLANT_FLG = "F101Plant";

    #region ���

    /// <summary>
    /// BGM
    /// </summary>
    /// <returns></returns>
    public override Tuple<SoundManager.FieldBgmType, AudioClip> GetBgm()
    {
        return new Tuple<SoundManager.FieldBgmType, AudioClip>(SoundManager.FieldBgmType.Common1, null);
    }

    #endregion
}

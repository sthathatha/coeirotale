using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F111�@���̏C��
/// </summary>
public class F111System : MainScriptBase
{
    /// <summary>���̏C���@0:�����@1:�˗��@2:�C���ς�</summary>
    public const string BRIDGE_FLG = "BridgeRepair";

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���X�{�X��P
/// </summary>
public class BossGameSystemA : GameSceneScriptBase
{
    /// <summary>
    /// �t�F�[�h�C����
    /// </summary>
    /// <returns></returns>
    public override IEnumerator AfterFadeIn()
    {
        yield return base.AfterFadeIn();
        yield return new WaitForSeconds(3f);

    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// サンプル
/// </summary>
public class SampleSceneField : MainScriptBase
{
    #region 基底
    /// <summary>
    /// 初期化
    /// </summary>
    /// <returns></returns>
    override protected IEnumerator Start()
    {
        yield return base.Start();
    }

    /// <summary>
    /// フレーム処理
    /// </summary>
    override protected void Update()
    {
        base.Update();
    }

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// フィールドキャラクター共通処理
/// </summary>
public class CharacterScript : ObjectBase
{
    /// <summary>モデルアニメーション</summary>
    protected Animator modelAnim;

    /// <summary>
    /// 初期化
    /// </summary>
    protected override void Start()
    {
        base.Start();

        modelAnim = model.GetComponent<Animator>();
    }

    /// <summary>
    /// フレーム処理
    /// </summary>
    protected override void Update()
    {
        base.Update();
    }
}

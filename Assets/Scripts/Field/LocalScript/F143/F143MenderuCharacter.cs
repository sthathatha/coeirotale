using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// キャラ設定
/// </summary>
public class F143MenderuCharacter : CharacterScript
{
    protected override void Start()
    {
        base.Start();

        SetDirection(Constant.Direction.Up);
    }
}

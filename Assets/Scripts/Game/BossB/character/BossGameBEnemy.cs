using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ラスボス本戦　敵キャラ基本
/// </summary>
public class BossGameBEnemy : BossGameBCharacterBase
{
    /// <summary>
    /// 初期化
    /// </summary>
    protected override void Start()
    {
        base.Start();

        CharacterType = CharaType.Enemy;
    }
}

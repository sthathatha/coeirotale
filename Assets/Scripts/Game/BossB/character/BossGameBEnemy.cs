using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���X�{�X�{��@�G�L������{
/// </summary>
public class BossGameBEnemy : BossGameBCharacterBase
{
    /// <summary>
    /// ������
    /// </summary>
    protected override void Start()
    {
        base.Start();

        CharacterType = CharaType.Enemy;
    }
}

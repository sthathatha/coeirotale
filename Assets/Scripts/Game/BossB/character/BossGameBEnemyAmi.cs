using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGameBEnemyAmi : BossGameBEnemy
{
    #region メンバー

    /// <summary>はるのとなり回復SE</summary>
    public AudioClip se_harunotonari1;

    #endregion

    /// <summary>
    /// ID
    /// </summary>
    /// <returns></returns>
    public override BossGameSystemB.CharacterID GetCharacterID()
    {
        return BossGameSystemB.CharacterID.Ami;
    }

    /// <summary>
    /// 初期化
    /// </summary>
    protected override void Start()
    {
        base.Start();

        skillList.Add(BossGameBDataBase.SkillID.Ami1);
    }
}

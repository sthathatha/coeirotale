using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGameBEnemyPierre : BossGameBEnemy
{
    /// <summary>
    /// ID
    /// </summary>
    /// <returns></returns>
    public override BossGameSystemB.CharacterID GetCharacterID()
    {
        return BossGameSystemB.CharacterID.Pierre;
    }

    /// <summary>
    /// èâä˙âª
    /// </summary>
    protected override void Start()
    {
        base.Start();

        skillList.Add(BossGameBDataBase.SkillID.Pierre1);
    }
}

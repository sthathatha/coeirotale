using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGameBEnemyMati : BossGameBEnemy
{
    /// <summary>
    /// ID
    /// </summary>
    /// <returns></returns>
    public override BossGameSystemB.CharacterID GetCharacterID()
    {
        return BossGameSystemB.CharacterID.Mati;
    }

    /// <summary>
    /// èâä˙âª
    /// </summary>
    protected override void Start()
    {
        base.Start();

        skillList.Add(BossGameBDataBase.SkillID.Mati1);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGameBEnemyMana : BossGameBEnemy
{
    /// <summary>
    /// ID
    /// </summary>
    /// <returns></returns>
    public override BossGameSystemB.CharacterID GetCharacterID()
    {
        return BossGameSystemB.CharacterID.Mana;
    }

    /// <summary>
    /// èâä˙âª
    /// </summary>
    protected override void Start()
    {
        base.Start();

        skillList.Add(BossGameBDataBase.SkillID.Mana1);
    }
}

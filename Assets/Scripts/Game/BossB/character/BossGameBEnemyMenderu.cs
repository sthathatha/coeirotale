using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGameBEnemyMenderu : BossGameBEnemy
{
    /// <summary>
    /// ID
    /// </summary>
    /// <returns></returns>
    public override BossGameSystemB.CharacterID GetCharacterID()
    {
        return BossGameSystemB.CharacterID.Menderu;
    }

    /// <summary>
    /// ������
    /// </summary>
    protected override void Start()
    {
        base.Start();

        skillList.Add(BossGameBDataBase.SkillID.Menderu1);
    }
}

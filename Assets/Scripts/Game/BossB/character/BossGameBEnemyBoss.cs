using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ラスボス本戦　本体
/// </summary>
public class BossGameBEnemyBoss : BossGameBEnemy
{
    /// <summary>
    /// ID
    /// </summary>
    /// <returns></returns>
    public override BossGameSystemB.CharacterID GetCharacterID()
    {
        return BossGameSystemB.CharacterID.Boss;
    }

    /// <summary>
    /// 初期化
    /// </summary>
    protected override void Start()
    {
        base.Start();

        skillList.Add(BossGameBDataBase.SkillID.Boss1_Carnage);
        skillList.Add(BossGameBDataBase.SkillID.Boss2_Prazma);
        skillList.Add(BossGameBDataBase.SkillID.Boss3_Canon);
        skillList.Add(BossGameBDataBase.SkillID.Boss4_Tranqui);
    }

    /// <summary>
    /// 特殊AI
    /// </summary>
    /// <returns></returns>
    protected override AIResult DecideAI()
    {
        //todo:ボスAI
        var ret = new AIResult();
        ret.Act = AIResult.ActionType.None;

        return ret;
    }
}

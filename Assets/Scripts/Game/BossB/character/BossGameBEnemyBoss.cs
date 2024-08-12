using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;

/// <summary>
/// ラスボス本戦　本体
/// </summary>
public class BossGameBEnemyBoss : BossGameBEnemy
{
    #region 定数

    private const int CARNAGE1_HP = 10000;
    private const int CARNAGE2_HP = 5000;
    private const int CARNAGE3_HP = 1500;

    #endregion

    #region メンバー

    public AudioClip se_skill_plasma;
    public AudioClip se_skill_charge;
    public AudioClip se_skill_canon;
    public AudioClip se_skill_tranqui;

    #endregion

    #region 変数

    bool carnage1 = true;
    bool carnage2 = true;
    bool carnage3 = true;

    #endregion

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
        skillList.Add(BossGameBDataBase.SkillID.Boss2_Plasma);
        skillList.Add(BossGameBDataBase.SkillID.Boss3_Canon);
        skillList.Add(BossGameBDataBase.SkillID.Boss4_Tranqui);
    }

    /// <summary>
    /// 特殊AI
    /// </summary>
    /// <returns></returns>
    protected override AIResult DecideAI()
    {
        var plrLoc = system.GetPlayerLoc();

        var ret = new AIResult();
        ret.Act = AIResult.ActionType.Skill;
        ret.SkillTargetLoc = plrLoc; // どうせ全部プレイヤー対象

        // HP条件にカルネージ
        if (carnage1 && param_HP < CARNAGE1_HP)
        {
            carnage1 = false;
            ret.SkillID = BossGameBDataBase.SkillID.Boss1_Carnage;
            return ret;
        }
        else if (carnage2 && param_HP < CARNAGE2_HP)
        {
            carnage2 = false;
            ret.SkillID = BossGameBDataBase.SkillID.Boss1_Carnage;
            return ret;
        }
        else if (carnage3 && param_HP < CARNAGE3_HP)
        {
            carnage3 = false;
            ret.SkillID = BossGameBDataBase.SkillID.Boss1_Carnage;
            return ret;
        }

        // 斜めに居たらアームストロング
        var checkCenter = location + new Vector2(0, body_height / 2);
        var dist = plrLoc - checkCenter;
        if (Mathf.Abs(dist.x) == Mathf.Abs(dist.y))
        {
            ret.SkillID = BossGameBDataBase.SkillID.Boss3_Canon;
            return ret;
        }

        // 縦横に居たら確率でトランキライザー
        if (Mathf.Abs(dist.x) <= 1 || Mathf.Abs(dist.y) <= 1)
        {
            if (Util.RandomCheck(50))
            {
                ret.SkillID = BossGameBDataBase.SkillID.Boss4_Tranqui;
                return ret;
            }
        }

        // 他はプラズマ
        ret.SkillID = BossGameBDataBase.SkillID.Boss2_Plasma;
        return ret;
    }

    /// <summary>
    /// スキル動作
    /// </summary>
    /// <param name="skillID"></param>
    /// <param name="targetCell"></param>
    /// <returns></returns>
    protected override IEnumerator UseSkill(BossGameBDataBase.SkillID skillID, Vector2Int targetCell)
    {
        var fieldCenter = new Vector2Int(BossGameSystemB.CELL_X_COUNT / 2, BossGameSystemB.CELL_Y_COUNT / 2);
        var skill = BossGameBDataBase.SkillList[skillID];
        var sound = ManagerSceneScript.GetInstance().soundMan;
        yield return null;

        var targetList = GetSkillHitCharacters(skillID, targetCell);
        targetList.Remove(this);

        var bodyCenter = location + new Vector2Int(0, body_height / 2);

        switch (skillID)
        {
            // カルネージ
            case BossGameBDataBase.SkillID.Boss1_Carnage:
                {
                    system.CreateNoFutureEffect();
                    yield return new WaitForSeconds(2f);
                    var chargeSe = sound.PlaySELoop(se_skill_charge);
                    system.CreateChargeEffect(bodyCenter);
                    yield return new WaitForSeconds(BossGameBChargeEffect.CHARGE_TIME);
                    sound.StopLoopSE(chargeSe);

                    // レーザー
                    system.CreateCarnageEffect(bodyCenter);
                    system.CreateCarnageEffect(bodyCenter);
                    yield return new WaitForSeconds(BossGameBCarnageEffect.CARNAGE_TIME);

                    yield return AttackDamage(targetList, skill.Value);
                }
                break;

            // プラズマフィールド
            case BossGameBDataBase.SkillID.Boss2_Plasma:
                {
                    // エフェクト
                    var basePos = BossGameSystemB.GetCellPosition(fieldCenter);
                    basePos.y += 50f;
                    var ofsX = BossGameSystemB.CELL_WIDTH * 1.75f;
                    var ofsY = BossGameSystemB.CELL_HEIGHT * 1.7f;
                    var base1 = basePos + new Vector3(ofsX, ofsY);
                    var base2 = basePos + new Vector3(-ofsX, -ofsY);
                    var base3 = basePos + new Vector3(-ofsX, ofsY);
                    var base4 = basePos + new Vector3(ofsX, -ofsY);

                    var loopSe = sound.PlaySELoop(se_skill_plasma);
                    for (var i = 0; i < 10; ++i)
                    {
                        var rand = new Vector3(Util.RandomFloat(-ofsX, ofsX), Util.RandomFloat(-ofsY, ofsY));
                        system.CreatePlasmaEffect(base1 + rand);
                        yield return new WaitForSeconds(0.02f);
                        rand = new Vector3(Util.RandomFloat(-ofsX, ofsX), Util.RandomFloat(-ofsY, ofsY));
                        system.CreatePlasmaEffect(base2 + rand);
                        yield return new WaitForSeconds(0.02f);
                        rand = new Vector3(Util.RandomFloat(-ofsX, ofsX), Util.RandomFloat(-ofsY, ofsY));
                        system.CreatePlasmaEffect(base3 + rand);
                        yield return new WaitForSeconds(0.02f);
                        rand = new Vector3(Util.RandomFloat(-ofsX, ofsX), Util.RandomFloat(-ofsY, ofsY));
                        system.CreatePlasmaEffect(base4 + rand);
                        yield return new WaitForSeconds(0.02f);
                    }
                    sound.StopLoopSE(loopSe);
                    yield return new WaitForSeconds(0.4f);

                    // 地形生成
                    var cellList = BossGameSystemB.CreateSkillEffectCellList(skillID, targetCell);
                    foreach (var cell in cellList)
                    {
                        if (Util.RandomCheck(20)) continue;
                        system.SetFieldEffect(cell, BossGameBDataObject.FieldEffect.Plasma, Util.RandomInt(60, 170));
                    }

                    // ダメージ
                    yield return AttackDamage(targetList, skill.Value);
                }
                break;

            // アームストロング砲
            case BossGameBDataBase.SkillID.Boss3_Canon:
                {
                    var chargeSe = sound.PlaySELoop(se_skill_charge);
                    system.CreateChargeEffect(bodyCenter);
                    yield return new WaitForSeconds(BossGameBChargeEffect.CHARGE_TIME);

                    sound.StopLoopSE(chargeSe);
                    sound.PlaySE(se_skill_canon);
                    system.CreateCanonEffect(targetCell - bodyCenter);
                    yield return new WaitForSeconds(2.5f);
                    yield return AttackDamage(targetList, skill.Value);
                }
                break;

            // トランキーライザー
            case BossGameBDataBase.SkillID.Boss4_Tranqui:
                {
                    var p1 = BossGameSystemB.GetCellPosition(bodyCenter);
                    var p2 = BossGameSystemB.GetCellPosition(targetCell);
                    sound.PlaySE(se_skill_tranqui);
                    // エフェクト
                    for (var i = 0; i < 6; ++i)
                    {
                        system.CreateTranquiEffect(p1, p2);
                        yield return new WaitForSeconds(0.07f);
                    }
                    yield return new WaitForSeconds(0.2f);
                    yield return AttackDamage(targetList, skill.Value);
                    targetList = targetList.Where(t => t.GetHp() > 0).ToList();
                    if (targetList.Any() && Util.RandomCheck(80))
                    {
                        yield return system.BuffAttack(targetList, 0.8f);
                    }
                }
                break;
        }
    }
}

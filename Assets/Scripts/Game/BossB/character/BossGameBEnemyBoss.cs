using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;

/// <summary>
/// ���X�{�X�{��@�{��
/// </summary>
public class BossGameBEnemyBoss : BossGameBEnemy
{
    #region �萔

    private const int CARNAGE1_HP = 10000;
    private const int CARNAGE2_HP = 5000;
    private const int CARNAGE3_HP = 1500;

    #endregion

    #region �����o�[

    public AudioClip se_skill_plasma;
    public AudioClip se_skill_charge;
    public AudioClip se_skill_canon;
    public AudioClip se_skill_tranqui;

    #endregion

    #region �ϐ�

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
    /// ������
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
    /// ����AI
    /// </summary>
    /// <returns></returns>
    protected override AIResult DecideAI()
    {
        var plrLoc = system.GetPlayerLoc();

        var ret = new AIResult();
        ret.Act = AIResult.ActionType.Skill;
        ret.SkillTargetLoc = plrLoc; // �ǂ����S���v���C���[�Ώ�

        // HP�����ɃJ���l�[�W
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

        // �΂߂ɋ�����A�[���X�g�����O
        var checkCenter = location + new Vector2(0, body_height / 2);
        var dist = plrLoc - checkCenter;
        if (Mathf.Abs(dist.x) == Mathf.Abs(dist.y))
        {
            ret.SkillID = BossGameBDataBase.SkillID.Boss3_Canon;
            return ret;
        }

        // �c���ɋ�����m���Ńg�����L���C�U�[
        if (Mathf.Abs(dist.x) <= 1 || Mathf.Abs(dist.y) <= 1)
        {
            if (Util.RandomCheck(50))
            {
                ret.SkillID = BossGameBDataBase.SkillID.Boss4_Tranqui;
                return ret;
            }
        }

        // ���̓v���Y�}
        ret.SkillID = BossGameBDataBase.SkillID.Boss2_Plasma;
        return ret;
    }

    /// <summary>
    /// �X�L������
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
            // �J���l�[�W
            case BossGameBDataBase.SkillID.Boss1_Carnage:
                {
                    system.CreateNoFutureEffect();
                    yield return new WaitForSeconds(2f);
                    var chargeSe = sound.PlaySELoop(se_skill_charge);
                    system.CreateChargeEffect(bodyCenter);
                    yield return new WaitForSeconds(BossGameBChargeEffect.CHARGE_TIME);
                    sound.StopLoopSE(chargeSe);

                    // ���[�U�[
                    system.CreateCarnageEffect(bodyCenter);
                    system.CreateCarnageEffect(bodyCenter);
                    yield return new WaitForSeconds(BossGameBCarnageEffect.CARNAGE_TIME);

                    yield return AttackDamage(targetList, skill.Value);
                }
                break;

            // �v���Y�}�t�B�[���h
            case BossGameBDataBase.SkillID.Boss2_Plasma:
                {
                    // �G�t�F�N�g
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

                    // �n�`����
                    var cellList = BossGameSystemB.CreateSkillEffectCellList(skillID, targetCell);
                    foreach (var cell in cellList)
                    {
                        if (Util.RandomCheck(20)) continue;
                        system.SetFieldEffect(cell, BossGameBDataObject.FieldEffect.Plasma, Util.RandomInt(60, 170));
                    }

                    // �_���[�W
                    yield return AttackDamage(targetList, skill.Value);
                }
                break;

            // �A�[���X�g�����O�C
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

            // �g�����L�[���C�U�[
            case BossGameBDataBase.SkillID.Boss4_Tranqui:
                {
                    var p1 = BossGameSystemB.GetCellPosition(bodyCenter);
                    var p2 = BossGameSystemB.GetCellPosition(targetCell);
                    sound.PlaySE(se_skill_tranqui);
                    // �G�t�F�N�g
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

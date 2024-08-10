using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

/// <summary>
/// ���X�{�X�{��@�v���C���[
/// </summary>
public class BossGameBPlayer : BossGameBCharacterBase
{
    #region �萔

    /// <summary>���Ԍo�߂Ȃ��ŕ��������</summary>
    private const int WALK_DELAY_COUNT = 3;

    #endregion

    #region �����o�[

    /// <summary>�T�C�N���g����SE</summary>
    public AudioClip se_cycrotron1;
    /// <summary>�I�N�g�X�g���C�NSE</summary>
    public AudioClip se_octstrike1;
    /// <summary>�C�[�O���_�C�uSE</summary>
    public AudioClip se_eagleDive1;
    /// <summary>�C���r���V�u��SE</summary>
    public AudioClip se_invincible1;
    /// <summary>���C���J�[�l�[�V����SE</summary>
    public AudioClip se_reincarnation1;
    /// <summary>�I���W��SE</summary>
    public AudioClip se_origin1;
    /// <summary>�C�O�j�b�V����SE</summary>
    public AudioClip se_ignition1;
    /// <summary>�L�[�j���O�V���tSE</summary>
    public AudioClip se_keeningsylph1;

    #endregion

    #region �ϐ�

    /// <summary>����������</summary>
    private int walkCount = 0;

    /// <summary>�L�����Z����</summary>
    private int resetSelect = 0;

    #endregion

    #region ���

    /// <summary>
    /// ������
    /// </summary>
    protected override void Start()
    {
        base.Start();

        skillList.Add(BossGameBDataBase.SkillID.Reko1_C);
        skillList.Add(BossGameBDataBase.SkillID.Reko2_O);
        skillList.Add(BossGameBDataBase.SkillID.Reko3_E);
        skillList.Add(BossGameBDataBase.SkillID.Reko4_I);
        skillList.Add(BossGameBDataBase.SkillID.Reko5_R);
        skillList.Add(BossGameBDataBase.SkillID.Reko6_O);
        skillList.Add(BossGameBDataBase.SkillID.Reko7_I);
        skillList.Add(BossGameBDataBase.SkillID.Reko8_N);
        skillList.Add(BossGameBDataBase.SkillID.Reko9_K);

        // �A�팋�ʂŃX�L���ǉ�
        var g = Global.GetTemporaryData();
        if (g.bossRushAmiWon) skillList.Add(BossGameBDataBase.SkillID.Ami1);
        if (g.bossRushManaWon) skillList.Add(BossGameBDataBase.SkillID.Mana1);
        if (g.bossRushMatiWon) skillList.Add(BossGameBDataBase.SkillID.Mati1);
        if (g.bossRushMenderuWon) skillList.Add(BossGameBDataBase.SkillID.Menderu1);
        if (g.bossRushMatukaWon) skillList.Add(BossGameBDataBase.SkillID.Matuka1);
        if (g.bossRushPierreWon) skillList.Add(BossGameBDataBase.SkillID.Pierre1);

        CharacterType = CharaType.Player;
    }

    /// <summary>
    /// �L����ID
    /// </summary>
    /// <returns></returns>
    public override BossGameSystemB.CharacterID GetCharacterID()
    {
        return BossGameSystemB.CharacterID.Player;
    }

    /// <summary>
    /// �^�[���s��
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator TurnProcess()
    {
        var input = InputManager.GetInstance();
        var command = system.commandUI;
        var cells = system.cellUI;
        var sound = ManagerSceneScript.GetInstance().soundMan;

        while (true)
        {
            yield return null;

            if (input.GetKeyPress(InputManager.Keys.South) || resetSelect == 1)
            {
                // ���j���[���J���ď���
                if (resetSelect != 1) sound.PlaySE(sound.commonSeWindowOpen);
                command.SkillList = skillList;
                yield return command.Open(resetSelect != 1);
                if (command.Result == BossGameBUICommand.CommandResult.Cancel)
                {
                    sound.PlaySE(sound.commonSeError);
                    resetSelect = 0;
                    continue;
                }
                sound.PlaySE(sound.commonSeSelect);

                // �I�������X�L���̑ΏۑI��UI
                yield return cells.ShowSelect(command.SelectSkill, this);
                if (cells.Result == BossGameBUICellSelect.CellSelectResult.Cancel)
                {
                    sound.PlaySE(sound.commonSeError);
                    resetSelect = 1;
                    continue;
                }
                resetSelect = 0;

                // �X�L���g�p
                yield return UseSkillBase(command.SelectSkill, cells.SelectCell);

                system.playerWalkReset = true;
                break;
            }
            else if (input.GetKeyPress(InputManager.Keys.East))
            {
                // �L�����Z���{�^����HP�\��
                yield return system.ShowHp();
            }
            else
            {
                var walkLoc = new Vector2Int(0, 0);
                if (input.GetKey(InputManager.Keys.Up))
                    walkLoc.y = 1;
                else if (input.GetKey(InputManager.Keys.Down))
                    walkLoc.y = -1;
                else if (input.GetKey(InputManager.Keys.Left))
                    walkLoc.x = -1;
                else if (input.GetKey(InputManager.Keys.Right))
                    walkLoc.x = 1;

                if (walkLoc.x != 0 || walkLoc.y != 0)
                {
                    if (system.CanWalk(GetLocation() + walkLoc))
                    {
                        yield return Walk(walkLoc.x, walkLoc.y);

                        // �n�`���ʃ`�F�b�N
                        var effect = system.GetCellFieldEffect(location);
                        if (effect == BossGameBDataObject.FieldEffect.Mantrap)
                        {
                            // �}���g���b�v�ɓ������狭���I��
                            sound.PlaySE(system.dataObj.se_skill_mantrap);
                            system.ClearFieldEffect(location);
                            yield return new WaitForSeconds(0.4f);
                            system.playerWalkReset = true;
                            break;
                        }
                        else if (effect == BossGameBDataObject.FieldEffect.Plasma)
                        {
                            // �_���[�W������
                            sound.PlaySE(system.dataObj.se_field_plasma);
                            yield return HitDamage(Util.RandomInt(160, 240), true);
                            system.ClearFieldEffect(location);
                        }

                        if (WalkEndCheck() == false) break;
                    }
                    else
                    {
                        // �ړ��ł��Ȃ��ꏊ�͌�����ς��邾��
                        SetDirection(walkLoc.x, walkLoc.y);
                    }
                }
            }

            resetSelect = 0;
        }
    }

    /// <summary>
    /// �X�L�����s
    /// </summary>
    /// <param name="skillID"></param>
    /// <param name="targetCell"></param>
    /// <returns></returns>
    protected override IEnumerator UseSkill(BossGameBDataBase.SkillID skillID, Vector2Int targetCell)
    {
        var skill = BossGameBDataBase.SkillList[skillID];
        var sound = ManagerSceneScript.GetInstance().soundMan;
        yield return null;

        var targetList = GetSkillHitCharacters(skillID, targetCell);

        switch (skillID)
        {
            case BossGameBDataBase.SkillID.Reko1_C: // �T�C�N���g����
                sound.PlaySE(se_cycrotron1);
                // ��]
                for (var i = 0; i < 16; ++i)
                {
                    model.sprite = sp_leftup;
                    yield return new WaitForSeconds(0.04f);
                    model.sprite = sp_rightup;
                    yield return new WaitForSeconds(0.04f);
                }
                ResetDirection();
                // �����̑��x�A�b�v
                yield return system.BuffSpeed(targetList, 1.2f);
                yield return new WaitForSeconds(0.5f);
                break;
            case BossGameBDataBase.SkillID.Reko2_O: // �I�N�g�X�g���C�N
                yield return EffectMove(targetCell, 0.2f);
                var targetCellPos = BossGameSystemB.GetCellPosition(targetCell);
                for (var i = 0; i < 8; ++i)
                {
                    var randPos = new Vector3(Util.RandomFloat(-40f, 40f), Util.RandomFloat(-40f, 40f));
                    system.CreateGeneralEffect(targetCellPos + randPos, BossGameBDataObject.EffectKind.Hit);
                    sound.PlaySE(se_octstrike1);
                    model.sprite = sp_leftup;
                    yield return new WaitForSeconds(0.04f);
                    model.sprite = sp_rightup;
                    yield return new WaitForSeconds(0.04f);
                    model.sprite = sp_leftup;
                    yield return new WaitForSeconds(0.04f);
                    model.sprite = sp_rightup;
                    yield return new WaitForSeconds(0.04f);
                }
                ResetDirection();
                yield return EffectMove(location, 0.4f);
                yield return AttackDamage(targetList, skill.Value);
                break;
            case BossGameBDataBase.SkillID.Reko3_E: // �C�[�O���_�C�u
                yield return EffectMove(targetCell, 0.6f, 400f);
                sound.PlaySE(se_eagleDive1);
                system.CreateSlashEffect(targetCell, new Vector3(0, -1));
                yield return new WaitForSeconds(0.4f);
                yield return EffectMove(location, 0.6f, 300f);
                yield return AttackDamage(targetList, skill.Value);
                break;
            case BossGameBDataBase.SkillID.Reko4_I: // �C���r���V�u��
                system.CreateGeneralEffect(BossGameSystemB.GetCellPosition(location), BossGameBDataObject.EffectKind.Invincible);
                yield return new WaitForSeconds(0.14f);
                sound.PlaySE(se_invincible1);
                yield return new WaitForSeconds(0.8f);
                isInvincible = true;
                break;
            case BossGameBDataBase.SkillID.Reko5_R: // ���C���J�[�l�[�V����
                sound.PlaySE(se_reincarnation1);
                yield return system.PlayHealEffect(location, 1, 1);
                yield return HealDamage(param_HP_max);
                break;
            case BossGameBDataBase.SkillID.Reko6_O: // �I���W��
                sound.PlaySE(se_origin1);
                system.CreateOriginEffect(location);
                yield return new WaitForSeconds(0.8f);
                foreach (var chara in targetList)
                {
                    chara.ResetParam();
                }
                system.ClearFieldEffect();
                break;
            case BossGameBDataBase.SkillID.Reko7_I: // �C�O�j�b�V����
                system.CreateGeneralEffect(BossGameSystemB.GetCellPosition(location), BossGameBDataObject.EffectKind.Ignition);
                yield return new WaitForSeconds(0.2f);
                sound.PlaySE(se_ignition1);
                yield return new WaitForSeconds(0.6f);
                yield return system.BuffAttack(targetList, 1.2f);
                yield return new WaitForSeconds(0.5f);
                break;
            case BossGameBDataBase.SkillID.Reko8_N: // �i�C�g���A
                {
                    system.PlayHorrorEffect(targetCell);
                    yield return new WaitForSeconds(2.6f);
                    yield return AttackDamage(targetList, skill.Value, false);
                    var slowChara = new List<BossGameBCharacterBase>();
                    foreach (var chara in targetList)
                    {
                        // �m���ōs�����ԑ���
                        if (Util.RandomCheck(50)) chara.DelayTime(1);

                        // �m���ő��x�_�E��
                        if (Util.RandomCheck(50)) slowChara.Add(chara);
                    }
                    yield return system.BuffSpeed(slowChara, 0.9f);
                }
                break;
            case BossGameBDataBase.SkillID.Reko9_K: // �L�[�j���O�V���t
                var center = BossGameSystemB.GetCellPosition(location);
                var ofsX = BossGameSystemB.CELL_WIDTH;
                var ofsY = BossGameSystemB.CELL_HEIGHT;
                var cs = new[] {
                    center + new Vector3(ofsX, ofsY, 0),
                    center + new Vector3(-ofsX, -ofsY, 0),
                    center + new Vector3(-ofsX, ofsY, 0),
                    center + new Vector3(ofsX, -ofsY, 0)
                };
                // ��]
                for (var i = 0; i < 16; ++i)
                {
                    if (i % 2 == 0)
                    {
                        sound.PlaySE(se_keeningsylph1);
                    }
                    var effPos = cs[i % 4] + new Vector3(Util.RandomFloat(-ofsX, ofsX), Util.RandomFloat(-ofsY, ofsY));
                    system.CreateGeneralEffect(effPos, BossGameBDataObject.EffectKind.Cycrone);
                    model.sprite = sp_leftup;
                    yield return new WaitForSeconds(0.04f);
                    model.sprite = sp_rightup;
                    yield return new WaitForSeconds(0.04f);
                }
                ResetDirection();
                yield return AttackDamage(targetList, skill.Value, false);
                break;
        }
    }

    #endregion

    #region �@�\

    /// <summary>
    /// ���ԃ��Z�b�g
    /// </summary>
    /// <param name="init"></param>
    public override void ResetTime(bool init = false)
    {
        if (walkCount > 0)
        {
            // ���������̎���
            param_wait_time = GetMaxWaitTime() / 3;
        }
        else if (init)
        {
            // �v���C���[�̏������Ԃ͏��Ȃ�
            param_wait_time = GetMaxWaitTime() / 2;
        }
        else
        {
            base.ResetTime();
        }
    }

    /// <summary>
    /// ���s�I�����̎��Ԍo�ߔ���
    /// </summary>
    /// <returns></returns>
    private bool WalkEndCheck()
    {
        ++walkCount;
        // �R���܂ł͎��Ԃ����Ȃ�
        if (walkCount <= WALK_DELAY_COUNT) return true;

        var walkWait = GetMaxWaitTime() / 3;
        if (system.CanWalkWait(this, walkWait))
        {
            // �܂����������Ȃ��Ȃ猸�炵�đ��s
            system.DecreaseAllCharacterWait(this, walkWait);
            return true;
        }

        return false;
    }

    public void ResetWalkCount()
    {
        walkCount = 0;
    }

    #endregion
}

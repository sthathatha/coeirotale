using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// ���X�{�X�{��@�L�����N�^�[���ʏ���
/// </summary>
public class BossGameBCharacterBase : MonoBehaviour
{
    #region �萔

    /// <summary>�L�����̌���</summary>
    public enum CharaDirection : int
    {
        LeftUp = 0,
        RightUp,
        LeftDown,
        RightDown,
    }

    /// <summary>�L��������</summary>
    public enum CharaType : int
    {
        Player = 0,
        Enemy,
    }

    #endregion

    #region �����o�[

    /// <summary>�Q�[���V�X�e��</summary>
    public BossGameSystemB system;

    /// <summary>���f��</summary>
    public SpriteRenderer model;

    /// <summary>�E��摜</summary>
    public Sprite sp_rightup = null;
    /// <summary>����摜</summary>
    public Sprite sp_leftup = null;
    /// <summary>�E���摜</summary>
    public Sprite sp_rightdown = null;
    /// <summary>�����摜</summary>
    public Sprite sp_leftdown = null;

    /// <summary>���}�X��</summary>
    public int body_width = 1;
    /// <summary>�c�}�X��</summary>
    public int body_height = 1;

    /// <summary>�������W</summary>
    public int init_locx = 0;
    /// <summary>�������W</summary>
    public int init_locy = 0;
    /// <summary>��������</summary>
    public int init_dir_x = 1;
    /// <summary>��������</summary>
    public int init_dir_y = 1;

    #endregion

    #region �ϐ�

    /// <summary>���݂̌���</summary>
    protected CharaDirection nowDirection;

    /// <summary>���ݍ��W</summary>
    protected Vector2Int location;

    /// <summary>�ő�HP</summary>
    public int param_HP_max;
    /// <summary>����HP</summary>
    protected int param_HP;
    /// <summary>�U���͕ϓ��l</summary>
    protected float param_ATK_rate;
    /// <summary>��{���x</summary>
    public int param_SPD_base;
    /// <summary>���x�ϓ��l</summary>
    protected float param_SPD_rate;
    /// <summary>�s���҂�����</summary>
    protected int param_wait_time;

    /// <summary>�g�p�\�X�L��</summary>
    protected List<BossGameBDataBase.SkillID> skillList;

    /// <summary>�L��������</summary>
    public CharaType CharacterType { get; set; }

    /// <summary>�_���[�W���o�Đ���</summary>
    private bool damageEffecting = false;

    /// <summary>���G�t���O</summary>
    protected bool isInvincible = false;

    #endregion

    #region ������

    /// <summary>
    /// ������
    /// </summary>
    protected virtual void Start()
    {
        SetDirection(init_dir_x, init_dir_y);
        location = new Vector2Int(init_locx, init_locy);
        transform.localPosition = BossGameSystemB.GetCellPosition(location);

        skillList = new List<BossGameBDataBase.SkillID>();
    }

    #endregion

    #region �@�\

    /// <summary>
    /// �L����ID�@�h����Ŏ���
    /// </summary>
    /// <returns></returns>
    public virtual BossGameSystemB.CharacterID GetCharacterID() { return BossGameSystemB.CharacterID.Player; }

    /// <summary>
    /// Vector���������ݒ�
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void SetDirection(int x, int y)
    {
        // �㉺�E���E���t���O����
        var isRight = false;
        var isUp = false;

        if (x > 0) isRight = true;
        else if (x < 0) isRight = false;
        else isRight = nowDirection == CharaDirection.RightUp || nowDirection == CharaDirection.RightDown;

        if (y > 0) isUp = true;
        else if (y < 0) isUp = false;
        else isUp = nowDirection == CharaDirection.RightUp || nowDirection == CharaDirection.LeftUp;


        // �E��
        if (isRight && isUp) SetDirection(CharaDirection.RightUp);
        // �E��
        else if (isRight && !isUp) SetDirection(CharaDirection.RightDown);
        // ����
        else if (!isRight && isUp) SetDirection(CharaDirection.LeftUp);
        // ����
        else SetDirection(CharaDirection.LeftDown);
    }

    /// <summary>
    /// ������ݒ�
    /// </summary>
    /// <param name="dir"></param>
    public void SetDirection(CharaDirection dir)
    {
        // �摜�ύX
        model.sprite = dir switch
        {
            CharaDirection.LeftUp => sp_leftup,
            CharaDirection.RightUp => sp_rightup,
            CharaDirection.RightDown => sp_rightdown,
            _ => sp_leftdown,
        };

        nowDirection = dir;
    }

    /// <summary>
    /// ���݂̌���
    /// </summary>
    /// <returns></returns>
    public CharaDirection GetDirection() { return nowDirection; }

    /// <summary>
    /// ���݂̌����ɉ摜�����Z�b�g
    /// </summary>
    public void ResetDirection() { SetDirection(nowDirection); }

    /// <summary>
    /// ���݈ʒu
    /// </summary>
    /// <returns></returns>
    public Vector2Int GetLocation() { return location; }

    #region �p�����[�^�Ǘ�

    /// <summary>
    /// �p�����[�^������
    /// </summary>
    public void InitParameter()
    {
        param_ATK_rate = 1f;
        param_SPD_rate = 1f;
        param_HP = param_HP_max;

        param_wait_time = CalcWaitTime(param_SPD_base);
    }

    /// <summary>
    /// ��{�̍s���ҋ@����
    /// </summary>
    /// <returns></returns>
    public int GetMaxWaitTime() { return CalcWaitTime(Mathf.FloorToInt(param_SPD_base * param_SPD_rate)); }
    /// <summary>
    /// ���݂̑҂�����
    /// </summary>
    /// <returns></returns>
    public int GetWaitTime() { return param_wait_time; }
    /// <summary>
    /// �҂����Ԃ����炷
    /// </summary>
    /// <param name="t"></param>
    public void DecreaseTime(int t) { param_wait_time -= t; }
    /// <summary>
    /// �s���x��
    /// </summary>
    /// <param name="turn">�x����</param>
    public void DelayTime(int turn) { param_wait_time += GetMaxWaitTime(); }

    /// <summary>
    /// �҂����Ԃ��Đݒ�
    /// </summary>
    /// <param name="init"></param>
    public virtual void ResetTime(bool init = false) { param_wait_time = GetMaxWaitTime(); }
    /// <summary>
    /// ���x�ω�
    /// </summary>
    /// <param name="mul"></param>
    public void ChangeSpeed(float mul)
    {
        var before = param_SPD_rate;
        param_SPD_rate *= mul;

        // �҂����Ԃ�ϓ�
        ChangeWaitTimeRate(before, param_SPD_rate);
    }

    /// <summary>
    /// �U���͕ω�
    /// </summary>
    /// <param name="mul"></param>
    public void ChangeAttackRate(float mul)
    {
        param_ATK_rate *= mul;
    }

    /// <summary>
    /// �o�t�E�f�o�t�����Z�b�g
    /// </summary>
    public void ResetParam()
    {
        // �҂����Ԃ�ϓ�
        ChangeWaitTimeRate(param_SPD_rate, 1f);

        param_ATK_rate = 1f;
        param_SPD_rate = 1f;
    }

    /// <summary>
    /// SPD�ω��ɂ��҂����Ԃ�ϓ�
    /// </summary>
    /// <param name="beforeRate"></param>
    /// <param name="afterRate"></param>
    private void ChangeWaitTimeRate(float beforeRate, float afterRate)
    {
        var beforeTimeMax = CalcWaitTime(Mathf.FloorToInt(param_SPD_base * beforeRate));
        var afterTimeMax = CalcWaitTime(Mathf.FloorToInt(param_SPD_base * afterRate));
        var spdRate = (float)afterTimeMax / beforeTimeMax;

        var newTime = param_wait_time * spdRate;
        if (afterTimeMax > beforeTimeMax)
        {
            // �����鎞�͐؂艺��
            param_wait_time = Mathf.FloorToInt(newTime);
        }
        else
        {
            // ���鎞�͐؂�グ
            param_wait_time = Mathf.CeilToInt(newTime);
        }
    }

    /// <summary>
    /// ����HP
    /// </summary>
    /// <returns></returns>
    public int GetHp() { return param_HP; }
    /// <summary>
    /// �ő�HP
    /// </summary>
    /// <returns></returns>
    public int GetHpMax() { return param_HP_max; }

    /// <summary>
    /// ���G��
    /// </summary>
    /// <returns></returns>
    public bool IsInvincible() { return isInvincible; }

    #endregion

    #endregion

    #region �ėp���\�b�h

    /// <summary>
    /// SPD�ɂ�肩����W���ҋ@����
    /// </summary>
    /// <param name="spd"></param>
    /// <returns></returns>
    protected static int CalcWaitTime(int spd)
    {
        var tmp = 100 - spd;
        if (tmp < 10) tmp = 10;
        return tmp;
    }

    /// <summary>
    /// �X�L���������鑊�������
    /// </summary>
    /// <param name="skillID"></param>
    /// <param name="center"></param>
    /// <returns></returns>
    protected List<BossGameBCharacterBase> GetSkillHitCharacters(BossGameBDataBase.SkillID skillID, Vector2Int center)
    {
        var skill = BossGameBDataBase.SkillList[skillID];
        var list = new List<BossGameBCharacterBase>();

        for (var x = center.x - skill.EffectRange; x <= center.x + skill.EffectRange; ++x)
            for (var y = center.y - skill.EffectRange; y <= center.y + skill.EffectRange; ++y)
            {
                var chara = system.GetCellCharacter(new Vector2Int(x, y));
                if (chara == null) continue;
                if (skill.TargetType == BossGameBDataBase.TargetTypeEnum.Fellow && chara.CharacterType != CharacterType)
                    continue;
                if (skill.TargetType == BossGameBDataBase.TargetTypeEnum.Enemy && chara.CharacterType == CharacterType)
                    continue;

                if (!list.Contains(chara)) list.Add(chara);
            }

        return list;
    }

    /// <summary>
    /// �x�N�g���ɊY������Direction��I��
    /// </summary>
    /// <param name="vec"></param>
    /// <returns></returns>
    protected CharaDirection GetVectorDirection(Vector2Int vec)
    {
        var nowDirVec = BossGameSystemB.GetDirectionCell(nowDirection);
        // x��0�̏ꍇ�͌��݂̌������̗p
        if (vec.x == 0) vec.x = nowDirVec.x;
        // y��0�̏ꍇ�͌��݂̌������̗p
        if (vec.y == 0) vec.y = nowDirVec.y;

        if (vec.x > 0 && vec.y > 0) return CharaDirection.RightUp;
        else if (vec.x > 0 && vec.y < 0) return CharaDirection.RightDown;
        else if (vec.x < 0 && vec.y > 0) return CharaDirection.LeftUp;
        else return CharaDirection.LeftDown;
    }

    #endregion

    #region �R���[�`��

    /// <summary>
    /// �����ړ�
    /// </summary>
    /// <param name="x">�ړ�����</param>
    /// <param name="y">�ړ�����</param>
    /// <returns></returns>
    public IEnumerator Walk(int x, int y)
    {
        /// �摜�ύX
        SetDirection(x, y);

        location.x += x;
        location.y += y;

        var deltaPos = new DeltaVector3();
        deltaPos.Set(transform.localPosition);
        deltaPos.MoveTo(BossGameSystemB.GetCellPosition(location), 0.2f, DeltaFloat.MoveType.LINE);
        while (deltaPos.IsActive())
        {
            yield return null;
            deltaPos.Update(Time.deltaTime);
            transform.localPosition = deltaPos.Get();
        }
    }

    /// <summary>
    /// �ӂ��Ƃ΂���
    /// </summary>
    /// <param name="x">�ړ�����</param>
    /// <param name="y">�ړ�����</param>
    /// <returns></returns>
    public IEnumerator Slide(int x, int y)
    {
        location.x += x;
        location.y += y;

        var deltaPos = new DeltaVector3();
        deltaPos.Set(transform.localPosition);
        deltaPos.MoveTo(BossGameSystemB.GetCellPosition(location), 0.05f, DeltaFloat.MoveType.LINE);
        while (deltaPos.IsActive())
        {
            yield return null;
            deltaPos.Update(Time.deltaTime);
            transform.localPosition = deltaPos.Get();
        }
    }

    /// <summary>
    /// ���W���̂܂܂ŉ摜�݈̂ړ�
    /// </summary>
    /// <param name="targetLoc">�ړ���̍��W</param>
    /// <param name="time">�ړ�����</param>
    /// <param name="jumpH">�W�����v����ꍇ</param>
    /// <returns></returns>
    public IEnumerator EffectMove(Vector2Int targetLoc, float time, float jumpH = 0f)
    {
        var jumpR = new DeltaFloat();
        jumpR.Set(0f);
        jumpR.MoveTo(Mathf.PI, time, DeltaFloat.MoveType.LINE);
        var deltaPos = new DeltaVector3();
        deltaPos.Set(transform.localPosition);
        deltaPos.MoveTo(BossGameSystemB.GetCellPosition(targetLoc), time, DeltaFloat.MoveType.LINE);
        while (deltaPos.IsActive())
        {
            yield return null;
            deltaPos.Update(Time.deltaTime);
            jumpR.Update(Time.deltaTime);

            var pos = deltaPos.Get();
            pos += new Vector3(0, Mathf.Sin(jumpR.Get()) * jumpH);
            transform.localPosition = pos;
        }
    }

    /// <summary>
    /// �^�[���s��
    /// </summary>
    /// <returns></returns>
    public IEnumerator TurnProcessBase()
    {
        model.sortingOrder = 10;
        isInvincible = false;

        yield return TurnProcess();

        model.sortingOrder = 0;
    }

    /// <summary>
    /// �^�[���s���{��
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator TurnProcess()
    {
        // �h����Ŏ���
        yield return new WaitForSeconds(1f);
    }

    /// <summary>
    /// �X�L���g�p
    /// </summary>
    /// <param name="skillID"></param>
    /// <param name="targetCell"></param>
    /// <returns></returns>
    protected IEnumerator UseSkillBase(BossGameBDataBase.SkillID skillID, Vector2Int targetCell)
    {
        var skill = BossGameBDataBase.SkillList[skillID];
        var nameUI = system.skillNameUI;
        var cells = system.cellUI;

        // �X�L�����\��
        nameUI.Show(skill.Name);

        // �g������������
        var dir = targetCell - GetLocation();
        SetDirection(dir.x, dir.y);

        // �����ҋ@
        if (CharacterType == CharaType.Player)
        {
            yield return new WaitForSeconds(0.3f);
        }
        else
        {
            // CPU�͑I��UI�\��
            var ui = system.cellUI;
            var cellList = BossGameSystemB.CreateEnableCellList(skillID, this);
            var cursor = new BossGameBUICellSelect.CellUIParam(targetCell, skill.EffectRange * 2 + 1, skill.EffectRange * 2 + 1, false);
            ui.Show(cellList, new List<BossGameBUICellSelect.CellUIParam>() { cursor });
            yield return new WaitForSeconds(1.0f);
            ui.Hide();
        }

        // �g�p
        // �G�����g����X�L���̏ꍇ�����Ŏ���
        var generalSkills = new[]
        {
            BossGameBDataBase.SkillID.Ami1,
            BossGameBDataBase.SkillID.Mana1,
            BossGameBDataBase.SkillID.Matuka1,
            BossGameBDataBase.SkillID.Mati1,
            BossGameBDataBase.SkillID.Menderu1,
            BossGameBDataBase.SkillID.Pierre1,
        };
        if (generalSkills.Contains(skillID))
        {
            yield return UseGeneralSkill(skillID, targetCell);
        }
        else
        {
            yield return UseSkill(skillID, targetCell);
        }

        // �X�L������\��
        nameUI.Hide();

        // �����G���Z�b�g
        ResetDirection();
    }

    /// <summary>
    /// �X�L���g�p�{��
    /// </summary>
    /// <param name="skillID"></param>
    /// <param name="targetCell"></param>
    /// <returns></returns>
    protected virtual IEnumerator UseSkill(BossGameBDataBase.SkillID skillID, Vector2Int targetCell)
    {
        yield return null;
    }

    /// <summary>
    /// �G�������ʃX�L��
    /// </summary>
    /// <returns></returns>
    protected IEnumerator UseGeneralSkill(BossGameBDataBase.SkillID skillID, Vector2Int targetCell)
    {
        var skill = BossGameBDataBase.SkillList[skillID];
        var sound = ManagerSceneScript.GetInstance().soundMan;
        yield return null;

        var targetList = GetSkillHitCharacters(skillID, targetCell);

        switch (skillID)
        {
            case BossGameBDataBase.SkillID.Ami1: // �͂�̂ƂȂ�
                sound.PlaySE(system.dataObj.se_skill_harunotonari);
                yield return system.PlayHealEffect(location, 3, 3);
                // ��
                yield return AttackDamage(targetList, -skill.Value);
                // �̓A�b�v
                yield return system.BuffAttack(targetList, 1.1f);
                // ���x�A�b�v
                yield return system.BuffSpeed(targetList, 1.1f);
                yield return new WaitForSeconds(0.5f);
                break;
            case BossGameBDataBase.SkillID.Mana1: //�V���E�_�E��
                //todo:
                break;
            case BossGameBDataBase.SkillID.Mati1: // ���߂̌��a��
                {
                    var dist = targetCell - location;
                    yield return EffectMove(targetCell + dist, 0.03f);
                    system.CreateSlashEffect(targetCell, BossGameSystemB.GetCellPosition(dist));
                    sound.PlaySE(system.dataObj.se_skill_setuna);
                    yield return new WaitForSeconds(0.7f);
                    yield return EffectMove(location, 0.5f, 270f);
                    yield return AttackDamage(targetList, Mathf.FloorToInt(skill.Value * param_ATK_rate));
                }
                break;
            case BossGameBDataBase.SkillID.Matuka1: // ��
                {
                    ManagerSceneScript.GetInstance().mainCam.PlayShakeOne(Shaker.ShakeSize.Weak);
                    sound.PlayVoice(system.dataObj.se_skill_katu);
                    yield return new WaitForSeconds(1f);
                    targetList.Remove(this);
                    yield return AttackDamage(targetList, Mathf.FloorToInt(skill.Value * param_ATK_rate));
                    foreach (var chara in targetList)
                    {
                        // �m���ōs�����ԑ���
                        if (Util.RandomCheck(70)) chara.DelayTime(1);
                    }
                }
                break;
            case BossGameBDataBase.SkillID.Menderu1: // �}���g���b�v���@�C��
                //todo:
                break;
            case BossGameBDataBase.SkillID.Pierre1: // �W���O�����O�q�b�g
                //todo:
                break;
        }
    }

    /// <summary>
    /// �_���[�W������
    /// </summary>
    /// <param name="dmg"></param>
    /// <returns></returns>
    public IEnumerator HitDamage(int dmg)
    {
        damageEffecting = true;
        if (isInvincible) dmg = 0;
        else if (dmg > 9999) dmg = 9999;
        var basePos = BossGameSystemB.GetCellPosition(location);

        for (var i = 0; i < 12; ++i)
        {
            var addX = (1f - (Mathf.Pow(i, 2) / 144f)) * 10f;
            transform.localPosition = basePos + new Vector3(addX, 0);
            yield return new WaitForSeconds(0.03f);
            transform.localPosition = basePos + new Vector3(-addX, 0);
            yield return new WaitForSeconds(0.03f);
        }
        transform.localPosition = basePos;

        // �_���[�WUI�\��
        yield return system.ShowDamage(location, dmg);

        param_HP -= dmg;
        if (param_HP < 0)
        {
            // ���S���o���ď���
            param_HP = 0;
            yield return DeadEffect();
            system.RemoveCharacter(this);
        }

        damageEffecting = false;
    }

    /// <summary>
    /// �_���[�W�\����
    /// </summary>
    /// <returns></returns>
    public bool IsDamageEffecting()
    {
        return damageEffecting;
    }

    /// <summary>
    /// ���S���o
    /// </summary>
    protected virtual IEnumerator DeadEffect()
    {
        for (var i = 0; i < 14; ++i)
        {
            model.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.03f);
            model.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.03f);
        }

        model.gameObject.SetActive(false);
    }

    /// <summary>
    /// �񕜂���
    /// </summary>
    /// <param name="heal"></param>
    /// <returns></returns>
    public IEnumerator HealDamage(int heal)
    {
        damageEffecting = true;

        // �񕜐����\��
        yield return system.ShowDamage(location, -heal);

        param_HP += heal;
        if (param_HP > param_HP_max) param_HP = param_HP_max;

        damageEffecting = false;
    }

    /// <summary>
    /// �_���[�W��^���� �L�������Ƃɗ����ŏ㉺
    /// </summary>
    /// <param name="targets"></param>
    /// <param name="baseDamage">�}�C�i�X�ŉ�</param>
    /// <returns></returns>
    protected IEnumerator AttackDamage(List<BossGameBCharacterBase> targets, int baseDamage)
    {
        if (targets.Count == 0)
        {
            yield return new WaitForSeconds(0.5f);
            yield break;
        }

        foreach (var t in targets)
        {
            var dmg = Util.RandomInt(Mathf.FloorToInt(baseDamage * 0.8f), baseDamage);
            if (dmg >= 0)
            {
                // �t�����h���[�t�@�C�A�͌y��
                if (t.CharacterType == this.CharacterType) dmg = Mathf.FloorToInt(dmg * 0.4f);
                StartCoroutine(t.HitDamage(dmg));
            }
            else
                StartCoroutine(t.HealDamage(-dmg));
        }

        // �Đ��҂�
        foreach (var t in targets)
        {
            yield return new WaitWhile(() => t.IsDamageEffecting());
        }
    }

    #endregion
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// ���X�{�X�{��@�G�L������{
/// </summary>
public class BossGameBEnemy : BossGameBCharacterBase
{
    /// <summary>
    /// ������
    /// </summary>
    protected override void Start()
    {
        base.Start();

        CharacterType = CharaType.Enemy;
    }

    /// <summary>
    /// �^�[���s��
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator TurnProcess()
    {
        var sound = ManagerSceneScript.GetInstance().soundMan;
        // �s������
        var ai = DecideAI();

        switch (ai.Act)
        {
            case AIResult.ActionType.Skill:
                yield return UseSkillBase(ai.SkillID, ai.SkillTargetLoc);
                system.playerWalkReset = true;
                break;
            case AIResult.ActionType.ChangeDir:
                SetDirection(ai.ChangeDir);
                break;
            case AIResult.ActionType.Walk:
                yield return Walk(ai.WalkLoc.x, ai.WalkLoc.y);
                // �n�`���ʃ`�F�b�N
                var effect = system.GetCellFieldEffect(location);
                if (effect == BossGameBDataObject.FieldEffect.Plasma)
                {
                    // �_���[�W������
                    sound.PlaySE(system.dataObj.se_field_plasma);
                    yield return HitDamage(Util.RandomInt(160, 240), true);
                    system.ClearFieldEffect(location);
                }
                break;
        }

        yield return null;
    }

    /// <summary>
    /// �s�����苤��
    /// ����AI�͔h���ŃI�[�o�[���C�h
    /// </summary>
    protected virtual AIResult DecideAI()
    {
        var ret = new AIResult();

        // �g�p�\�ȃX�L��������
        var enableSkillList = new Dictionary<BossGameBDataBase.SkillID, List<Vector2Int>>();
        foreach (var sid in skillList)
        {
            var sdata = BossGameBDataBase.SkillList[sid];
            var cells = BossGameSystemB.CreateEnableCellList(sid, this);
            var targetCellList = new List<Vector2Int>();
            foreach (var cell in cells)
            {
                var chr = system.GetCellCharacter(cell);
                if (chr == null) continue;
                if (sdata.TargetType == BossGameBDataBase.TargetTypeEnum.Fellow && chr.CharacterType != this.CharacterType) continue;
                if (sdata.TargetType == BossGameBDataBase.TargetTypeEnum.Enemy && chr.CharacterType == this.CharacterType) continue;

                targetCellList.Add(cell);
                break;
            }
            if (targetCellList.Any())
                enableSkillList.Add(sid, targetCellList);
        }

        // �������炻�̒����烉���_���őI��
        if (enableSkillList.Any())
        {
            ret.Act = AIResult.ActionType.Skill;
            var idList = enableSkillList.Keys.ToList();
            var sid = idList[Util.RandomInt(0, enableSkillList.Count - 1)];
            var sdata = BossGameBDataBase.SkillList[sid];
            var cellList = enableSkillList[sid];
            ret.SkillID = sid;
            ret.SkillTargetLoc = cellList[Util.RandomInt(0, cellList.Count - 1)];
            return ret;
        }

        // ������΃v���C���[�ɋ߂Â�����
        var pLoc = system.GetPlayerLoc();
        var dist = pLoc - location;

        // ����1�Ȃ�v���C���[�̕��������ďI��
        if (Mathf.Abs(dist.x) <= 1 && Mathf.Abs(dist.y) <= 1)
        {
            ret.Act = AIResult.ActionType.ChangeDir;
            ret.ChangeDir = GetVectorDirection(dist);
            return ret;
        }

        // ���W���傫������ڕW
        var walkTarget = new Vector2Int(0, 0);
        if (Mathf.Abs(dist.x) >= Mathf.Abs(dist.y))
        {
            walkTarget.x = dist.x > 0 ? 1 : -1;
            // ���������������������ړ��s�Ȃ�����Е�
            if (!system.CanWalk(location + walkTarget))
            {
                walkTarget.x = 0;
                walkTarget.y = dist.y > 0 ? 1 : -1;
            }
        }
        else
        {
            walkTarget.y = dist.y > 0 ? 1 : -1;
            if (!system.CanWalk(location + walkTarget))
            {
                walkTarget.y = 0;
                walkTarget.x = dist.x > 0 ? 1 : -1;
            }
        }

        // �ǂ�����ړ��s�Ȃ�v���C���[�̕��Ɍ�����ς��邾���ŏI���
        if (!system.CanWalk(location + walkTarget))
        {
            ret.Act = AIResult.ActionType.ChangeDir;
            ret.ChangeDir = GetVectorDirection(dist);
            return ret;
        }

        // �ړ������������Ɍ����Ă��邩�`�F�b�N
        var vecDir = BossGameSystemB.GetDirectionCell(nowDirection);
        bool checkWalkDir = walkTarget.x > 0 && vecDir.x > 0 ||
            walkTarget.x < 0 && vecDir.x < 0 ||
            walkTarget.y > 0 && vecDir.y > 0 ||
            walkTarget.y < 0 && vecDir.y < 0;

        if (checkWalkDir)
        {
            // ����OK�Ȃ�����Ŋm��
            ret.Act = AIResult.ActionType.Walk;
            ret.WalkLoc = walkTarget;
            return ret;
        }

        // OK����Ȃ��Ȃ�v���C���[�̕��Ɍ�����ς��ďI���
        ret.Act = AIResult.ActionType.ChangeDir;
        ret.ChangeDir = GetVectorDirection(dist);
        return ret;
    }

    #region �s������N���X

    /// <summary>
    /// AI�I�����N���X
    /// </summary>
    protected class AIResult
    {
        public enum ActionType
        {
            /// <summary>�X�L���g�p</summary>
            Skill = 0,
            /// <summary>������ς���</summary>
            ChangeDir,
            /// <summary>����</summary>
            Walk,
            /// <summary></summary>
            None,
        }

        /// <summary>�A�N�V�������</summary>
        public ActionType Act;

        /// <summary>�X�L���g�p�̏ꍇ�@ID</summary>
        public BossGameBDataBase.SkillID SkillID;
        /// <summary>�X�L���g�p�̏ꍇ�@�ΏۃZ��</summary>
        public Vector2Int SkillTargetLoc;

        /// <summary>�����ύX�̏ꍇDir</summary>
        public CharaDirection ChangeDir;

        /// <summary>�����ꍇ�@�ړ���</summary>
        public Vector2Int WalkLoc;
    }
    #endregion
}

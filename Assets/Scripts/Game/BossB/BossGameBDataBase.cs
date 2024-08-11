using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���X�{�X�{��f�[�^�x�[�X
/// </summary>
public static class BossGameBDataBase
{
    #region �X�L���f�[�^

    /// <summary>
    /// �˒������̃^�C�v
    /// </summary>
    public enum RangeTypeEnum : int
    {
        /// <summary>�S����</summary>
        All = 0,
        /// <summary>�����̂ݑS����</summary>
        AllLine,
        /// <summary>�΂߂̂ݑS����</summary>
        AllCrossLine,
        /// <summary>�c���̂ݑS����</summary>
        AllPlusLine,
        /// <summary>�O���i�΂߁j�E���E�i�c�j�̂ݒ���</summary>
        ThreeLine,
        /// <summary>�O���i�΂߁j�̂ݒ���</summary>
        FrontLine,
    }

    /// <summary>
    /// �ΏۑI���^�C�v
    /// </summary>
    public enum TargetTypeEnum : int
    {
        /// <summary>�Ȃ�ł�</summary>
        All = 0,
        /// <summary>�i�g�p�҂́j�G</summary>
        Enemy,
        /// <summary>�i�g�p�҂́j����</summary>
        Fellow,
    }

    /// <summary>
    /// �X�L��
    /// </summary>
    public enum SkillID : int
    {
        /// <summary>�T�C�N���g����</summary>
        Reko1_C = 0,
        /// <summary>�I�N�g�X�g���C�N</summary>
        Reko2_O,
        /// <summary>�C�[�O���_�C�u</summary>
        Reko3_E,
        /// <summary>�C���r���V�u��</summary>
        Reko4_I,
        /// <summary>���C���J�[�l�[�V����</summary>
        Reko5_R,
        /// <summary>�I���W��</summary>
        Reko6_O,
        /// <summary>�C�O�j�b�V����</summary>
        Reko7_I,
        /// <summary>�i�C�g���A</summary>
        Reko8_N,
        /// <summary>�L�[�j���O�V���t</summary>
        Reko9_K,
        /// <summary>�J���l�[�W</summary>
        Boss1_Carnage,
        /// <summary>�v���Y�}</summary>
        Boss2_Plasma,
        /// <summary>�A�[���X�g�����O</summary>
        Boss3_Canon,
        /// <summary>�g�����L�[���C�U�[</summary>
        Boss4_Tranqui,
        /// <summary>�͂�̂ƂȂ�</summary>
        Ami1,
        /// <summary>�V���E�_�E��</summary>
        Mana1,
        /// <summary>��</summary>
        Matuka1,
        /// <summary>���߂̌��a��</summary>
        Mati1,
        /// <summary>�}���g���b�v���@�C��</summary>
        Menderu1,
        /// <summary>�W���O�����O�q�b�g</summary>
        Pierre1,
    }

    /// <summary>
    /// �X�L���f�[�^�N���X
    /// </summary>
    public class SkillData
    {
        /// <summary>����</summary>
        public string Name;
        /// <summary>����</summary>
        public string Detail;

        /// <summary>�˒������̌���</summary>
        public RangeTypeEnum RangeType;
        /// <summary>�˒��������X�g 0:����</summary>
        public List<int> RangeList;
        /// <summary>���ʔ͈́i0�Ȃ�P�́A1�Ŏ���1�}�X��3x3</summary>
        public int EffectRange;

        /// <summary>�I��Ώ�</summary>
        public TargetTypeEnum TargetType;

        /// <summary>���ʗ�</summary>
        public int Value;

        /// <summary>
        /// �������R���X�g���N�^
        /// ���Ɏw�肵�Ȃ���Ύ����Ώۂ̐ݒ�
        /// </summary>
        public SkillData()
        {
            Name = "";
            Detail = "";
            RangeType = RangeTypeEnum.All;
            RangeList = new List<int>() { 0 };
            EffectRange = 0;
            TargetType = TargetTypeEnum.All;
            Value = 0;
        }
    }

    private static Dictionary<SkillID, SkillData> _skillList = null;
    /// <summary>
    /// �X�L�����X�g
    /// </summary>
    public static Dictionary<SkillID, SkillData> SkillList
    {
        get
        {
            if (_skillList == null)
            {
                _skillList = new Dictionary<SkillID, SkillData>();

                #region ���R�X�L��
                _skillList.Add(SkillID.Reko1_C, new SkillData()
                {
                    Name = StringMinigameMessage.BossB_SkillA1_Name,
                    Detail = StringMinigameMessage.BossB_SkillA1_Detail,
                    Value = 120,
                });
                _skillList.Add(SkillID.Reko2_O, new SkillData()
                {
                    Name = StringMinigameMessage.BossB_SkillA2_Name,
                    Detail = StringMinigameMessage.BossB_SkillA2_Detail,
                    TargetType = BossGameBDataBase.TargetTypeEnum.Enemy,
                    RangeList = new List<int>() { 1 },
                    Value = 888,
                });
                _skillList.Add(SkillID.Reko3_E, new SkillData()
                {
                    Name = StringMinigameMessage.BossB_SkillA3_Name,
                    Detail = StringMinigameMessage.BossB_SkillA3_Detail,
                    TargetType = BossGameBDataBase.TargetTypeEnum.Enemy,
                    RangeType = BossGameBDataBase.RangeTypeEnum.AllLine,
                    RangeList = new List<int>() { 3 },
                    Value = 500,
                });
                _skillList.Add(SkillID.Reko4_I, new SkillData()
                {
                    Name = StringMinigameMessage.BossB_SkillA4_Name,
                    Detail = StringMinigameMessage.BossB_SkillA4_Detail,
                });
                _skillList.Add(SkillID.Reko5_R, new SkillData()
                {
                    Name = StringMinigameMessage.BossB_SkillA5_Name,
                    Detail = StringMinigameMessage.BossB_SkillA5_Detail,
                });
                _skillList.Add(SkillID.Reko6_O, new SkillData()
                {
                    Name = StringMinigameMessage.BossB_SkillA6_Name,
                    Detail = StringMinigameMessage.BossB_SkillA6_Detail,
                    EffectRange = 6,
                });
                _skillList.Add(SkillID.Reko7_I, new SkillData()
                {
                    Name = StringMinigameMessage.BossB_SkillA7_Name,
                    Detail = StringMinigameMessage.BossB_SkillA7_Detail,
                    Value = 120,
                });
                _skillList.Add(SkillID.Reko8_N, new SkillData()
                {
                    Name = StringMinigameMessage.BossB_SkillA8_Name,
                    Detail = StringMinigameMessage.BossB_SkillA8_Detail,
                    TargetType = BossGameBDataBase.TargetTypeEnum.Enemy,
                    RangeType = BossGameBDataBase.RangeTypeEnum.All,
                    RangeList = new List<int>() { 1, 2 },
                    EffectRange = 1,
                    Value = 300,
                });
                _skillList.Add(SkillID.Reko9_K, new SkillData()
                {
                    Name = StringMinigameMessage.BossB_SkillA9_Name,
                    Detail = StringMinigameMessage.BossB_SkillA9_Detail,
                    TargetType = TargetTypeEnum.Enemy,
                    EffectRange = 2,
                    Value = 500,
                });

                #endregion

                #region �{�X
                _skillList.Add(SkillID.Boss1_Carnage, new SkillData()
                {
                    Name = StringMinigameMessage.BossB_SkillBBoss1_Name,
                    EffectRange = 6,
                    RangeList = new List<int>() { 0, 1, 2, 3, 4, 5, 6 },
                    Value = 800,
                });
                _skillList.Add(SkillID.Boss2_Plasma, new SkillData()
                {
                    Name = StringMinigameMessage.BossB_SkillBBoss2_Name,
                    TargetType = TargetTypeEnum.Enemy,
                    EffectRange = 6,
                    RangeList = new List<int>() { 0, 1, 2, 3, 4, 5, 6 },
                    Value = 300,
                });
                _skillList.Add(SkillID.Boss3_Canon, new SkillData()
                {
                    Name = StringMinigameMessage.BossB_SkillBBoss3_Name,
                    RangeType = RangeTypeEnum.AllCrossLine,
                    RangeList = new List<int>() { 0, 1, 2, 3, 4, 5, 6 },
                    Value = 1300,
                });
                _skillList.Add(SkillID.Boss4_Tranqui, new SkillData()
                {
                    Name = StringMinigameMessage.BossB_SkillBBoss4_Name,
                    RangeType = RangeTypeEnum.AllPlusLine,
                    RangeList = new List<int>() { 0, 1, 2, 3, 4, 5, 6 },
                    Value = 300,
                });

                #endregion

                #region ��

                // �A�~
                _skillList.Add(SkillID.Ami1, new SkillData()
                {
                    Name = StringMinigameMessage.BossB_SkillBAmi1_Name,
                    Detail = StringMinigameMessage.BossB_SkillBAmi1_Detail,
                    TargetType = TargetTypeEnum.Fellow,
                    EffectRange = 1,
                    Value = 1100,
                });
                // MANA
                _skillList.Add(SkillID.Mana1, new SkillData()
                {
                    Name = StringMinigameMessage.BossB_SkillBMana1_Name,
                    Detail = StringMinigameMessage.BossB_SkillBMana1_Detail,
                    TargetType = TargetTypeEnum.Fellow,
                    EffectRange = 6,
                });
                // �܂��肷��
                _skillList.Add(SkillID.Matuka1, new SkillData()
                {
                    Name = StringMinigameMessage.BossB_SkillBMatuka1_Name,
                    Detail = StringMinigameMessage.BossB_SkillBMatuka1_Detail,
                    EffectRange = 6,
                    Value = 150,
                });
                // �����f��
                _skillList.Add(SkillID.Menderu1, new SkillData()
                {
                    Name = StringMinigameMessage.BossB_SkillBMenderu1_Name,
                    Detail = StringMinigameMessage.BossB_SkillBMenderu1_Detail,
                    TargetType = TargetTypeEnum.Enemy,
                    RangeList = new List<int>() { 1, 2 },
                    EffectRange = 1,
                    Value = 100,
                });
                // �}�`
                _skillList.Add(SkillID.Mati1, new SkillData()
                {
                    Name = StringMinigameMessage.BossB_SkillBMati1_Name,
                    Detail = StringMinigameMessage.BossB_SkillBMati1_Detail,
                    TargetType = TargetTypeEnum.Enemy,
                    RangeType = RangeTypeEnum.ThreeLine,
                    RangeList = new List<int>() { 1 },
                    Value = 1300,
                });
                // �s�G�[��
                _skillList.Add(SkillID.Pierre1, new SkillData()
                {
                    Name = StringMinigameMessage.BossB_SkillBPierre1_Name,
                    Detail = StringMinigameMessage.BossB_SkillBPierre1_Detail,
                    TargetType = TargetTypeEnum.Enemy,
                    RangeType = RangeTypeEnum.ThreeLine,
                    RangeList = new List<int>() { 1, 2 },
                    Value = 30,
                });

                #endregion
            }

            return _skillList;
        }
    }

    #endregion

}

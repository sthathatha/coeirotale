using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �~�j�Q�[���Ŏg�p���郁�b�Z�[�W
/// </summary>
public class StringMinigameMessage
{
    #region �}�`

    public const string MatiA_Title = "���z�Ԏa";
    public const string MatiA_Tutorial = "�u�I�v�}�[�N�������ꂽ��A���������{�^�����������I";

    public const string MatiB_Title = "���̘T�S";
    public const string MatiB_Tutorial = "�\�����������L�[�ƌ���{�^���������珇�ɂ������I\n�V�񐬌��ŏ����A�R�񎸔s�ŕ����I";

    #endregion

    #region �����f��

    public const string MenderuA_Title = "�����f���̎�";
    public const string MenderuA_Tutorial = "25�̎�����݂�1�`3��荇���܂��B\n" +
            "��̓^�[�����ڂ鎞�ɏ㉺���E�ɂ���󗓂̐������������A10�܂Ő�������Ǝ��Ȃ��Ȃ�܂��B\n" +
            "�����̃^�[����1�����Ȃ������ꍇ�A�����ƂȂ�܂��B";
    public const string MenderuA_Serif_Start = "�u�����f���̎�v�ŏ�����I";
    public const string MenderuA_Serif_PTurn0 = "�����A���Ȃ��̔Ԃ�";
    public const string MenderuA_Serif_ETurn0 = "���ꂶ�Ⴀ�@���́c";
    public const string MenderuA_Serif_ETurn0_1 = "����Ɓc";
    public const string MenderuA_Serif_ETurn0_2 = "����ˁI";
    public const string MenderuA_Serif_ETurn1_0 = "�ȁA�Ȃ�ł����āI";
    public const string MenderuA_Serif_ETurn1_1 = "����킪�ЂƂ�������I";
    public const string MenderuA_Serif_ETurn1_2 = "���̕����ˁc�������I";
    public const string MenderuA_Serif_ETurn2_0 = "����A��������킪������";
    public const string MenderuA_Serif_ETurn2_1 = "���̏����ˁA�܂��V�т܂��傤";

    public const string MenderuB_Title = "�����f���̎�";
    public const string MenderuB_Tutorial = "�������̐��������_�ƂȂ�A�Ō�ɂ�葽���̓_���l���������������ƂȂ�܂��B";

    #endregion

    #region �s�G�[��

    public const string PierreA_Title = "���n����";
    public const string PierreA_Tutorial = "�{�[���𓥂�ŉ������āA�s�G�[����ǂ������悤�I\n" +
            "�R��^�b�`�����珟���A�R�񂱂�񂾂畉�������I";
    public const string PierreA_Serif0 = "�w�C�w�C�w�[�C�I�I";
    public const string PierreA_Random0 = "�n�[�b�n�b�n�b�n�I";
    public const string PierreA_Random1 = "�B���b�z�H�H�H�E�I";
    public const string PierreA_Random2 = "�w�C�w�C�w�[�C�I";
    public const string PierreA_Random3 = "�ق�ق�ǂ������I";
    public const string PierreA_Random4 = "�܂��܂��������˂��I";
    public const string PierreA_Random5 = "����Ȃ�ǂ����ȁI";
    public const string PierreA_Random6 = "�����܂ł����ŁI";
    public const string PierreA_Win = "�܂������I�N�̏������I";
    public const string PierreA_Lose = "�A�b�n�b�n�c�O�I\n�܂����ł������ŁI";

    public const string PierreB_Title = "Say Ho!Yo!Yo!More!";
    public const string PierreB_Tutorial = "����{�^���Ń{�[���𔭎˂ł��邼�I\n�����܂��ɂ��I";

    public const string PierreB_Music = "��Conjurer";
    public const string PierreB_Spell1 = "�㉉�u�A�y�C�����̉΂̗ւ�����v";
    public const string PierreB_Spell2 = "�㉉�u�����u�����O�p���[�h�v";

    #endregion

    #region �܂��肷��

    public const string MatukaA_Title = "����";
    public const string MatukaA_Tutorial = "�Ƃɂ����A�ł���I";
    public const string MatukaA_Win = "���i\n�j���Ă��";
    public const string MatukaA_Lose = "�s���i";
    public const string MatukaA_Serif1 = "��{�������悤";
    public const string MatukaA_Serif2 = "�����@����Ă݂��܂�";
    public const string MatukaA_Naration1 = "��[��";
    public const string MatukaA_Naration2 = "�͂��߁I";
    public const string MatukaA_Naration3 = "�����܂ŁI";

    public const string MatukaB_Win = "����";
    public const string MatukaB_Lose = "����";

    #endregion

    #region MANA

    public const string ManaA_Title = "�X�s�[�h";
    public const string ManaA_Tutorial = "��ɂ���J�[�h���P�������P���Ȃ��J�[�h���o���Ă�\n" +
            "��ɑS���̃J�[�h���o�����ق��������I";
    public const string ManaA_Ready = "��[��";
    public const string ManaA_Go = "�����[�ƁI";
    public const string ManaA_Win = "�������傤��I";
    public const string ManaA_Lose = "�܂��c";

    public const string ManaB_Title = "���傤�X�s�[�h";
    public const string ManaB_Tutorial = "������ƂĂ��킢���I";

    #endregion

    #region �A�~

    public const string AmiA_Title = "�_���X�o�g���I";
    public const string AmiA_Tutorial = "�^�C�~���O�悭�㉺���E�������ėx�낤�I\n�\���{�^���AABXY�{�^���A\n�L�[�{�[�h���ADFJK�L�[�ŉ�";
    public const string AmiA_Great = "Great";
    public const string AmiA_Good = "Good";
    public const string AmiA_Bad = "Bad";

    public const string AmiB_Title = "�{�X���b�V��";
    public const string AmiB_Tutorial = "�ɂ����̂���������|���ƍŌ�̂����������L���ɂȂ��";

    #endregion

    #region ���X�{�X�{��

    public const string BossB_Win = "�����b�I�I";
    public const string BossB_Lose = "�s�k�c";

    public const string BossB_SkillA1_Name = "�T�C�N���g����";
    public const string BossB_SkillA1_Detail = "�b�F\n�s�����x�A�b�v";
    public const string BossB_SkillA2_Name = "�I�N�g�X�g���C�N";
    public const string BossB_SkillA2_Detail = "�n�F\n�A���̑Ō��ő�_���[�W��^����";
    public const string BossB_SkillA3_Name = "�C�[�O���_�C�u";
    public const string BossB_SkillA3_Detail = "�d�F\n���ꂽ�ꏊ�ɋ��͂ȕ����U��";
    public const string BossB_SkillA4_Name = "�C���r���V�u��";
    public const string BossB_SkillA4_Detail = "�h�F\n���̎����̃^�[���܂Ŗ��G";
    public const string BossB_SkillA5_Name = "���C���J�[�l�[�V����";
    public const string BossB_SkillA5_Detail = "�q�F\n�̗͂�S��";
    public const string BossB_SkillA6_Name = "�I���W��";
    public const string BossB_SkillA6_Detail = "�n�F\n�G�����S���̃X�e�[�^�X�ω��ƁA�S�Ă̒n�`���N���A";
    public const string BossB_SkillA7_Name = "�C�O�j�b�V����";
    public const string BossB_SkillA7_Detail = "�h�F\n�����U���̓A�b�v";
    public const string BossB_SkillA8_Name = "�i�C�g���A";
    public const string BossB_SkillA8_Detail = "�m�F\n�͈͂ɏ��_���[�W��^���A�܂�ɍs����x�点��";
    public const string BossB_SkillA9_Name = "�L�[�j���O�V���t";
    public const string BossB_SkillA9_Detail = "�j�F\n���͂̍L�͈͂ɕ��̃_���[�W";

    public const string BossB_SkillBAmi1_Name = "�͂�̂ƂȂ�";
    public const string BossB_SkillBAmi1_Detail = "�̗͂���\n���܂����f�����ƕ����U���̓A�b�v";
    public const string BossB_SkillBMana1_Name = "�V���E�_�E��";
    public const string BossB_SkillBMana1_Detail = "�����S�̂ɂȂɂ���������I";
    public const string BossB_SkillBMatuka1_Name = "��";
    public const string BossB_SkillBMatuka1_Detail = "�S�̂��Ј����čs����x�点��";
    public const string BossB_SkillBMati1_Name = "���߂̌��a��";
    public const string BossB_SkillBMati1_Detail = "�ڂɂ����܂�ʋ�������";
    public const string BossB_SkillBMenderu1_Name = "�}���g���b�v���@�C��";
    public const string BossB_SkillBMenderu1_Detail = "�c�^�n�`�𐶐����A���~�߂���";
    public const string BossB_SkillBPierre1_Name = "�W���O�����O�q�b�g";
    public const string BossB_SkillBPierre1_Detail = "�������ʂɓ������čU��";
    public const string BossB_SkillBBoss1_Name = "�J�[�l�C�W";
    public const string BossB_SkillBBoss2_Name = "�v���Y�}�t�B�[���h";
    public const string BossB_SkillBBoss3_Name = "�l�I�A�[���X�g�����O�T�C�N�����W�F�b�g�A�[���X�g�����O�C";
    public const string BossB_SkillBBoss4_Name = "�g�����L�[���C�U�[";

    #endregion
}

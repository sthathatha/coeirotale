using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �t�B�[���h�Ŏg�p���郁�b�Z�[�W
/// </summary>
public partial class StringFieldMessage
{
    #region �f�o�b�O

    // �Œ蕶��
    // �����������������������������������ĂƂȂɂʂ˂̂͂Ђӂւق܂݂ނ߂��������������
    // �A�C�E�G�I�J�L�N�P�R�T�V�X�Z�\�^�`�c�e�g�i�j�k�l�m�n�q�t�w�z�}�~����������������������������
    // �������������������������Âłǂ΂тԂׂڂς҂Ղ؂ۃK�M�O�Q�S�U�W�Y�[�]�_�a�d�f�h�o�r�u�x�{�p�s�v�y�|
    // �[�`
    // �����������������@�B�D�F�H�������b���������������
    // abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ
    // �����������������������������������������������������`�a�b�c�d�e�f�g�h�i�j�k�l�m�n�o�p�q�r�s�t�u�v�w�x�y
    // 0123456789�O�P�Q�R�S�T�U�V�W�X
    // �u�v"'�h�f

    public const string DebugMap_Menderu = "���ƗV�ԁH";
    public const string DebugMap_Pierre = "�l�ƗV�Ԃ����H";
    public const string DebugMap_Mati = "���ƗV�Ԃ̂��H";
    public const string DebugMap_Matuka = "���h���h���h���h���h���h���h�I�I";
    public const string DebugMap_Mana = "���[�H\nMANA����Ə������邩�[�H";
    public const string DebugMap_Ami = "��������Ⴂ�܂��I�V��ł����܂����H";
    public const string DebugMap_Ami2 = "�����Ɠ���Ȃ������ł����H";

    #endregion

    #region �`���[�g���A��

    public const string Tutorial000_01Reko = "����c�����́c";
    public const string Tutorial000_02Tukuyomi = "���ڊo�߂̂悤�ł���";
    public const string Tutorial000_03Reko = "�N�I�H";
    public const string Tutorial000_04Tukuyomi = "�����@���������Ă�������\n�ł邱�Ƃ͂���܂���";
    public const string Tutorial000_05Tukuyomi = "���Ȃ��ɂ��b������܂��@�����ł����H\n�ǂ����@����������";
    public const string Tutorial000_06Tukuyomi = "���Ȃ��͉��炩�̂ӂ����ȗ͂ɂЂ��ς��Ă����ɖ������݂܂���";
    public const string Tutorial000_07Tukuyomi = "�����@�����@�킩���Ă܂�\n���ł��ˁH";
    public const string Tutorial000_08Tukuyomi = "���Ȃ��́A���������Ă��܂����悤�ł���";
    public const string Tutorial000_09Reko = "�Ӗ����킩��Ȃ��I\n���͍��c";
    public const string Tutorial000_10Tukuyomi = "���v�@���������Ă�������";
    public const string Tutorial000_11Tukuyomi = "���̐��E�ɂ���ƂȂ��������Ȃ��ł�\n���킸�ɍς�ł���̂͂������U�l�����c";
    public const string Tutorial000_12Reko = "�ǂ����Ă���Ȃ��ƂɁH";
    public const string Tutorial000_13Tukuyomi = "�킩��܂���c���������N��\n���̂��߂ɂ���Ȑ��E��������̂�";
    public const string Tutorial000_14Tukuyomi = "�ЂƂ܂��������̐��E�����ē����܂���";
    public const string Tutorial000_15Reko = "�����I���Ȃ��͉��҂Ȃ�ł����H";
    public const string Tutorial000_16Tukuyomi = "�\���x��܂����A���̓t���[�f�ރL�����N�^�[�̂���݂����\n���͂����ŁA�������񂾕��̃t�H���[�����Ă��܂�";
    public const string Tutorial000_17Tukuyomi = "���������Ƃ���������Ȃ�ł������Ă���������";
    public const string Tutorial000_18Reko = "�t���[�\�U�C�L�����N�^�[���ĂȂ񂾁c�H";

    public const string Tutorial001_01Tukuyomi = "����͕ǂł�";
    public const string Tutorial001_02Reko = "����΂킩��܂�";
    public const string Tutorial001_03Tukuyomi = "�ǂ͒ʂ�܂���";
    public const string Tutorial001_04Reko = "������c";
    public const string Tutorial001_05Tukuyomi = "����̓L�[�{�[�h���Q�[���p�b�h�ł����H\n�ǂꂩ�̃{�^���������Ή������ꏊ�𒲂ׂ邱�Ƃ��ł��܂���";
    public const string Tutorial001_06Reko = "����͋����Ă���Ȃ���";

    public const string Tutorial002_01Tukuyomi = "������͂��������тɐ؂�ւ�����^�C�v�ł���\n�������ЂƂ̃X�C�b�`�ł������̕ǂ������܂�";
    public const string Tutorial002_02Reko = "�Ȃ�ł���Ȗʓ|�Ȃ��̍������ł���";
    public const string Tutorial002_03Tukuyomi = "�ق�A�K�i�̖�������ď㉺�ǂ���ł��I���I�t�ł��邶��Ȃ��ł���";
    public const string Tutorial002_04Reko = "��Ί֌W�����Ǝv��";

    public const string Tutorial003_01Tukuyomi = "���߂łƂ��������܂��I\n���Ȃ��͂������̐��E�̕��������[�������ł��܂����I";
    public const string Tutorial003_02Reko = "�Ǔ������������ł�����";
    public const string Tutorial003_03Tukuyomi = "����ς肾�߂ł��傤��";
    public const string Tutorial003_04Reko = "�s���Ȃ���΂����Ȃ��ꏊ���������C�������ł�\n�ǂ�����΂�����ł����H";
    public const string Tutorial003_05Tukuyomi = "���ꂪ�c�킩��Ȃ���ł�\n�����͂ƂĂ����������E�ł����A�o���͂ǂ��ɂ�����܂���";
    public const string Tutorial003_06Reko = "����ȁI�o���Ȃ���ł����H";
    public const string Tutorial003_07Tukuyomi = "�P�ӏ������T���Ă��Ȃ��ꏊ�c�Ƃ���������Ȃ��ꏊ������܂�\n�ꉞ���ɍs���Ă݂܂����H";
    public const string Tutorial003_08Reko = "�������I";
    public const string Tutorial003_09Tukuyomi = "�������܂�܂���\n�ǂ����������";

    public const string Tutorial004_01Tukuyomi = "���̏�Ɖ��ŁA�݂Ȃ���v���v���ɂ�������Ă��܂�";
    public const string Tutorial004_02Tukuyomi = "�܂��������ɍs���ƃJ�t�F������̂ŁA��{�I�ɂ͂����炪�����̃x�[�X�ɂȂ�܂���";
    public const string Tutorial004_03Reko = "���̌��́H";
    public const string Tutorial004_04Tukuyomi = "�����G��ƃZ�[�u�ł��܂����A���͎g���܂���";
    public const string Tutorial004_05Tukuyomi = "�`���[�g���A���̓r���ŃZ�[�u�����Ƃ��낢��ʓ|�ł�����";
    public const string Tutorial004_06Reko = "�悭�킩��Ȃ�";
    public const string Tutorial004_07Tukuyomi = "��̏ꏊ�͉E�̉��ł�";

    public const string Tutorial005_01Tukuyomi = "���̏�͊C�ɂȂ��Ă��āA�댯�Ȃ̂Ői�߂܂���";
    public const string Tutorial005_02Tukuyomi = "���͓���Ȃ��悤�ɍ������Ă܂�����A�����܂łȂ�s���Ă����܂��܂��񂯂ǂ�";
    public const string Tutorial005_03Reko = "�d�q�̊C�̏��c\n�����@����";

    public const string Tutorial008_01Tukuyomi = "�����̒ʂ�A�s�v�c�ȗ͂Œʂꂸ����ȏ㉜�ɓ���Ȃ��̂ł�";
    public const string Tutorial008_02Reko = "����́H";
    public const string Tutorial008_03Tukuyomi = "���̕��͂����ɗ��Ă��炸���Ɩ�����ʂ낤�Ɗ撣���Ă����ł����A���߂݂����ł���";
    public const string Tutorial008_04Reko = "���A�����ł���";
    public const string Tutorial008_05Tukuyomi = "�\�ł́A�V�̐������҂��������̐�ɐi�߂�炵����ł�";
    public const string Tutorial008_06Reko = "�ǂ����Ă���ȉ\���H";
    public const string Tutorial008_07Tukuyomi = "�����̗��ĎD�ɏ����Ă���܂�";
    public const string Tutorial008_08Reko = "�����A�����c";
    public const string Tutorial008_09Reko = "�V�̐��c�U�l�͐����c���Ă���Č����Ă܂�����ˁH���ƂP�l����΂�����������c";
    public const string Tutorial008_10Tukuyomi = "��ɍs���܂����H�݂Ȃ�����I�ȕ��ł����ɂ͎��݂��Ă���Ȃ���������܂��񂪁A���͂��܂���I";
    public const string Tutorial008_11Reko = "�������܂��傤�A��낵�����肢���܂�";
    public const string Tutorial008_12Tukuyomi = "�K�v�Ƃ���Ύ��̑̂����g�����������I";

    #endregion

    #region �t�B�[���h000�`010

    public const string F003_GetKey = "�����Ȍ�����ɓ��ꂽ�I";

    public const string F006_1_ErapsNew = "����ɂ��́A�V�������ł���";
    public const string F006_2_Eraps = "���̐�̓k�V�����ނƂ����Ă���C�ŁA�����֎~�ɂȂ��Ă��܂�";
    public const string F006_3_Eraps = "����ȏ����́A���j�̂悤�ȋ��̂悤�Ȏp�����Ă���񂾂Ƃ�";
    public const string F006_4_Reko = "���j�Ƌ�����S�R�Ⴄ��";
    public const string F006_5_Tukuyomi = "�`���Ƃ͂����������̂Ȃ̂ł�";
    public const string F006_6_Reko = "�ǂ��������̂��낤";

    public const string F008_Board = "���̐������҂�\n�@���͊J�����";
    public const string F009_Board1 = "\n���呛���͏Z���ɔz�����ė��ꏬ����";
    public const string F009_Board2 = "\n������\n�@�Â��ȏꏊ�Ő��_����";
    public const string F009_Board3 = "\n�N���E���T�[�J�X��";
    public const string F010_Board1 = "\n��MANA����̂������I";
    public const string F010_Board2 = "\n���J�t�F ���t���a";
    public const string F010_Board3 = "\n�\���ԋ���\nCOEIRO�x����";

    #endregion

    #region �t�B�[���hXX1�`�@�����M�~�b�N�n

    public const string F111_New1_Exa = "�您�A�V���肩";
    public const string F111_New2_Tukuyomi = "��قǖ�������ł������ł�\n������ɓn�肽���̂ł���";
    public const string F111_New3_Exa = "�����ȁA�܂��C�����I����ĂȂ��񂾂�";
    public const string F111_New4_Exa = "���O����ł񂾂��珟��ɍs���΂�������˂���";
    public const string F111_New5_Tukuyomi = "���������킯�ɂ͂܂���܂������";
    public const string F111_New6_Exa = "�����c";
    public const string F111_New7_Exa = "�������爫�����ǂ����Ń��[�v�����Ă��Ă����\n������Ƒ���Ȃ��Ȃ����܂��Ă�";
    public const string F111_New8_Exa = "�ǂ������Ȃ��Ɛi�߂Ȃ����I�����Ƃ��o���Ȃ��Ă������\n���񂾂��I";
    public const string F111_New9_Reko = "�͂��I���̊Ԃɂ������󂯂����ƂɂȂ��Ă�";
    public const string F111_New10_Tukuyomi = "�d������܂���A���[�v�T���Ă��܂��傤";
    public const string F111_1_Exa = "���[�v�͂��������H";
    public const string F111_2_Tukuyomi = "���������܂����I";
    public const string F111_3_Exa = "�悵�҂��ĂȁA���������Ă��";
    public const string F111_4_Exa = "�����I�������\n�����������";
    public const string F111_5_Exa = "�ǂ������A�s���Ƃ��낪����񂾂�H";

    public const string F121_1_Tukuyomi = "�����������Ă��܂���";
    public const string F121_2_Reko = "�N�����Ȃ��̂ł́H";
    public const string F121_3_Tukuyomi = "���T���ē������Ⴂ�܂��傤�I";
    public const string F121_4_Reko = "�����[";
    public const string F121_Board1 = "�N���E���T�[�J�X�|�l��W���I\n�N�ł��C�y�ɂǂ����I";
    public const string F121_Board2 = "�䂱���͂Ƃ������͂��ł����ڎx�z�l�܂ŁI";
    public const string F121_Board3 = "�����̌��͕�������ԏ��Ȃ����ĎD���牺�ɂS���A���ɂS�����炢�̂Ƃ���ɉB���Ă��邼�I";
    public const string F121_Board4 = "�P���̃T�C�Y�͂Ȃ�ƂȂ��@���Ă����ȁI";

    public const string F141_Board1 = "���Ԃ͂���̔����݂��悤";
    public const string F141_Board2 = "���Ă��鎞�͓��������ڂ����ǁA�J���ƂЂƂ����Ⴄ��I";
    public const string F141_Board3 = "�ǂꂩ�̔��ɐG�ꂽ��A����Ƒ��̃����_���ȂЂƂ��c���Ă��ׂĂ̔����J����";
    public const string F141_Board4 = "���̌�ł��炽�߂čD���Ȕ���i��ł�";
    public const string F141_1_Koob = "��킵�Ă�݂������ˁA�q���g���K�v���ȁH";
    public const string F141_2_Koob = "�Q��ڂ̑I���́A�J���Ă������I��ł�������";
    public const string F141_3_Koob = "�J���ƂЂƂ����Ⴄ���ǁA�J����������͂���ς蓯�������ڂɂȂ����Ⴄ��";
    public const string F141_4_Koob = "�J���u�Ԃ��悭����ƒ��Ԃ͂���͌����邩����";

    public const string F151_1_Reko = "���̔�n���Ζʓ|�ȉ�蓹���Ȃ��Ă�����";
    public const string F151_2_Tukuyomi = "���[�v�������Ɏg���邩������Ȃ��̂Ŏ����Ă����܂��傤";



    #endregion

    #region �t�B�[���h143�@�����f��

    public const string F143_Fast1_Koob = "��A������\n���������̂�";
    public const string F143_Fast2_Koob = "�撣���ĂˁA�������������Ƃ��������玄�͏��t���a�ɂ��邩��";

    public const string F143_New1_Menderu = "�悤������������Ⴂ�܂����A������q�r��";
    public const string F143_New2_Menderu = "�c�Ȃ񂾁A����݂����";
    public const string F143_New3_Tukuyomi = "�Ȃ񂾂Ƃ͂Ȃ�";
    public const string F143_New4_Menderu = "�������̐l�́H";
    public const string F143_New5_Reko = "���́A���̐��E�ɖ�������ł��܂����炵����";
    public const string F143_New6_Menderu = "�����A��ς��������";
    public const string F143_New7_Menderu = "���͉������A�݂�ȃ����f�����ČĂ�ł��";
    public const string F143_New8_Tukuyomi = "�o����T�������Ƃ������ƂŁA�����f������̂��͂��؂�ɂ��܂����I";
    public const string F143_New9_Menderu = "���̂���ˁA������z������Ȃ�Ė{�C�Ŏv���Ă�́H";
    public const string F143_New10_Menderu = "�]�v�Ȃ��Ƃ��Ȃ��ł����̕�炵�ɏ��������ق����c�y�ł������";
    public const string F143_New11_Tukuyomi = "�����ǃ����f����������̐��E�ɐS�c��͂����ł��傤�H���̌��������āc";
    public const string F143_New12_Menderu = "���ʂ�c�����Ȃ񂩉��̖��ɂ������Ȃ������B�������Ȃ������Đ��E�͉����ς��Ȃ�";
    public const string F143_New13_Reko = "����Ȃ��Ƃ͂���܂����A�������Ă��Ȃ������Ȃ��Ƃ��߂Ȃ�ł�";
    public const string F143_New14_Reko = "���Ȃ��̐��E�ɂ����āA���Ȃ�������K�v�Ƃ��Ă���l������͂��ł��傤�I";
    public const string F143_New15_Menderu = "���z��C���������ł͉����ς����Ȃ���B����[���������邾���̗͂��������Ƃ��ł���H";
    public const string F143_New16_Tukuyomi = "�]�ނƂ���ł��I�󂯂ė����܂���";
    public const string F143_New17_Reko = "�����A�����";

    public const string F143_Lose1_Menderu = "���߂ˁA����Ȃ񂶂Ⴀ���̕ǂ͉z�����Ȃ���";
    public const string F143_Lose2_Reko = "�i����֌W�����ł���ˁc�H�j";
    public const string F143_Lose3_Tukuyomi = "�i�ˁA���I�ł��傤�j";

    public const string F143_Retry_Menderu = "���āA��������������H";

    public const string F143_Win1_Menderu = "�ӂ��A�킩������A���͂��܂��傤";
    public const string F143_Win2_Tukuyomi = "���܂����ˁI����O�i�ł��I";
    public const string F143_Win3_Menderu = "�ł����̕ǂ̐�̎��͉����킩���ĂȂ��񂾂���A�ߓx�Ȋ��҂͂��Ȃ��ł������Ƃ�";
    public const string F143_Win4_Reko = "���̎��͂��̎��ł���";

    #endregion

    #region �t�B�[���h200�`�@���X�g�_���W�����n
    #endregion
}

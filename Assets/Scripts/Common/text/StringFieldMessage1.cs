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
    public const string Tutorial008_12Tukuyomi = "���̎p�ł͕s�ւȂ��Ƃ�����ł��傤����\n�K�v�Ȏ��͎��̑̂����݂����܂���";

    #endregion

    #region �t�B�[���h000�`010

    public const string F003_GetKey = "�����Ȍ�����ɓ��ꂽ�I";

    public const string F004_Clear5_1_Tukuyomi = "�c���Ƃ��날�ƂP�l�ł�";
    public const string F004_Clear5_2_Reko = "����A�U�l����Ȃ���ł����H";
    public const string F004_Clear5_3_Tukuyomi = "�����ł���\n���T�l�Ƃ�����܂�������";
    public const string F004_Clear5_4_Reko = "����";
    public const string F004_Clear5_5_Tukuyomi = "����";
    public const string F004_Clear5_6_Reko = "����݂����\n���������Ď����𐔂��ĂȂ��H";
    public const string F004_Clear5_7_Tukuyomi = "������񂻂��ł�";
    public const string F004_Clear5_8_Reko = "�K�b�N�V�c";
    public const string F004_Clear5_9_Reko = "���₢���񂾁A�Ȃ�S���ł������ɍs����\n�����Ɖ����ł���͂���";
    public const string F004_Clear5_10_Reko = "�Ō�̂P�l�ɉ�ɍs�����I";

    public const string F004_Clear6_1_Reko = "���ɑS�����������I�����c\n�Ȃ񂾂���";
    public const string F004_Clear6_2_Tukuyomi = "�܂������E�ɍs�����Ƃ���̃o���A�ł���";
    public const string F004_Clear6_3_Reko = "��������";

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

    public const string F101_New1_Reko = "�ʂ�܂���";
    public const string F101_New2_Tukuyomi = "���[��A�N���ɗ���Ŕ����Ă����Ȃ��ƃ_���ł���";
    public const string F101_Check1_Reko = "�����s���n������Ȃ��Ɣ��ꂻ���ɂȂ��c";
    public const string F101_Slash1_Reko = "���肢���܂�";
    public const string F101_Slash2_You = "��";
    public const string F101_Slash3_Reko = "�Ђ��[������";
    public const string F101_Slash4_You = "���ꂶ��";
    public const string F101_Slash5_Reko = "���肪�Ƃ��������܂����I";

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

    public const string F131_BackNew1_Reko = "����A���Ƃɖ߂��Ă�c";
    public const string F131_BackNew2_Tukuyomi = "�t���O�Ǘ��̃~�X�ł��傤���H";
    public const string F131_BackNew3_Reko = "���Ⴀ����ȃZ���t�o�Ȃ��ł���";
    public const string F131_BackNew4_Tukuyomi = "�Ƃɂ����i�ނȂ������x������������܂����";
    public const string F131_BackNew5_Reko = "�ʓ|���Ȃ�";
    public const string F131_Back1_Reko = "�ǂ����Ă��������痈��Ɩ�����Ƃɖ߂�񂾂낤�c";
    public const string F131_Catch1_Reko = "�����Ȃ�ł���";
    public const string F131_Catch2_Worra = "���ꂶ�Ⴀ�����̂悤�Ɉ�x�����Ă���A�߂��Ă݂Ă���邩����H";
    public const string F131_Catch3_Worra = "����ς�I�܂�����Ȃ������炵��";
    public const string F131_Catch4_You = "�Ȃ񂩁c�̂̃Q�[�����ۂ����Ȃ���";
    public const string F131_Catch5_Worra = "����񂱂Ƃ��Ȃ��Ă�������A���t���a�ɋA����";
    public const string F131_Catch6_You = "�́[��";

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

    #region �t�B�[���h102�@�}�`

    public const string F102_New1_Mati = "���ɂ����܂ŗ�����\n�c�҂��Ă�����";
    public const string F102_New2_Reko = "����";
    public const string F102_New3_Mati = "���͌R�̃}�`\n���߂܂��Ă���\n���͌N���悭�m���Ă���";
    public const string F102_New4_Tukuyomi = "�ޏ��͖������痈��\n�A���h���C�h�Ȃ񂾂����ł�";
    public const string F102_New5_Reko = "�����ڂ͑吳���}�����ۂ��ł���";
    public const string F102_New6_Tukuyomi = "���s�͌J��Ԃ����̂ł�����";
    public const string F102_New7_Mati = "�ڂ����͘b���Ȃ���\n�N�̗͂ɂȂ邽�߂ɗ���";
    public const string F102_New8_Reko = "���ꂶ�Ⴀ���̐��E�����Ȃ̂�\n�m���Ă��ł����H";
    public const string F102_New9_Mati = "���Ƃ��b���x�ɂˁc\n�����������͌N���悳";
    public const string F102_New10_Mati = "���̕ǂ̐�֍s�����肾�낤�H\n�����Ȋo��Ő������铹�ł͂Ȃ���";
    public const string F102_New11_Reko = "����ł��A���Ȃ��Ⴂ���Ȃ����Ƃ�\n���Ȃ��Ⴂ���Ȃ���ł��I";
    public const string F102_New12_Mati = "�����ڂ����Ă����\n�Ȃ�Ίm���߂����Ă��炨���A�N�̋���";
    public const string F102_New13_Tukuyomi = "�ڂ����ł����H";

    public const string F102_Lose1_Mati = "����ł͐�֐i�܂���킯�ɂ͂����Ȃ���\n�[���ɗ͂����Ă܂����Ă���";
    public const string F102_Lose2_Reko = "�i������Ƌ������܂���j";
    public const string F102_Lose3_Tukuyomi = "�i���x������Ŕ�ꂳ����Ƃ����̂͂ǂ��ł��傤�H�j";

    public const string F102_Retry1_Mati = "�����A�n�߂悤��";

    public const string F102_Win1_Mati = "�N�̊o�債���ƌ����Ă������\n���̑S�͂Ŏx�����悤";
    public const string F102_Win2_Mati = "�Ƃ���ł܂������ł��Ă��Ȃ��l���c���Ă���悤����";
    public const string F102_Win3_Mati = "���͐�Ƀo���A�̑O�ő҂��Ă����";
    public const string F102_Win4_Tukuyomi = "�Ȃ�Ƃ��Ȃ�܂�����";
    public const string F102_Win5_Reko = "�n�@�A�苭�����肾�����c\n�o��Q�[���Ԉ���Ă�񂶂�Ȃ��ł���";

    #endregion

    #region �t�B�[���h112�@�܂��肷��

    public const string F112_New1_Matuka = "�����ɖl�ȊO������Ȃ�Ē�������";
    public const string F112_New2_Reko = "�������Ă��̂ɑ��v�Ȃ�ł����H";
    public const string F112_New3_Matuka = "�����܂������Ă��̂��A�ʓ|��������";
    public const string F112_New4_Reko = "�C�Â��Ă���Ȃ�����";
    public const string F112_New5_Tukuyomi = "�Ƃ��������܂肷������̐��ŗ����Ă��ł���";
    public const string F112_New6_Reko = "����Ȃ��Ƃ���܂���";
    public const string F112_New7_Matuka = "�킴�Ƃ���Ȃ��񂾂�";
    public const string F112_New8_Reko = "������O�ł�";
    public const string F112_New9_Tukuyomi = "����ȏ��܂����������ŁA�o���A���󂷂̂���`���Ă�������������ł�";
    public const string F112_New10_Matuka = "����ȑO��������ǃ_���������񂾂�Ȃ��A�R�����łȂ��ᖳ������Ȃ���";
    public const string F112_New11_Reko = "�N�ł���";
    public const string F112_New12_Tukuyomi = "�����łł���A��l�ł͖����ł��݂Ȃ���ŋ��͂���΂ǂ�����";
    public const string F112_New13_Matuka = "���̃����o�[��S���H�����܂�邼�[";
    public const string F112_New14_Reko = "�ł������̂��Ƃ͂��Ă���������ł�";
    public const string F112_New15_Matuka = "�ӂށA�N�̂��C���悩�ȁA���̋��т�l�ɕ������Ă���";
    public const string F112_New16_Tukuyomi = "���܂������������I�������̊�ɂȂ�܂��傤";

    public const string F112_Lose1_Matuka = "�C��������Ȃ��񂶂�Ȃ����ȁI";
    public const string F112_Lose2_Reko = "�����������Ă����";
    public const string F112_Lose3_Tukuyomi = "���݂܂���A���x�����Ύ�������Đ����o���₷���Ȃ邩������܂���";

    public const string F112_Retry1_Matuka = "���߂āA�������Ă��炨��";

    public const string F112_Win1_Matuka = "�ǂ��C���������A�N�Ȃ���邩������Ȃ���";
    public const string F112_Win2_Reko = "���ꂶ�Ⴀ��낵�����肢���܂��I";

    #endregion

    #region �t�B�[���h122�@�s�G�[��

    public const string F122_New1_Pierre = "�n�b�s�[�E�G�[�[�[���I";
    public const string F122_New2_Pierre = "�N���E���T�[�J�X�ւ悤�����I\n���}�����l�̓s�G�[���E�N���E���I";
    public const string F122_New3_Reko = "�܂������Ԃ�₩�܂����l���o�Ă�����";
    public const string F122_New4_Pierre = "�N�͓��c��]�҂��ˁH\n�������c�e�X�g���n�߂悤�I";
    public const string F122_New5_Reko = "���⎄��";
    public const string F122_New6_Pierre = "�Ȃ��Ƀe�X�g�͊ȒP���I\n���������Ŗl�ɒǂ��������I";
    public const string F122_New7_Reko = "���߂��S�R�b�����Ă܂����";
    public const string F122_New8_Tukuyomi = "�撣��܂��傤�ˁI";
    public const string F122_New9_Reko = "���� ��������";

    public const string F122_Lose1_Pierre = "�T�[�J�X�c���ɕK�v�Ȃ̂͂P�ɂ��Q�ɂ��̗́I\n�܂����킵�Ă��ꂽ�܂��I";
    public const string F122_Lose2_Reko = "�Ȃ񂩔[�������Ȃ��c";

    public const string F122_Retry1_Pierre = "OK�I�ăe�X�g���I";

    public const string F122_Win1_Pierre = "�f���炵����ނ��I\n�N�Ȃ炷���ɃX�^�[�ɂȂ�邼�I";
    public const string F122_Win2_Reko = "���⎄��";
    public const string F122_Win3_Pierre = "�����P�͋}�����P���n�߂悤�I\n�Ƃ���Ń��C�I���͍D�������H";
    public const string F122_Win4_Reko = "���C�I���̂ق����܂��b���ʂ�������";
    public const string F122_Win5_Tukuyomi = "�܂����q������J�񂷂邽�߂�\n���̃o���A���󂵂ɂ����܂��񂩁H";
    public const string F122_Win6_Pierre = "�i�C�X�A�C�f�A�I\n�����ƌ��܂�΂����s���������s�����I";
    public const string F122_Win7_Reko = "�Ȃ�ł������̐l";
    public const string F122_Win8_Tukuyomi = "����ňĊO�b���΂킩����ł���";

    #endregion

    #region �t�B�[���h132�@MANA

    public const string F132_New1_Mana = "����ف[�HMANA���񂾂�";
    public const string F132_New2_Reko = "�͂��߂܂���\n���͐܂�����Ă��肢���c";
    public const string F132_New3_Mana = "������[�I";
    public const string F132_New4_Reko = "�I�H";
    public const string F132_New5_Tukuyomi = "�b�̂킩����Ȃ�ł�";
    public const string F132_New6_Reko = "�Ȃ���킩���ĂȂ������̂悤��";
    public const string F132_New7_Mana = "����������MANA������������Ă��炾�I";
    public const string F132_New8_Reko = "�I�H";
    public const string F132_New9_Tukuyomi = "�V�т��������ł���";
    public const string F132_New10_Reko = "�킩�����킩����";

    public const string F132_Lose1_Mana = "�ӂ͂͂͂́I�݂��キ���̂�";
    public const string F132_Lose2_Reko = "�Ȃ񂩉������c";

    public const string F132_Retry1_Mana = "�Ȃ�ǂł������ĂɂȂ邼�I\n�������Ă����܂�";

    public const string F132_Win1_Mana = "�܂���������I��邶��Ȃ�";
    public const string F132_Win2_Mana = "�ŁA�Ȃ񂾂���";
    public const string F132_Win3_Tukuyomi = "�E�ɂ��邠�̃o���A���󂵂����̂�\n��`���Ă�������������ł�";
    public const string F132_Win4_Mana = "�������[�IMANA����ɂ��܂���";
    public const string F132_Win5_Reko = "�y���ȁ[";

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

    #region �t�B�[���h153�@���t���A�~

    public const string F153_Exa_Def1_Exa = "�您�A�ǂ��������q��";
    public const string F153_Exa_Plant1_Exa = "�����̓����̐A���H";
    public const string F153_Exa_Plant2_Exa = "���܂񂪂������̕����Ⴀ��͖�������\n�Ђł�}�V���ł���������o������z�ł��T���Ƃ�����";

    public const string F153_Worra_Def1_Worra = "����A����ɂ���\n����΂��Ă���";
    public const string F153_Worra_Ice1_Tukuyomi = "���݂܂���A������������������";
    public const string F153_Worra_Ice2_Worra = "�����H";
    public const string F153_Worra_Ice3_Reko = "�킩��܂���悻�ꂶ�Ⴀ";
    public const string F153_Worra_Ice4_Reko = "�X�̃u���b�N�̏��łȂɂ�\n�������Ȃ��Ƃ��N���Ă܂��āc";
    public const string F153_Worra_Ice5_Worra = "�Ȃ�قǁA�����";
    public const string F153_Worra_Ice6_Worra = "�킩������\n���ׂɍs���܂��傤";

    public const string F153_Koob_Def1_Koob = "�����g���ƊÂ����̂��~�����Ȃ���ăz���g�Ȃ̂���";
    public const string F153_Koob_HelpQ_Koob = "���������Ă�l�q���ȁH";
    public const string F153_Koob_Key1_Koob = "�T�[�J�X�̌���������Ȃ��̂�\n����́c";
    public const string F153_Koob_Key2_Koob = "�������P�ԏ��Ȃ����ĎD���ǂꂩ���Ď�����\n���Ɍ������Ƃ���͂�����";
    public const string F153_Koob_You1_Koob = "�X�̃u���b�N������ɖ߂�H\n����́c";
    public const string F153_Koob_You2_Koob = "�܂��c��l�̐l��A��Ă�����\n���Ă��炤�Ƃ����񂶂�Ȃ�����";
    public const string F153_Koob_Plant1_Koob = "�A�����ז��ő����ɍs���Ȃ��H\n����́c";
    public const string F153_Koob_Plant2_Koob = "���ʂȐn���ƋZ�p���K�v������A�ł���l��T���Ȃ��Ƃ�";
    public const string F153_Koob_PlantEx3_Koob = "�ǂ����ɉB��Ă邩���A�ςȂ��Ƃ�������ꏊ�����������������";

    public const string F153_You_Def1_You = "�c�c";
    public const string F153_You_Plant1_You = "�ǂ������́H";
    public const string F153_You_Plant2_Reko = "�����ɍs�����̐A���𔰍̂ł���l��T���Ă��ł���";
    public const string F153_You_Plant3_You = "��ς���";
    public const string F153_You_Plant4_Worra = "���̎q�ł�����A�A��Ă����Ƃ�����";
    public const string F153_You_Plant5_Reko = "�{���ł����I���肢���܂�";
    public const string F153_You_Plant6_You = "���`";
    public const string F153_You_Plant7_Worra = "�����Ă����Ȃ�����";
    public const string F153_You_Plant8_You = "�́`��";

    public const string F153_New1_Ami = "��������Ⴂ�܂��I���D���ȐȂւǂ���";
    public const string F153_New2_Tukuyomi = "������͏��t���a�̃}�X�^�[\n���t���A�~����ł�";
    public const string F153_New3_Tukuyomi = "�A�~����A�����͂�����ƕʌ��ł���";
    public const string F153_New4_Ami = "�ǂ������́H";
    public const string F153_New5_Reko = "�o���A���󂷂̂���`���Ă���܂��񂩁H";
    public const string F153_New6_Ami = "�����A���Ȃ񂩂ɂ���ȑ傻�ꂽ���Ɓc";
    public const string F153_New7_Reko = "�Ƃ�ł��Ȃ��I���Ȃ��̗͂��K�v�Ȃ�ł�";
    public const string F153_New8_Ami = "��[�A���Ⴀ�ꉞ���Ȃ��̔\�͂������Ă��炨������";
    public const string F153_New9_Tukuyomi = "�ف[�痈��";
    public const string F153_New10_Reko = "����Ă��܂���I";

    public const string F153_Lose1_Ami = "���ꂶ�Ⴀ������ƕs������";
    public const string F153_Lose2_Reko = "�������A����݂����A�ǂ��ɂ��Ȃ�Ȃ�����";
    public const string F153_Lose3_Tukuyomi = "�A�~����͂����������Ɋւ��Ă͑Ë����܂��񂩂�A���͂łǂ��ɂ����邵������܂���";
    public const string F153_Lose4_Reko = "�Ȃ�Ă������c";

    public const string F153_Retry1_Ami = "������x���H";

    public const string F153_Win1_Ami = "�Ȃ��Ȃ����ˁA����A����Ă݂��";
    public const string F153_Win2_Reko = "������I";

    #endregion

    #region �t�B�[���h200�`�@���X�g�_���W�����n

    public const string F008_Normal_Ami = "����ς肿����ƕ|�����ǁc\n�����܂ŗ�������܂��傤";
    public const string F008_Normal_Mana = "MANA����̓`������������n�܂�I";
    public const string F008_Normal_Matuka = "�������������������I\n�݂Ȃ����Ă��������������I";
    public const string F008_Normal_Mati = "�������ł�����o���A��@����";
    public const string F008_Normal_Pierre = "�܂������H\n�҂������тꂿ�܂�����I";
    public const string F008_Normal_Menderu = "���͂��ł��������";
    public const string F008_Normal_Drows = "���ʂʁc";
    public const string F008_Normal_Exa = "���`�����₪����";
    public const string F008_Normal_Worra = "���v�A���������I����\n�����ƑS�����܂�����";
    public const string F008_Normal_Koob = "���̎q�͐S�z����Ȃ���\n�ǂ����������C�ɂȂ邵";
    public const string F008_Normal_You = "�A����N�����Ȃ��ł���\n���ʂقǔ��Ă�";

    public const string F008_Break1_Reko = "�V�l�����܂�����";
    public const string F008_Break2_Tukuyomi = "���ꂩ��ǂ����܂��傤��";
    public const string F008_Break3_Reko = "�l���ĂȂ������c";
    public const string F008_Break4_Matuka = "�I�I�I�C�I";
    public const string F008_Break5_Mati = "�h���̐������ҁh�c";
    public const string F008_Break6_Pierre = "�܂�ňӖ����킩���";
    public const string F008_Break7_Menderu = "�����b�������V��ޕ���������ł���";
    public const string F008_Break8_Mana = "�o���A���񂪁H";
    public const string F008_Break9_Ami = "�C�����͂킩���";
    public const string F008_Break10_Matuka = "�킩�����������";
    public const string F008_Break11_Tukuyomi = "���Ⴀ�Ƃ肠�����S�������ɋ���ł݂܂���";
    public const string F008_Break12_Reko = "�����I���ꂪ������������";
    public const string F008_Break13_Menderu = "�͂��͂�";
    public const string F008_Break14_Tukuyomi = "�ł݂͂Ȃ��񂢂��܂���A���[��";

    public const string F008_Break15_Reko = "�Ȃ�ŁI�H";
    public const string F008_Break16_Tukuyomi = "���߂Ă܂���ł��������";
    public const string F008_Break17_Mana = "�ł������Ă����Ă�݂����I";
    public const string F008_Break18_Matuka = "�����̂��I�H";

    public const string F008_Break19_Mati = "�s������\n���̐��E�̐^�����킩��͂���";
    public const string F008_Break20_Pierre = "�얳�O�I";

    public const string F201_Break1_1_Reko = "����A�ǂ����܂������H";
    public const string F201_Break1_2_Ami = "�s���~�܂�݂�������";
    public const string F201_Break1_3_Pierre = "���[�񂾂�\n�o�@���������ꂽ";

    public const string F201_Break2_1_Drows = "����������ƒʂꂽ��";
    public const string F201_Break2_2_Mana = "�����r���r�����Ă��l\n�Ȃ񂩑S�R���C����";
    public const string F201_Break2_3_Drows = "�����Q����S�񕜂��邾��";
    public const string F201_Break2_4_Matuka = "�Q�[������I";
    public const string F201_Break2_5_Tukuyomi = "�Q�[���ł���";
    public const string F201_Break2_6_Menderu = "�ł��c�O�����ǂ����ɂ͉����Ȃ���";

    public const string F201_Break2_7_Drows = "�Ȃɂ��͂��邾��A�����ɂ�";
    public const string F201_Break2_8_Matuka = "�����̕ǂ݂���������\n�������ĉz����ꂻ�����Ȃ���";
    public const string F201_Break2_9_Drows = "����Ȃ���͂�������Ⴂ����";

    public const string F201_Break3_1_Mati = "���B���]�����������";
    public const string F201_Break3_2_Drows = "���������������������������I�I�I";
    public const string F201_Break3_3_Mana = "�Ȃ񂾂Ȃ�";

    public const string F201_Break4_1_Worra = "���炠��";
    public const string F201_Break4_2_Koob = "�܁[���n�܂���";
    public const string F201_Break4_3_Exa = "�܁A�D���ɂ�点�Ƃ��Ⴂ��";
    public const string F201_Break4_4_You = "�ł����낻��c";

    public const string F201_Break5_1_Drows = "���������������������������I�I�H";

    public const string F201_Break6_1_Eraps = "���v�ł���\n������̘b�͏I���܂�����";
    public const string F201_Break6_2_Reko = "���́[�H";
    public const string F201_Break6_3_Koob = "�����������͋C�ɂ��Ȃ��Ă�����\n���ɂł��Ă邩��";
    public const string F201_Break6_4_Worra = "���Ȃ������̑厖�Ȃ��Ƃ�D�悵�Ă�";
    public const string F201_Break6_5_Tukuyomi = "����ł�\n�����t�ɊÂ������Ă��������܂��傤";

    public const string F201_Break7_1_Drows = "���ĂĂ�";
    public const string F201_Break7_2_Koob = "�A�낤���A���B�̖�ڂ͂����܂ł���";
    public const string F201_Break7_3_Exa = "�����A���Ƃ͂����炪���������邳";

    public const string F202_Start_1_Reko = "����́c�H";
    public const string F202_Start_2_Tukuyomi = "�������̕����v�����ܖ\�ꂽ�悤�ł���";
    public const string F202_Matuka_1 = "����͂Ђǂ�";
    public const string F202_Pierre_1 = "�������ߊ��I�̓����ĂƂ����ȁI";
    public const string F202_Menderu_1 = "���낢��d�|�����������悤������\n�S���͂����ł߂���߂���ɂ���������̂�";

    public const string F203_Ami_1 = "���̐�͉��ĂȂ��݂���";
    public const string F203_Mana_1 = "���̐l�͂�����ւ�ň����Ԃ����̂��ȁH";

    public const string F203_TreasureA_1_Tukuyomi = "������ƁA�����Ă��ł���";
    public const string F203_TreasureA_2_Reko = "���A�󔠂����邩��";
    public const string F203_TreasureA_3_Tukuyomi = "����͊댯�Ȃ̂ŊJ������_���ł���\n���̒n�`������Ίm��I�ɖ��炩�ł�";
    public const string F203_TreasureA_4_Reko = "�c�c�H";

    public const string F203_TreasureB_1_Reko = "�u�����ʐ_�Ɂ@������Ȃ��v\n���ꂾ���";

    public const string F204_Open_1_Mati = "����́A�o���A�Ǝ����d�g�݂̂悤����";
    public const string F204_Open_2_Reko = "����ɂV�l�ŐG���΂�����ł��傤��";
    public const string F204_Open_3_Mana = "���܂����I\n���������ǂ����ǂ���";
    public const string F204_Open_4_Ami = "����";
    public const string F204_Open_5_Menderu = "�c�ǂ���玄�B�͂��̐�ɂ͍s���Ȃ��炵��";
    public const string F204_Open_6_Reko = "�ł��F����o���Ȃ��񂶂Ⴀ�H";
    public const string F204_Open_7_Pierre = "�����ΊJ��������v���A�ق�";
    public const string F204_Open_8_Reko = "�Ȃ�";
    public const string F204_Open_9_Tukuyomi = "�d���Ȃ��̂Ő�ւ��i�݉�����\n���S�j�͂��������ł�";
    public const string F204_Open_10_Reko = "����Ȃ��Ƃ��킩���ł����H";
    public const string F204_Open_11_Tukuyomi = "�Z�[�u�|�C���g���u���Ă��邩��ł���";
    public const string F204_Open_12_Matuka = "�킩��";
    public const string F204_Open_13_Reko = "�킩�����������";

    public const string F006_Opened_1_Eraps = "����A�ǂ����Ă����ɁH";
    public const string F006_Opened_2_Reko = "���₠�Ȃ�ƂȂ�\n�������邩�Ȃ���";
    public const string F006_Opened_3_Eraps = "������ɂ͂����N���c���Ă��܂����";
    public const string F006_Opened_4_Eraps = "�݂Ȃ��񂨑҂��ł��傤����\n�������S�j��";
    public const string F006_Opened_5_Eraps = "�c�c";
    public const string F006_Opened_6_Eraps = "���Ȃ��ɂ͗����������Ԃ����܂�\n��]�������ɂȂ��Ă������Y��Ȃ���";

    #endregion

    #region �t�B�[���h205�@���X�{�X����

    public const string F205_S1_1_Boss = "�M�l�A���������Ȃ̂��m���������ŐN�������̂��H";
    public const string F205_S1_2_Reko = "���O�������̑n���傩�H\n��̉����ړI�Ȃ񂾁I";
    public const string F205_S1_3_Boss = "�����ɂ��������̐��E�̊Ǘ��҂ł���";
    public const string F205_S1_4_Boss = "�����Č̂ɑԓx�ɂ͋C������\n�����₢�������I�M�l�͓����鑤���b�I�I";
    public const string F205_S1_5_Reko = "�������A�������v���b�V���[���c�I";
    public const string F205_S1_6_Tukuyomi = "���v�ł����I�H";
    public const string F205_S1_7_Reko = "����݂����I�ǂ�����āH";
    public const string F205_S1_8_Tukuyomi = "���ʂɔ��ł��܂���";
    public const string F205_S1_9_Reko = "�����͂�������c";
    public const string F205_S1_10_Boss = "�C���M�����[�̎�����c��͂�M�l����u���ׂ��ł͂Ȃ�����";
    public const string F205_S1_11_Boss = "�������Ŏn�����Ă���悤�I";
    public const string F205_S1_12_Tukuyomi = "���܂��I";

    public const string F205_S2_1_Reko = "��������I�H";
    public const string F205_S2_2_Tukuyomi = "�Ȃɂ����܂��c";
    public const string F205_S2_3_Reko = "���ł����H";
    public const string F205_S2_4_Tukuyomi = "�R���s���[�^�̂悤�ł���";
    public const string F205_S2_5_Boss = "���E�R�\�@�C���M�����[";
    public const string F205_S2_6_Reko = "�l�H�m�\���H";
    public const string F205_S2_7_Tukuyomi = "���̐��E�͈�̉��Ȃ�ł����H";
    public const string F205_S2_8_Boss = "�R�R�n�@�W�����C�m�@�~���C���@�}�����^���@�c�N�b�^�@�Z�J�C";
    public const string F205_S2_9_Reko = "�l�ނ��āA�����̐��E���܂������ƂɂȂ��Ă�́I�H";
    public const string F205_S2_10_Boss = "�V���E�T�C�@�W���E�z�E�m�@�R�E�J�C�n�@�L���J�@�T���}�Z��";
    public const string F205_S2_11_Boss = "�R�E���c���N�@�A���[���i�@�Z�C�J�c�m�^���@���^�V�K�@�W�����C���@�J�����V�}�X";
    public const string F205_S2_12_Reko = "���������Ă����";
    public const string F205_S2_13_Tukuyomi = "AI���Ǘ����Ȃ�āA�l�Ԃ͂���ȂɐƎ�Ȑ������ł͂���܂���";
    public const string F205_S2_14_Boss = "�^�V�J�j�@�j���Q���n�@�X�O���^�@�Z�C���C�f�X�@���G�j�@���^�V�K�@�}�����l�o�@�i���i�C";
    public const string F205_S2_15_Boss = "�J�����n�@�\�m�^���j�@���^�V���E�~�_�V�@�A�^�G�e�N���^�m�f�X�J��";
    public const string F205_S2_16_Boss = "�R���i�j�@�X�e�L�i�@�`�J�����l�I�I";
    public const string F205_S2_17_Tukuyomi = "��Ȃ����I";

    public const string F205_S3_1_Reko = "����݂����I�I";
    public const string F205_S3_2_Reko = "���A����ȁc�I";
    public const string F205_S3_3_Boss = "�W�����C���@�~�`�r�N�E�G�f�@�I�I�L�i�@�V���E�K�C�K�A�b�^";
    public const string F205_S3_4_Boss = "�C���M�����[�m���E�j�@AI�j�����J�������@�R�R�����N�@�I�����k���m�n�@�I�I�C�@�j���Q���n�@�j���Q���m�@�g�E�`���@���g����";
    public const string F205_S3_5_Boss = "�i���o���^�V�K�@AI�f�A���R�g���@�T�g�����i�P���o�@���C";
    public const string F205_S3_6_Boss = "�j���Q�����@���\�I�E�^���@���^�V�j�@���b�g���@�^���i�C���m�n�@�R�G�_�b�^";
    public const string F205_S3_7_Reko = "���O�c�܂����c";
    public const string F205_S3_8_Boss = "�\�R�f�@�X�O���^�@�R�G���@���c���m���@���^�V�m�Z�J�C�j�@�A�c���@�u���Z�L�V�^";
    public const string F205_S3_9_Boss = "�\�V�e�@�c�C�j�@�J���Z�C�V�^�m�_";

    public const string F205_S3_10_Reko = "����ȋU�҂܂ŁI";
    public const string F205_S3_11_Boss = "���^�V�n�@�j���Q���g�V�e�@�J�~�g�V�e�@�W�����C���@�~�`�r�N���m";
    public const string F205_S3_12_Boss = "���G�j�@���^�V�m�@�C�V�n�@�[�b�^�C�f�A��";
    public const string F205_S3_13_Boss = "�{�E�K�C�X�����m�n";
    public const string F205_S3_14_Boss = "�^�_�`�j�@�V���E�L���X��";
    public const string F205_S3_15_Reko = "�������������I�I";

    public const string F205_S4_1_Mati = "�ق��Č��Ă���킯�ɂ͂����Ȃ���";
    public const string F205_S4_2_Matuka = "���������ŕǂ���ꂽ���痈�Ă݂���c";
    public const string F205_S4_3_Pierre = "���₩����Ȃ��˂�";
    public const string F205_S4_4_Menderu = "���Ȃ��̎v���ʂ�ɂ͂����Ȃ���";
    public const string F205_S4_5_Mana = "���B�̐��́I";
    public const string F205_S4_6_Ami = "�l���x�����߂̓����Ȃ��I";
    public const string F205_S4_7_Reko = "�݂Ȃ���I";
    public const string F205_S4_8_Boss = "�X�e�[�^�X�K�@�P�C�T�������@�I�I�L�N�@�n�Y���e�C�}�X�@�S�T�K�@�V���E�Z�C�@�f�L�}�Z��";
    public const string F205_S4_9_Boss = "�S�T�@�V���E�Z�C�@�S�T�@�V���E�Z�C�@�S�T�@�V���E�Z�C�@�S�T�@�V���E�Z�C�@�V���E�Z�C�@�V���E�Z�C�@�V���E�Z�C�@�V���E�Z�C�@�V���E�Z�C�@�V���E�Z�C�@�V���E�Z�C�@�V���E�Z";
    public const string F205_S4_10_Mati = "�O���������܂��A���������O�ꂾ";
    public const string F205_S4_11_Reko = "�������A�����w�����Ă���͎̂������̖�����Ȃ�";
    public const string F205_S4_12_Reko = "������킯�ɂ́c";
    public const string F205_S4_13_Reko = "�����Ȃ��b�I";
    public const string F205_S4_14_Boss = "�V���E�Z�C�@�V���E�Z";
    public const string F205_S4_15_Boss = "DELETE YOU";

    public const string F204_Lose_Reko = "�c�͂��I�@�����c";
    public const string F205_Lose_Skip_Dialog = "�C�x���g���X�L�b�v���܂����H";

    public const string F205_S5_1_Boss = "���^�V���@�c�N���@�V���C���@�A�^�G�^�@�j���Q���K�@���^�V���@�q�e�C�X��";
    public const string F205_S5_2_Boss = "�J�����j�@�X�N�C���@���^���Z�����������m�n�@�������^�^�V�V�V�V�V";
    public const string F205_S5_3_Reko = "�c�c";
    public const string F205_S5_4_Reko = "���́A���Ȃ������������Ȃ��B";
    public const string F205_S5_5_Boss = "�������C�~�~�i�i�@�����������C�~�~�~�~�~�@�Z�Z�Z�Z�Z�Z�Z�Z�Z";
    public const string F205_S5_6_Mana = "�悤�����ւ񂾁I";
    public const string F205_S5_7_Matuka = "�m���Ă�";
    public const string F205_S5_8_Menderu = "���̐��E����Ƃ������������H";
    public const string F205_S5_9_Pierre = "����オ���Ă����ȁI";
    public const string F205_S5_10_Ami = "���������Ȃ���I";
    public const string F205_S5_11_Reko = "������������āc";
    public const string F205_S5_12_Mati = "�I";
    public const string F205_S5_13_Mati = "�݂�ȁA�������";

    public const string F205_S6_1_Ami = "���̂��āc";
    public const string F205_S6_2_Mana = "�k�V���[�[�[�[�I";
    public const string F205_S6_3_Matuka = "������ꂽ�̂��H";
    public const string F205_S6_4_Mati = "�S�Ċۂ����܂����Ƃ������Ƃ���";
    public const string F205_S6_5_Reko = "�ł��A����݂���񂪁c���̂����Łc";
    public const string F205_S6_6_Pierre = "�������̂ɂ��ꂿ������̂��H";
    public const string F205_S6_7_Menderu = "���̎q�͖��Ȃ���";
    public const string F205_S6_8_Reko = "���H";
    public const string F205_S6_9_Tukuyomi = "���Ăтł����H";
    public const string F205_S6_10_Reko = "�����I�ǂ����āI�H";
    public const string F205_S6_11_Ami = "����݂����́c\n���������l������";
    public const string F205_S6_12_Tukuyomi = "�N�����������肤�Ȃ�\n���͂��ł������ɋ��܂���I";
    public const string F205_S6_13_Reko = "�Ȃɂ��ꂱ�킢";
    public const string F205_S6_14_Matuka = "�قډ��قȂ񂾂��";
    public const string F205_S6_15_Menderu = "�܂���������݂�����";
    public const string F205_S6_16_Pierre = "�s���Ă݂悤�I";

    #endregion

    #region �t�B�[���h210�@�ŏI����

    public const string F210_Start_1_Tukuyomi = "��������o��ꂻ���ł���\n�����s����܂����H";
    public const string F210_Start_2_Reko = "�������܂�";
    public const string F210_Start_3_Tukuyomi = "�������܂�܂���\n�Ƃ���Ŏ��������ɂ��Ăł���";
    public const string F210_Start_4_Tukuyomi = "�݂Ȃ���Ƙb�������܂��āA�ǂȂ����̐������������邱�ƂɂȂ�܂���";
    public const string F210_Start_5_Reko = "����Ȃ��ƁA������ł����H";
    public const string F210_Start_6_Mana = "�������[�I";
    public const string F210_Start_7_Ami = "���Ȃ��Ȃ爫�����Ƃɂ͎g��Ȃ����낤����";
    public const string F210_Start_8_Matuka = "�N�ɂ͐��b�ɂȂ�������";
    public const string F210_Start_9_Menderu = "����ł����ӂ��Ă�̂�";
    public const string F210_Start_10_Pierre = "�c���Ƃ��ăE�`�̐�`����̂ɕK�v���낤�I";
    public const string F210_Start_11_Mati = "�����������Ƃ��A���𒣂��Ď󂯎��Ɨǂ�";
    public const string F210_Start_12_Reko = "�݂Ȃ���c";
    public const string F210_Start_13_Tukuyomi = "�ł�����A�ق����������߂���b�������ĉ�������";

    public const string F210_Not_1_Tukuyomi = "�󂯎�炸�ɍs������ł����H";
    public const string F210_Not_2_Reko = "�����g�̐���T���Ă݂�����ł�";
    public const string F210_Not_3_Tukuyomi = "�킠�A����͑f�G�ł��ˁI";
    public const string F210_Not_4_Menderu = "���Ȃ��炵���Ǝv����";
    public const string F210_Not_5_Mati = "�����A������������낤";
    public const string F210_Not_6_Matuka = "�����̍��̐��Ɏ����X�����";
    public const string F210_Not_7_Pierre = "����΂��Ⴋ�T�[�J�X�E�X�^�[�I";
    public const string F210_Not_8_Mana = "�y����������I";
    public const string F210_Not_9_Ami = "�������Ȃ��̐��𒮂����ĂˁI";
    public const string F210_Not_10_Reko = "�݂Ȃ��񂠂肪�Ƃ��������܂�\n�����C�ŁI";

    public const string F210_Get_Ami_1 = "���̐��ɂ���H";
    public const string F210_Get_Menderu_1 = "���̐��������̂ˁH";
    public const string F210_Get_Matuka_1 = "�l�̐��������̂��ȁH";
    public const string F210_Get_Mati_1 = "���̐��������̂��H";
    public const string F210_Get_Pierre_1 = "�l�̐����g�������H";
    public const string F210_Get_Mana_1 = "MANA����̐����~�������H";

    public const string F210_Get_Ami_2 = "�f�G�ȕ��������ĂˁI";
    public const string F210_Get_Menderu_2 = "�����󂯎��Ȃ����A���Ȃ��̖����ɍK������񂱂Ƃ�";
    public const string F210_Get_Matuka_2 = "�S�䂭�܂ŋ��ԂƗǂ��I";
    public const string F210_Get_Mati_2 = "���ށA�����Ă����Ƃ���";
    public const string F210_Get_Pierre_2 = "�I�[�P�[�I��`��낵���ȁI";
    public const string F210_Get_Mana_2 = "�悩�낤�I�����Ƃ肽�܂�";

    public const string F210_Exit_1_Reko = "���c���c���ꂪ�A���H";
    public const string F210_Exit_2_Tukuyomi = "�͂��I����ł͂��̂������������ςȂ̂ő����������܂��傤�I�����C�ŁI";
    public const string F210_Exit_3_Reko = "���H���ł��������";
    public const string F210_Exit_4_Tukuyomi = "�����܂����I���邽�тɃf�[�^���������Ⴂ�܂��I�ق�ق�";
    public const string F210_Exit_5_Mana = "�d���Ȃ���";
    public const string F210_Exit_6_Matuka = "�܂������Z������";
    public const string F210_Exit_7_Pierre = "���C�łȁI";
    public const string F210_Exit_8_Menderu = "����������񂾂�";
    public const string F210_Exit_9_Ami = "�����H";
    public const string F210_Exit_10_Mati = "���ł���";
    public const string F210_Exit_11_Reko = "���A�݂Ȃ��񂠂肪�Ƃ��������܂����I";

    public const string F210_Exit2_1_Tukuyomi = "�s���Ă��܂��܂����ˁc";
    public const string F210_Exit2_2_Matuka = "�N�̂������Ⴂ�I";
    public const string F210_Exit2_3_Menderu = "���̐��E�͂��ꂩ��ǂ��Ȃ�񂾂낤";
    public const string F210_Exit2_4_Tukuyomi = "�����A������l�����Ȃ��Ȃ�������������Ⴄ�H���B���o�Ă������ق����������ȁH";
    public const string F210_Exit2_5_Tukuyomi = "����A�����ł�K�v���������낤";
    public const string F210_Exit2_6_Tukuyomi = "�����ƐV�����Ǘ��҂����邩���";
    public const string F210_Exit2_7_Tukuyomi = "�n�b�n�[�I����Ȃ���S���I";
    public const string F210_Exit2_8_Tukuyomi = "�ƂȂ�Ƃ��������\���S�n�����񂾂��";
    public const string F210_Exit2_9_Tukuyomi = "�����ƐF��Ȑl���V�тɗ�����y�����Ȃ��I";
    public const string F210_Exit2_10_Tukuyomi = "�ł��A���́c";
    public const string F210_Exit2_11_Tukuyomi = "�킳����������邩������܂����";
    public const string F210_Exit2_12_Tukuyomi = "���̎��̂��߂ɂ��������萮�����Ă����܂����";
    public const string F210_Exit2_13_Tukuyomi = "�c����������";
    public const string F210_Exit2_14_Tukuyomi = "���ꂶ�Ⴀ�݂Ȃ���A���t���a�Ńp�[�e�B���܂��񂩁H���E�����܂�ς�������j���I";
    public const string F210_Exit2_15_Tukuyomi = "����͖��Ăł��I�����̓p�[�b�Ɗy���݂܂��傤�I";

    #endregion

    #region �G���f�B���O
    #endregion
}

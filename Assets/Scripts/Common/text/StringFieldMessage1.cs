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
    public const string F008_Board = "���̐������҂�\n�@���͊J�����";
    public const string F009_Board1 = "\n���呛���͏Z���ɔz�����ė��ꏬ����";
    public const string F009_Board2 = "\n������\n�@�Â��ȏꏊ�Ő��_����";
    public const string F009_Board3 = "\n���N���E���T�[�J�X";
    public const string F010_Board1 = "\n��MANA����̂������I";
    public const string F010_Board2 = "\n���J�t�F ���t���a";
    public const string F010_Board3 = "\n���\���ԋ���@COEIRO�x��";

    #endregion
}

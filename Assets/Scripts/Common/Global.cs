using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Global
{
    #region �C���X�^���X�擾
    private static SaveData _saveData = null;
    /// <summary>
    /// �Z�[�u�f�[�^
    /// </summary>
    /// <returns></returns>
    public static SaveData GetSaveData()
    {
        if (_saveData == null)
        {
            _saveData = new SaveData();
        }
        return _saveData;
    }

    private static TemporaryData _temporaryData = null;
    /// <summary>
    /// �ꎞ�f�[�^
    /// </summary>
    /// <returns></returns>
    public static TemporaryData GetTemporaryData()
    {
        if (_temporaryData == null)
        {
            _temporaryData = new TemporaryData();
        }
        return _temporaryData;
    }
    #endregion

    /// <summary>
    /// �Z�[�u�f�[�^
    /// </summary>
    public class SaveData
    {
        /// <summary>�v�����[�O�����t���O</summary>
        public int stage0Clear;

        /// <summary>�I�v�V����</summary>
        public SystemData system;

        /// <summary>
        /// �I�v�V����
        /// </summary>
        public struct SystemData
        {
            /// <summary>BGM</summary>
            public int bgmVolume;
            /// <summary>SE</summary>
            public int seVolume;
            /// <summary>�{�C�X</summary>
            public int voiceVolume;

            /// <summary>�N���A�t���O</summary>
            public int clearFlag;
        }

        public SaveData()
        {
            stage0Clear = 0;

            system.bgmVolume = 3;
            system.seVolume = 3;
            system.voiceVolume = 3;
            system.clearFlag = 0;
        }

        /// <summary>
        /// �Q�[���f�[�^���Z�[�u
        /// </summary>
        public void SaveGameData()
        {
            PlayerPrefs.SetInt("stage0Clear", stage0Clear);

            PlayerPrefs.Save();
        }

        /// <summary>
        /// �Q�[���f�[�^�����[�h
        /// </summary>
        public void LoadGameData()
        {
            stage0Clear = PlayerPrefs.GetInt("stage0Clear", 0);
        }

        /// <summary>
        /// �Z�[�u�����邩�ǂ���
        /// </summary>
        /// <returns></returns>
        public bool IsEnableGameData()
        {
            return false;
        }

        /// <summary>
        /// �V�X�e���f�[�^���Z�[�u
        /// </summary>
        public void SaveSystemData()
        {
            PlayerPrefs.SetInt("optionBgmVolume", system.bgmVolume);
            PlayerPrefs.SetInt("optionSeVolume", system.seVolume);
            PlayerPrefs.SetInt("optionVoiceVolume", system.voiceVolume);

            PlayerPrefs.SetInt("optionClearFlag", system.clearFlag);

            PlayerPrefs.Save();
        }

        /// <summary>
        /// �V�X�e���f�[�^�����[�h
        /// </summary>
        public void LoadSystemData()
        {
            system.bgmVolume = PlayerPrefs.GetInt("optionBgmVolume", 3);
            system.seVolume = PlayerPrefs.GetInt("optionSeVolume", 3);
            system.voiceVolume = PlayerPrefs.GetInt("optionVoiceVolume", 3);

            system.clearFlag = PlayerPrefs.GetInt("optionClearFlag", 0);
        }
    }

    /// <summary>
    /// �ۑ����Ȃ��f�[�^
    /// </summary>
    public class TemporaryData
    {
        /// <summary>�n���f�p��������</summary>
        public int loseCount;

        /// <summary>�Q�[���߂莞�̏�������</summary>
        public bool gameWon;

        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        public TemporaryData()
        {
            loseCount = 0;
            gameWon = false;
        }
    }
}

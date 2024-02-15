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
        public OptionData option;

        /// <summary>
        /// 
        /// </summary>
        public struct OptionData
        {
            /// <summary>BGM</summary>
            public int bgmVolume;
            /// <summary>SE</summary>
            public int seVolume;
            /// <summary>�{�C�X</summary>
            public int voiceVolume;
        }

        public SaveData()
        {
            stage0Clear = 0;

            option.bgmVolume = 32;
            option.seVolume = 32;
            option.voiceVolume = 32;
        }

        /// <summary>
        /// �Z�[�u
        /// </summary>
        public void Save()
        {
            PlayerPrefs.SetInt("stage0Clear", stage0Clear);

            PlayerPrefs.SetInt("optionBgmVolume", option.bgmVolume);
            PlayerPrefs.SetInt("optionSeVolume", option.seVolume);
            PlayerPrefs.SetInt("optionVoiceVolume", option.voiceVolume);

            PlayerPrefs.Save();
        }

        /// <summary>
        /// ���[�h
        /// </summary>
        public void Load()
        {
            stage0Clear = PlayerPrefs.GetInt("stage0Clear", 0);

            option.bgmVolume = PlayerPrefs.GetInt("optionBgmVolume", 32);
            option.seVolume = PlayerPrefs.GetInt("optionSeVolume", 32);
            option.voiceVolume = PlayerPrefs.GetInt("optionVoiceVolume", 32);
        }
    }

    /// <summary>
    /// �ۑ����Ȃ��f�[�^
    /// </summary>
    public class TemporaryData
    {

        public TemporaryData()
        {
        }
    }
}

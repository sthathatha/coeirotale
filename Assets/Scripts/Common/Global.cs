using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

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
        /// <summary>�Q�[�����̃f�[�^</summary>
        public Dictionary<string, string> gameData;

        /// <summary>�V�X�e���f�[�^</summary>
        public SystemData system;

        /// <summary>
        /// �V�X�e���f�[�^
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

        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        public SaveData()
        {
            gameData = new Dictionary<string, string>();

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
            var serial = ToSaveString(gameData);
            PlayerPrefs.SetString("gameData", serial);

            PlayerPrefs.Save();
        }

        /// <summary>
        /// �Q�[���f�[�^�����[�h
        /// </summary>
        public void LoadGameData()
        {
            ReadSaveString(PlayerPrefs.GetString("gameData"));
        }

        /// <summary>
        /// �Q�[���f�[�^������
        /// </summary>
        public void InitGameData()
        {
            gameData.Clear();

            //todo:�e�X�g�p
            //gameData[F101System.PLANT_FLG] = "3";
            //gameData[F111System.BRIDGE_FLG] = "2";
            //gameData[F121System.KEY_FLG] = "3";
            //gameData[F131System.ICE_BLOCK_FLG] = "1";
            //gameData[F131System.ICE_YOU_FLG] = "3";
            //gameData[F141System.CLEAR_FLG] = "1";
            //gameData[F151BoardSource.BOARD_USE_FLG] = "1";
            //gameData[F122System.F122_PIERRE_PHASE] = "2";
            //gameData[F143System.MENDERU_WIN_FLG] = "1";
            //gameData[F153System.AMI_WIN_FLG] = "1";
        }

        /// <summary>
        /// �Z�[�u�����邩�ǂ���
        /// </summary>
        /// <returns></returns>
        public bool IsEnableGameData()
        {
            return PlayerPrefs.HasKey("gameData");
        }

        /// <summary>
        /// �Q�[���f�[�^�Z�b�g
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetGameData(string key, string value)
        {
            gameData[key] = value;
        }

        /// <summary>
        /// �Q�[���f�[�^�Z�b�g������
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetGameData(string key, int value)
        {
            SetGameData(key, value.ToString());
        }

        /// <summary>
        /// �Q�[���f�[�^������擾
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetGameDataString(string key)
        {
            return gameData.ContainsKey(key) ? gameData[key] : "";
        }

        /// <summary>
        /// �Q�[���f�[�^�𐮐��Ŏ擾
        /// </summary>
        /// <param name="key"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public int GetGameDataInt(string key, int def = 0)
        {
            int ret;
            if (int.TryParse(GetGameDataString(key), out ret))
            {
                return ret;
            }
            else
            {
                return def;
            }
        }

        /// <summary>
        /// �O���{�X�̃N���A�����擾
        /// </summary>
        /// <returns></returns>
        public int GetABossClearCount()
        {
            var cnt = 0;
            if (GetGameDataInt(F102System.MATI_WIN_FLG) == 1) cnt++;
            if (GetGameDataInt(F112System.MATUKA_WIN_FLG) == 1) cnt++;
            if (GetGameDataInt(F122System.F122_PIERRE_PHASE) >= 2) cnt++;
            if (GetGameDataInt(F132System.MANA_WIN_FLG) == 1) cnt++;
            if (GetGameDataInt(F143System.MENDERU_WIN_FLG) == 1) cnt++;
            if (GetGameDataInt(F153System.AMI_WIN_FLG) == 1) cnt++;

            return cnt;
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

        /// <summary>
        /// �Z�[�u�p������ɕϊ�
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private string ToSaveString(Dictionary<string, string> data)
        {
            var strList = data.Select((pair, idx) => pair.Key + ":" + pair.Value);

            return string.Join(',', strList);
        }

        /// <summary>
        /// �Z�[�u�p�������ǂݍ���
        /// </summary>
        /// <param name="data"></param>
        private void ReadSaveString(string data)
        {
            gameData.Clear();
            foreach (var str in data.Split(','))
            {
                var pair = str.Split(':');
                gameData[pair[0]] = pair[1];
            }
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

        /// <summary>�{�X���b�V���t���O</summary>
        public bool bossRush;

        /// <summary>�{�X���b�V�����s�@���t���A�~</summary>
        public bool bossRushAmiWon;
        /// <summary>�{�X���b�V�����s�@MANA</summary>
        public bool bossRushManaWon;
        /// <summary>�{�X���b�V�����s�@�R�̃}�`</summary>
        public bool bossRushMatiWon;
        /// <summary>�{�X���b�V�����s�@�����f��</summary>
        public bool bossRushMenderuWon;
        /// <summary>�{�X���b�V�����s�@���܂肷��</summary>
        public bool bossRushMatukaWon;
        /// <summary>�{�X���b�V�����s�@�s�G�[��</summary>
        public bool bossRushPierreWon;
        /// <summary>���X�{�X�ɕ������t���O</summary>
        public bool lastBossLost;

        /// <summary>�N���A���̑I��</summary>
        public int ending_select_voice;

        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        public TemporaryData()
        {
            loseCount = 0;
            gameWon = false;
            bossRush = false;

            bossRushAmiWon = false;
            bossRushManaWon = false;
            bossRushMatiWon = false;
            bossRushMatukaWon = false;
            bossRushMenderuWon = false;
            bossRushPierreWon = false;

            lastBossLost = false;
            ending_select_voice = -1;
        }
    }
}

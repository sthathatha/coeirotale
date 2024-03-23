using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Global
{
    #region インスタンス取得
    private static SaveData _saveData = null;
    /// <summary>
    /// セーブデータ
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
    /// 一時データ
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
    /// セーブデータ
    /// </summary>
    public class SaveData
    {
        /// <summary>プロローグ見たフラグ</summary>
        public int stage0Clear;

        /// <summary>オプション</summary>
        public OptionData option;

        /// <summary>
        /// オプション
        /// </summary>
        public struct OptionData
        {
            /// <summary>BGM</summary>
            public int bgmVolume;
            /// <summary>SE</summary>
            public int seVolume;
            /// <summary>ボイス</summary>
            public int voiceVolume;
        }

        public SaveData()
        {
            stage0Clear = 0;

            option.bgmVolume = 3;
            option.seVolume = 3;
            option.voiceVolume = 3;
        }

        /// <summary>
        /// セーブ
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
        /// ロード
        /// </summary>
        public void Load()
        {
            stage0Clear = PlayerPrefs.GetInt("stage0Clear", 0);

            option.bgmVolume = PlayerPrefs.GetInt("optionBgmVolume", 3);
            option.seVolume = PlayerPrefs.GetInt("optionSeVolume", 3);
            option.voiceVolume = PlayerPrefs.GetInt("optionVoiceVolume", 3);
        }
    }

    /// <summary>
    /// 保存しないデータ
    /// </summary>
    public class TemporaryData
    {
        /// <summary>ハンデ用負けた回数</summary>
        public int loseCount;

        /// <summary>ゲーム戻り時の勝利判定</summary>
        public bool gameWon;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TemporaryData()
        {
            loseCount = 0;
            gameWon = false;
        }
    }
}

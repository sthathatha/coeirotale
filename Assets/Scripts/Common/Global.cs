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
        public SystemData system;

        /// <summary>
        /// オプション
        /// </summary>
        public struct SystemData
        {
            /// <summary>BGM</summary>
            public int bgmVolume;
            /// <summary>SE</summary>
            public int seVolume;
            /// <summary>ボイス</summary>
            public int voiceVolume;

            /// <summary>クリアフラグ</summary>
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
        /// ゲームデータをセーブ
        /// </summary>
        public void SaveGameData()
        {
            PlayerPrefs.SetInt("stage0Clear", stage0Clear);

            PlayerPrefs.Save();
        }

        /// <summary>
        /// ゲームデータをロード
        /// </summary>
        public void LoadGameData()
        {
            stage0Clear = PlayerPrefs.GetInt("stage0Clear", 0);
        }

        /// <summary>
        /// セーブがあるかどうか
        /// </summary>
        /// <returns></returns>
        public bool IsEnableGameData()
        {
            return false;
        }

        /// <summary>
        /// システムデータをセーブ
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
        /// システムデータをロード
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

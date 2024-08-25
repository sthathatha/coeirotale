using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

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
        /// <summary>ゲーム内のデータ</summary>
        public Dictionary<string, string> gameData;

        /// <summary>システムデータ</summary>
        public SystemData system;

        /// <summary>
        /// システムデータ
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

        /// <summary>
        /// コンストラクタ
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
        /// ゲームデータをセーブ
        /// </summary>
        public void SaveGameData()
        {
            var serial = ToSaveString(gameData);
            PlayerPrefs.SetString("gameData", serial);

            PlayerPrefs.Save();
        }

        /// <summary>
        /// ゲームデータをロード
        /// </summary>
        public void LoadGameData()
        {
            ReadSaveString(PlayerPrefs.GetString("gameData"));
        }

        /// <summary>
        /// ゲームデータ初期化
        /// </summary>
        public void InitGameData()
        {
            gameData.Clear();

            //todo:テスト用
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
        /// セーブがあるかどうか
        /// </summary>
        /// <returns></returns>
        public bool IsEnableGameData()
        {
            return PlayerPrefs.HasKey("gameData");
        }

        /// <summary>
        /// ゲームデータセット
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetGameData(string key, string value)
        {
            gameData[key] = value;
        }

        /// <summary>
        /// ゲームデータセット整数版
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetGameData(string key, int value)
        {
            SetGameData(key, value.ToString());
        }

        /// <summary>
        /// ゲームデータ文字列取得
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetGameDataString(string key)
        {
            return gameData.ContainsKey(key) ? gameData[key] : "";
        }

        /// <summary>
        /// ゲームデータを整数で取得
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
        /// 前半ボスのクリア数を取得
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

        /// <summary>
        /// セーブ用文字列に変換
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private string ToSaveString(Dictionary<string, string> data)
        {
            var strList = data.Select((pair, idx) => pair.Key + ":" + pair.Value);

            return string.Join(',', strList);
        }

        /// <summary>
        /// セーブ用文字列を読み込み
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
    /// 保存しないデータ
    /// </summary>
    public class TemporaryData
    {
        /// <summary>ハンデ用負けた回数</summary>
        public int loseCount;

        /// <summary>ゲーム戻り時の勝利判定</summary>
        public bool gameWon;

        /// <summary>ボスラッシュフラグ</summary>
        public bool bossRush;

        /// <summary>ボスラッシュ勝敗　小春音アミ</summary>
        public bool bossRushAmiWon;
        /// <summary>ボスラッシュ勝敗　MANA</summary>
        public bool bossRushManaWon;
        /// <summary>ボスラッシュ勝敗　軍歌マチ</summary>
        public bool bossRushMatiWon;
        /// <summary>ボスラッシュ勝敗　メンデル</summary>
        public bool bossRushMenderuWon;
        /// <summary>ボスラッシュ勝敗　松嘩りすく</summary>
        public bool bossRushMatukaWon;
        /// <summary>ボスラッシュ勝敗　ピエール</summary>
        public bool bossRushPierreWon;
        /// <summary>ラスボスに負けたフラグ</summary>
        public bool lastBossLost;

        /// <summary>クリア時の選択声</summary>
        public int ending_select_voice;

        /// <summary>
        /// コンストラクタ
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

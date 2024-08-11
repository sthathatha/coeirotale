using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ラスボス本戦データベース
/// </summary>
public static class BossGameBDataBase
{
    #region スキルデータ

    /// <summary>
    /// 射程距離のタイプ
    /// </summary>
    public enum RangeTypeEnum : int
    {
        /// <summary>全方位</summary>
        All = 0,
        /// <summary>直線のみ全方位</summary>
        AllLine,
        /// <summary>斜めのみ全方位</summary>
        AllCrossLine,
        /// <summary>縦横のみ全方位</summary>
        AllPlusLine,
        /// <summary>前方（斜め）・左右（縦）のみ直線</summary>
        ThreeLine,
        /// <summary>前方（斜め）のみ直線</summary>
        FrontLine,
    }

    /// <summary>
    /// 対象選択タイプ
    /// </summary>
    public enum TargetTypeEnum : int
    {
        /// <summary>なんでも</summary>
        All = 0,
        /// <summary>（使用者の）敵</summary>
        Enemy,
        /// <summary>（使用者の）味方</summary>
        Fellow,
    }

    /// <summary>
    /// スキル
    /// </summary>
    public enum SkillID : int
    {
        /// <summary>サイクロトロン</summary>
        Reko1_C = 0,
        /// <summary>オクトストライク</summary>
        Reko2_O,
        /// <summary>イーグルダイブ</summary>
        Reko3_E,
        /// <summary>インビンシブル</summary>
        Reko4_I,
        /// <summary>リインカーネーション</summary>
        Reko5_R,
        /// <summary>オリジン</summary>
        Reko6_O,
        /// <summary>イグニッション</summary>
        Reko7_I,
        /// <summary>ナイトメア</summary>
        Reko8_N,
        /// <summary>キーニングシルフ</summary>
        Reko9_K,
        /// <summary>カルネージ</summary>
        Boss1_Carnage,
        /// <summary>プラズマ</summary>
        Boss2_Plasma,
        /// <summary>アームストロング</summary>
        Boss3_Canon,
        /// <summary>トランキーライザー</summary>
        Boss4_Tranqui,
        /// <summary>はるのとなり</summary>
        Ami1,
        /// <summary>ショウダウン</summary>
        Mana1,
        /// <summary>喝</summary>
        Matuka1,
        /// <summary>刹那の見斬り</summary>
        Mati1,
        /// <summary>マントラップヴァイン</summary>
        Menderu1,
        /// <summary>ジャグリングヒット</summary>
        Pierre1,
    }

    /// <summary>
    /// スキルデータクラス
    /// </summary>
    public class SkillData
    {
        /// <summary>名称</summary>
        public string Name;
        /// <summary>説明</summary>
        public string Detail;

        /// <summary>射程距離の向き</summary>
        public RangeTypeEnum RangeType;
        /// <summary>射程距離リスト 0:自分</summary>
        public List<int> RangeList;
        /// <summary>効果範囲（0なら単体、1で周囲1マスの3x3</summary>
        public int EffectRange;

        /// <summary>選択対象</summary>
        public TargetTypeEnum TargetType;

        /// <summary>効果量</summary>
        public int Value;

        /// <summary>
        /// 初期化コンストラクタ
        /// 特に指定しなければ自分対象の設定
        /// </summary>
        public SkillData()
        {
            Name = "";
            Detail = "";
            RangeType = RangeTypeEnum.All;
            RangeList = new List<int>() { 0 };
            EffectRange = 0;
            TargetType = TargetTypeEnum.All;
            Value = 0;
        }
    }

    private static Dictionary<SkillID, SkillData> _skillList = null;
    /// <summary>
    /// スキルリスト
    /// </summary>
    public static Dictionary<SkillID, SkillData> SkillList
    {
        get
        {
            if (_skillList == null)
            {
                _skillList = new Dictionary<SkillID, SkillData>();

                #region レコスキル
                _skillList.Add(SkillID.Reko1_C, new SkillData()
                {
                    Name = StringMinigameMessage.BossB_SkillA1_Name,
                    Detail = StringMinigameMessage.BossB_SkillA1_Detail,
                    Value = 120,
                });
                _skillList.Add(SkillID.Reko2_O, new SkillData()
                {
                    Name = StringMinigameMessage.BossB_SkillA2_Name,
                    Detail = StringMinigameMessage.BossB_SkillA2_Detail,
                    TargetType = BossGameBDataBase.TargetTypeEnum.Enemy,
                    RangeList = new List<int>() { 1 },
                    Value = 888,
                });
                _skillList.Add(SkillID.Reko3_E, new SkillData()
                {
                    Name = StringMinigameMessage.BossB_SkillA3_Name,
                    Detail = StringMinigameMessage.BossB_SkillA3_Detail,
                    TargetType = BossGameBDataBase.TargetTypeEnum.Enemy,
                    RangeType = BossGameBDataBase.RangeTypeEnum.AllLine,
                    RangeList = new List<int>() { 3 },
                    Value = 500,
                });
                _skillList.Add(SkillID.Reko4_I, new SkillData()
                {
                    Name = StringMinigameMessage.BossB_SkillA4_Name,
                    Detail = StringMinigameMessage.BossB_SkillA4_Detail,
                });
                _skillList.Add(SkillID.Reko5_R, new SkillData()
                {
                    Name = StringMinigameMessage.BossB_SkillA5_Name,
                    Detail = StringMinigameMessage.BossB_SkillA5_Detail,
                });
                _skillList.Add(SkillID.Reko6_O, new SkillData()
                {
                    Name = StringMinigameMessage.BossB_SkillA6_Name,
                    Detail = StringMinigameMessage.BossB_SkillA6_Detail,
                    EffectRange = 6,
                });
                _skillList.Add(SkillID.Reko7_I, new SkillData()
                {
                    Name = StringMinigameMessage.BossB_SkillA7_Name,
                    Detail = StringMinigameMessage.BossB_SkillA7_Detail,
                    Value = 120,
                });
                _skillList.Add(SkillID.Reko8_N, new SkillData()
                {
                    Name = StringMinigameMessage.BossB_SkillA8_Name,
                    Detail = StringMinigameMessage.BossB_SkillA8_Detail,
                    TargetType = BossGameBDataBase.TargetTypeEnum.Enemy,
                    RangeType = BossGameBDataBase.RangeTypeEnum.All,
                    RangeList = new List<int>() { 1, 2 },
                    EffectRange = 1,
                    Value = 300,
                });
                _skillList.Add(SkillID.Reko9_K, new SkillData()
                {
                    Name = StringMinigameMessage.BossB_SkillA9_Name,
                    Detail = StringMinigameMessage.BossB_SkillA9_Detail,
                    TargetType = TargetTypeEnum.Enemy,
                    EffectRange = 2,
                    Value = 500,
                });

                #endregion

                #region ボス
                _skillList.Add(SkillID.Boss1_Carnage, new SkillData()
                {
                    Name = StringMinigameMessage.BossB_SkillBBoss1_Name,
                    EffectRange = 6,
                    RangeList = new List<int>() { 0, 1, 2, 3, 4, 5, 6 },
                    Value = 800,
                });
                _skillList.Add(SkillID.Boss2_Plasma, new SkillData()
                {
                    Name = StringMinigameMessage.BossB_SkillBBoss2_Name,
                    TargetType = TargetTypeEnum.Enemy,
                    EffectRange = 6,
                    RangeList = new List<int>() { 0, 1, 2, 3, 4, 5, 6 },
                    Value = 300,
                });
                _skillList.Add(SkillID.Boss3_Canon, new SkillData()
                {
                    Name = StringMinigameMessage.BossB_SkillBBoss3_Name,
                    RangeType = RangeTypeEnum.AllCrossLine,
                    RangeList = new List<int>() { 0, 1, 2, 3, 4, 5, 6 },
                    Value = 1300,
                });
                _skillList.Add(SkillID.Boss4_Tranqui, new SkillData()
                {
                    Name = StringMinigameMessage.BossB_SkillBBoss4_Name,
                    RangeType = RangeTypeEnum.AllPlusLine,
                    RangeList = new List<int>() { 0, 1, 2, 3, 4, 5, 6 },
                    Value = 300,
                });

                #endregion

                #region 他

                // アミ
                _skillList.Add(SkillID.Ami1, new SkillData()
                {
                    Name = StringMinigameMessage.BossB_SkillBAmi1_Name,
                    Detail = StringMinigameMessage.BossB_SkillBAmi1_Detail,
                    TargetType = TargetTypeEnum.Fellow,
                    EffectRange = 1,
                    Value = 1100,
                });
                // MANA
                _skillList.Add(SkillID.Mana1, new SkillData()
                {
                    Name = StringMinigameMessage.BossB_SkillBMana1_Name,
                    Detail = StringMinigameMessage.BossB_SkillBMana1_Detail,
                    TargetType = TargetTypeEnum.Fellow,
                    EffectRange = 6,
                });
                // まつかりすく
                _skillList.Add(SkillID.Matuka1, new SkillData()
                {
                    Name = StringMinigameMessage.BossB_SkillBMatuka1_Name,
                    Detail = StringMinigameMessage.BossB_SkillBMatuka1_Detail,
                    EffectRange = 6,
                    Value = 150,
                });
                // メンデル
                _skillList.Add(SkillID.Menderu1, new SkillData()
                {
                    Name = StringMinigameMessage.BossB_SkillBMenderu1_Name,
                    Detail = StringMinigameMessage.BossB_SkillBMenderu1_Detail,
                    TargetType = TargetTypeEnum.Enemy,
                    RangeList = new List<int>() { 1, 2 },
                    EffectRange = 1,
                    Value = 100,
                });
                // マチ
                _skillList.Add(SkillID.Mati1, new SkillData()
                {
                    Name = StringMinigameMessage.BossB_SkillBMati1_Name,
                    Detail = StringMinigameMessage.BossB_SkillBMati1_Detail,
                    TargetType = TargetTypeEnum.Enemy,
                    RangeType = RangeTypeEnum.ThreeLine,
                    RangeList = new List<int>() { 1 },
                    Value = 1300,
                });
                // ピエール
                _skillList.Add(SkillID.Pierre1, new SkillData()
                {
                    Name = StringMinigameMessage.BossB_SkillBPierre1_Name,
                    Detail = StringMinigameMessage.BossB_SkillBPierre1_Detail,
                    TargetType = TargetTypeEnum.Enemy,
                    RangeType = RangeTypeEnum.ThreeLine,
                    RangeList = new List<int>() { 1, 2 },
                    Value = 30,
                });

                #endregion
            }

            return _skillList;
        }
    }

    #endregion

}

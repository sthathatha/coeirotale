using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// ラスボス本戦　敵キャラ基本
/// </summary>
public class BossGameBEnemy : BossGameBCharacterBase
{
    /// <summary>
    /// 初期化
    /// </summary>
    protected override void Start()
    {
        base.Start();

        CharacterType = CharaType.Enemy;
    }

    /// <summary>
    /// ターン行動
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator TurnProcess()
    {
        var sound = ManagerSceneScript.GetInstance().soundMan;
        // 行動決定
        var ai = DecideAI();

        switch (ai.Act)
        {
            case AIResult.ActionType.Skill:
                yield return UseSkillBase(ai.SkillID, ai.SkillTargetLoc);
                system.playerWalkReset = true;
                break;
            case AIResult.ActionType.ChangeDir:
                SetDirection(ai.ChangeDir);
                break;
            case AIResult.ActionType.Walk:
                yield return Walk(ai.WalkLoc.x, ai.WalkLoc.y);
                // 地形効果チェック
                var effect = system.GetCellFieldEffect(location);
                if (effect == BossGameBDataObject.FieldEffect.Plasma)
                {
                    // ダメージうける
                    sound.PlaySE(system.dataObj.se_field_plasma);
                    yield return HitDamage(Util.RandomInt(160, 240), true);
                    system.ClearFieldEffect(location);
                }
                break;
        }

        yield return null;
    }

    /// <summary>
    /// 行動決定共通
    /// 特殊AIは派生でオーバーライド
    /// </summary>
    protected virtual AIResult DecideAI()
    {
        var ret = new AIResult();

        // 使用可能なスキルを検索
        var enableSkillList = new Dictionary<BossGameBDataBase.SkillID, List<Vector2Int>>();
        foreach (var sid in skillList)
        {
            var sdata = BossGameBDataBase.SkillList[sid];
            var cells = BossGameSystemB.CreateEnableCellList(sid, this);
            var targetCellList = new List<Vector2Int>();
            foreach (var cell in cells)
            {
                var chr = system.GetCellCharacter(cell);
                if (chr == null) continue;
                if (sdata.TargetType == BossGameBDataBase.TargetTypeEnum.Fellow && chr.CharacterType != this.CharacterType) continue;
                if (sdata.TargetType == BossGameBDataBase.TargetTypeEnum.Enemy && chr.CharacterType == this.CharacterType) continue;

                targetCellList.Add(cell);
                break;
            }
            if (targetCellList.Any())
                enableSkillList.Add(sid, targetCellList);
        }

        // あったらその中からランダムで選ぶ
        if (enableSkillList.Any())
        {
            ret.Act = AIResult.ActionType.Skill;
            var idList = enableSkillList.Keys.ToList();
            var sid = idList[Util.RandomInt(0, enableSkillList.Count - 1)];
            var sdata = BossGameBDataBase.SkillList[sid];
            var cellList = enableSkillList[sid];
            ret.SkillID = sid;
            ret.SkillTargetLoc = cellList[Util.RandomInt(0, cellList.Count - 1)];
            return ret;
        }

        // 無ければプレイヤーに近づきたい
        var pLoc = system.GetPlayerLoc();
        var dist = pLoc - location;

        // 距離1ならプレイヤーの方を向いて終了
        if (Mathf.Abs(dist.x) <= 1 && Mathf.Abs(dist.y) <= 1)
        {
            ret.Act = AIResult.ActionType.ChangeDir;
            ret.ChangeDir = GetVectorDirection(dist);
            return ret;
        }

        // 座標が大きい方を目標
        var walkTarget = new Vector2Int(0, 0);
        if (Mathf.Abs(dist.x) >= Mathf.Abs(dist.y))
        {
            walkTarget.x = dist.x > 0 ? 1 : -1;
            // ただし歩きたい方向が移動不可ならもう片方
            if (!system.CanWalk(location + walkTarget))
            {
                walkTarget.x = 0;
                walkTarget.y = dist.y > 0 ? 1 : -1;
            }
        }
        else
        {
            walkTarget.y = dist.y > 0 ? 1 : -1;
            if (!system.CanWalk(location + walkTarget))
            {
                walkTarget.y = 0;
                walkTarget.x = dist.x > 0 ? 1 : -1;
            }
        }

        // どちらも移動不可ならプレイヤーの方に向きを変えるだけで終わり
        if (!system.CanWalk(location + walkTarget))
        {
            ret.Act = AIResult.ActionType.ChangeDir;
            ret.ChangeDir = GetVectorDirection(dist);
            return ret;
        }

        // 移動したい方向に向いているかチェック
        var vecDir = BossGameSystemB.GetDirectionCell(nowDirection);
        bool checkWalkDir = walkTarget.x > 0 && vecDir.x > 0 ||
            walkTarget.x < 0 && vecDir.x < 0 ||
            walkTarget.y > 0 && vecDir.y > 0 ||
            walkTarget.y < 0 && vecDir.y < 0;

        if (checkWalkDir)
        {
            // 向きOKなら歩きで確定
            ret.Act = AIResult.ActionType.Walk;
            ret.WalkLoc = walkTarget;
            return ret;
        }

        // OKじゃないならプレイヤーの方に向きを変えて終わり
        ret.Act = AIResult.ActionType.ChangeDir;
        ret.ChangeDir = GetVectorDirection(dist);
        return ret;
    }

    #region 行動決定クラス

    /// <summary>
    /// AI選択情報クラス
    /// </summary>
    protected class AIResult
    {
        public enum ActionType
        {
            /// <summary>スキル使用</summary>
            Skill = 0,
            /// <summary>向きを変える</summary>
            ChangeDir,
            /// <summary>歩く</summary>
            Walk,
            /// <summary></summary>
            None,
        }

        /// <summary>アクション種類</summary>
        public ActionType Act;

        /// <summary>スキル使用の場合　ID</summary>
        public BossGameBDataBase.SkillID SkillID;
        /// <summary>スキル使用の場合　対象セル</summary>
        public Vector2Int SkillTargetLoc;

        /// <summary>向き変更の場合Dir</summary>
        public CharaDirection ChangeDir;

        /// <summary>歩く場合　移動先</summary>
        public Vector2Int WalkLoc;
    }
    #endregion
}

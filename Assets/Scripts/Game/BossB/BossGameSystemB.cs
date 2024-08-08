using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// ボス　＊＊＊＊戦
/// </summary>
public class BossGameSystemB : GameSceneScriptBase
{
    #region 定数

    /// <summary>
    /// キャラID
    /// </summary>
    public enum CharacterID : int
    {
        Player = 0,
        Boss,
        Ami,
        Mana,
        Matuka,
        Menderu,
        Mati,
        Pierre,
    }

    /// <summary>1マスの幅</summary>
    public const float CELL_WIDTH = 130f;
    /// <summary>1マスの高さ</summary>
    public const float CELL_HEIGHT = 86f;

    /// <summary>マスのX個数</summary>
    public const int CELL_X_COUNT = 7;
    /// <summary>マスのY個数</summary>
    public const int CELL_Y_COUNT = 7;

    #endregion

    #region メンバー

    public BossGameBDataObject dataObj;

    public BossGameBPlayer player;
    public BossGameBEnemy ami;
    public BossGameBEnemy mana;
    public BossGameBEnemy matuka;
    public BossGameBEnemy menderu;
    public BossGameBEnemy mati;
    public BossGameBEnemy pierre;
    public BossGameBEnemy boss;

    public BossGameBUICommand commandUI;
    public BossGameBUISkillName skillNameUI;
    public BossGameBUITurnShow turnUI;
    public BossGameBUICellSelect cellUI;

    public Transform hpParent;
    public BossGameBUIHPShow hpDummy;
    public BossGameBUIDamage damageDummy;

    public Transform effectParent;
    public Transform cellEffectParent;
    public BossGameBStatusBuffEffect buffEffectDummy;
    public BossGameBGeneralEffect generalEffectDummy;
    public BossGameBHorrorEffect horrorEffectDummy;
    public BossGameBOriginEffect originEffectDummy;

    public AudioClip se_turnStart;
    public AudioClip se_statusUp;
    public AudioClip se_statusDown;

    #endregion

    #region 変数

    /// <summary>生きてるキャラリスト</summary>
    private List<BossGameBCharacterBase> characterList = new List<BossGameBCharacterBase>();

    /// <summary>プレイヤーの歩数リセットするフラグ</summary>
    public bool playerWalkReset { get; set; } = true;

    #endregion

    #region 基底

    /// <summary>
    /// 開始前
    /// </summary>
    /// <returns></returns>
    public override IEnumerator Start()
    {
        var g = Global.GetTemporaryData();
        yield return base.Start();

        characterList.Add(player);
        characterList.Add(boss);
        // 連戦結果によって削除
        if (g.bossRushAmiWon) ami.gameObject.SetActive(false);
        else characterList.Add(ami);
        if (g.bossRushManaWon) mana.gameObject.SetActive(false);
        else characterList.Add(mana);
        if (g.bossRushMatukaWon) matuka.gameObject.SetActive(false);
        else characterList.Add(matuka);
        if (g.bossRushMenderuWon) menderu.gameObject.SetActive(false);
        else characterList.Add(menderu);
        if (g.bossRushMatiWon) mati.gameObject.SetActive(false);
        else characterList.Add(mati);
        if (g.bossRushPierreWon) pierre.gameObject.SetActive(false);
        else characterList.Add(pierre);

        foreach (var chara in characterList)
        {
            chara.InitParameter();
            chara.ResetTime(true);
        }

        // UI一旦非表示
        commandUI.Close();
        skillNameUI.Hide();
        turnUI.Show(new List<CharacterID>());

        // ダミー非表示
        hpDummy.gameObject.SetActive(false);
        damageDummy.gameObject.SetActive(false);
        generalEffectDummy.gameObject.SetActive(false);
        buffEffectDummy.gameObject.SetActive(false);
        horrorEffectDummy.gameObject.SetActive(false);
        originEffectDummy.gameObject.SetActive(false);
    }

    /// <summary>
    /// フェードイン後
    /// </summary>
    /// <returns></returns>
    public override IEnumerator AfterFadeIn()
    {
        yield return base.AfterFadeIn();

        StartCoroutine(MainCoroutine());
    }

    #endregion

    #region 動作

    /// <summary>
    /// メインコルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator MainCoroutine()
    {
        var sound = ManagerSceneScript.GetInstance().soundMan;
        var input = InputManager.GetInstance();

        while (true)
        {
            yield return null;
            // 誰かが行動した後はプレイヤーの歩数リセット
            if (playerWalkReset)
            {
                player.ResetWalkCount();
            }

            // 行動するキャラを決定
            BossGameBCharacterBase turnChara = null;
            var nextTime = int.MaxValue;
            foreach (var chara in characterList)
            {
                if (chara.GetWaitTime() < nextTime)
                {
                    turnChara = chara;
                    nextTime = chara.GetWaitTime();
                }
            }

            // 全員の待機時間を減らす
            foreach (var chara in characterList)
            {
                chara.DecreaseTime(nextTime);
            }

            // UI更新
            UpdateTurnUI();

            if (turnChara.CharacterType == BossGameBCharacterBase.CharaType.Player)
            {
                if (playerWalkReset)
                {
                    // プレイヤーターンの開始時
                    sound.PlaySE(se_turnStart);
                    // HP表示
                    yield return ShowHp();
                    playerWalkReset = false;
                }
            }

            // 行動処理
            yield return turnChara.TurnProcessBase();

            // ボスが死亡していたら終了処理
            if (!characterList.Contains(boss))
            {
                skillNameUI.Show(StringMinigameMessage.BossB_Win);
                break;
            }

            // プレイヤーが死亡していたら終了処理
            if (!characterList.Contains(player))
            {
                skillNameUI.Show(StringMinigameMessage.BossB_Lose);
                break;
            }

            // 行動したキャラの待機時間を再設定
            turnChara.ResetTime();
        }

        yield return new WaitForSeconds(2f);
        ManagerSceneScript.GetInstance().ExitGame();
    }

    #endregion

    #region システム機能

    /// <summary>
    /// セルの座標計算　左下(0,0)　右上(6,6)
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public static Vector3 GetCellPosition(Vector2Int location)
    {
        return new Vector3(location.x * CELL_WIDTH, location.y * CELL_HEIGHT);
    }

    /// <summary>
    /// ターン表示更新
    /// </summary>
    /// <param name="deleteTop">先頭のアイコンを確実に消すか</param>
    private void UpdateTurnUI(bool deleteTop = true)
    {
        var turnList = new List<CharacterID>();
        var tmpWaitTime = new List<int>();
        var tmpMaxWaitTime = new List<int>();
        foreach (var chara in characterList)
        {
            tmpWaitTime.Add(chara.GetWaitTime());
            tmpMaxWaitTime.Add(chara.GetMaxWaitTime());
        }

        for (var i = 0; i < 6; ++i)
        {
            var min = tmpWaitTime.Min();
            var idx = tmpWaitTime.IndexOf(min);
            turnList.Add(characterList[idx].GetCharacterID());

            for (var j = 0; j < tmpWaitTime.Count; ++j)
            {
                tmpWaitTime[j] -= min;
            }
            tmpWaitTime[idx] = tmpMaxWaitTime[idx];
        }

        turnUI.Show(turnList, deleteTop);
    }

    /// <summary>
    /// 歩き行動時間の続行チェック
    /// </summary>
    /// <param name="excludeCharacter">自身を判定から除く</param>
    /// <param name="time">経過する時間</param>
    /// <returns></returns>
    public bool CanWalkWait(BossGameBCharacterBase excludeCharacter, int time)
    {
        foreach (var character in characterList)
        {
            if (character == excludeCharacter) continue;
            if (character.GetWaitTime() < time) return false;
        }

        return true;
    }

    /// <summary>
    /// 全体の待ち時間を減らす
    /// </summary>
    /// <param name="excludeCharacter">自身を除く</param>
    /// <param name="time">経過時間</param>
    public void DecreaseAllCharacterWait(BossGameBCharacterBase excludeCharacter, int time)
    {
        foreach (var character in characterList)
        {
            if (character == excludeCharacter) continue;
            character.DecreaseTime(time);
        }

        UpdateTurnUI(false);
    }

    /// <summary>
    /// スキル使用可能位置を作成
    /// </summary>
    /// <param name="skillId">スキル</param>
    /// <param name="character">使用者</param>
    /// <param name="isPlayer">プレイヤーの場合向きに関係なく全方位に使用可能</param>
    public static List<Vector2Int> CreateEnableCellList(BossGameBDataBase.SkillID skillId, BossGameBCharacterBase character)
    {
        var basePos = character.GetLocation();
        var baseDir = character.GetDirection();
        var isPlayer = character.CharacterType == BossGameBCharacterBase.CharaType.Player;

        var offsetList = new List<Vector2Int>();
        var skill = BossGameBDataBase.SkillList[skillId];
        var dirCell = GetDirectionCell(baseDir);
        var rangeType = skill.RangeType;
        if (isPlayer)
        {
            if (rangeType == BossGameBDataBase.RangeTypeEnum.ThreeLine)
                rangeType = BossGameBDataBase.RangeTypeEnum.AllLine;
            else if (rangeType == BossGameBDataBase.RangeTypeEnum.FrontLine)
                rangeType = BossGameBDataBase.RangeTypeEnum.AllCrossLine;
        }

        // サイズにより位置オフセット（偶数に未対応）
        var outW = character.body_width / 2;
        var outH = character.body_height / 2;

        // 基本位置
        foreach (var r in skill.RangeList)
        {
            switch (rangeType)
            {
                // 全方位
                case BossGameBDataBase.RangeTypeEnum.All:
                    for (var x = -r - outW; x <= r + outW; ++x)
                    {
                        // 上と下辺
                        offsetList.Add(new Vector2Int(x, r + outH));
                        if (r != 0) // 0の場合は重なるのでOK
                            offsetList.Add(new Vector2Int(x, -r - outH));
                    }
                    for (var y = -r - outH + 1; y <= r + outH - 1; ++y)
                    {
                        // 左と右辺
                        offsetList.Add(new Vector2Int(r + outW, y));
                        offsetList.Add(new Vector2Int(-r - outW, y));
                    }
                    break;
                case BossGameBDataBase.RangeTypeEnum.AllLine:
                    // 8方向
                    offsetList.Add(new Vector2Int(r + outW, r + outH));
                    offsetList.Add(new Vector2Int(r + outW, -r - outH));
                    offsetList.Add(new Vector2Int(-r - outW, r + outH));
                    offsetList.Add(new Vector2Int(-r - outW, -r - outH));
                    for (var x = -outW; x <= outW; ++x)
                    {
                        offsetList.Add(new Vector2Int(x, r + outH));
                        offsetList.Add(new Vector2Int(x, -r - outH));
                    }
                    for (var y = -outH; y <= outH; ++y)
                    {
                        offsetList.Add(new Vector2Int(r + outW, y));
                        offsetList.Add(new Vector2Int(-r - outW, y));
                    }
                    break;
                case BossGameBDataBase.RangeTypeEnum.AllCrossLine:
                    // 斜め４方向
                    offsetList.Add(new Vector2Int(r + outW, r + outH));
                    offsetList.Add(new Vector2Int(r + outW, -r - outH));
                    offsetList.Add(new Vector2Int(-r - outW, r + outH));
                    offsetList.Add(new Vector2Int(-r - outW, -r - outH));
                    break;
                case BossGameBDataBase.RangeTypeEnum.AllPlusLine:
                    // 縦横
                    for (var x = -outW; x <= outW; ++x)
                    {
                        offsetList.Add(new Vector2Int(x, r + outH));
                        offsetList.Add(new Vector2Int(x, -r - outH));
                    }
                    for (var y = -outH; y <= outH; ++y)
                    {
                        offsetList.Add(new Vector2Int(r + outW, y));
                        offsetList.Add(new Vector2Int(-r - outW, y));
                    }
                    break;
                case BossGameBDataBase.RangeTypeEnum.ThreeLine:
                    // 向いてる方向の斜め
                    offsetList.Add(new Vector2Int(dirCell.x * (r + outW), dirCell.y * (r + outH)));
                    // 向いてる方向の横
                    for (var y = -outH; y <= outH; ++y)
                    {
                        offsetList.Add(new Vector2Int(dirCell.x * (r + outW), y));
                    }
                    // 向いてる方向の縦
                    for (var x = -outW; x <= outW; ++x)
                    {
                        offsetList.Add(new Vector2Int(x, dirCell.y * (r + outH)));
                    }
                    break;
                case BossGameBDataBase.RangeTypeEnum.FrontLine:
                    // 向いてる方向の斜めのみ
                    offsetList.Add(new Vector2Int(dirCell.x * (r + outW), dirCell.y * (r + outH)));
                    break;
            }
        }

        // キャラ位置を加算して範囲内のセルのみ
        var list = new List<Vector2Int>();
        foreach (var ofs in offsetList)
        {
            var pos = ofs + basePos + new Vector2Int(0, outH); // 大サイズは中央一番下が基準なのでYにプラス
            if (pos.x < 0 || pos.x >= CELL_X_COUNT ||
                pos.y < 0 || pos.y >= CELL_Y_COUNT) continue;

            list.Add(pos);
        }

        return list;
    }

    /// <summary>
    /// 向いてる方向の単位ベクトル
    /// </summary>
    /// <param name="dir"></param>
    /// <returns></returns>
    public static Vector2Int GetDirectionCell(BossGameBCharacterBase.CharaDirection dir)
    {
        return dir switch
        {
            BossGameBCharacterBase.CharaDirection.LeftUp => new Vector2Int(-1, 1),
            BossGameBCharacterBase.CharaDirection.RightUp => new Vector2Int(1, 1),
            BossGameBCharacterBase.CharaDirection.RightDown => new Vector2Int(1, -1),
            _ => new Vector2Int(-1, -1),
        };
    }

    /// <summary>
    /// 位置にいるキャラクターを取得
    /// </summary>
    /// <param name="loc"></param>
    /// <returns></returns>
    public BossGameBCharacterBase GetCellCharacter(Vector2Int loc)
    {
        foreach (var chara in characterList)
        {
            var center = chara.GetLocation();
            if (center.x - chara.body_width / 2 > loc.x) continue;
            if (center.x + chara.body_width / 2 < loc.x) continue;
            if (center.y > loc.y) continue;
            if (center.y + chara.body_height - 1 < loc.y) continue;

            return chara;
        }

        return null;
    }

    /// <summary>
    /// その場所に移動できるか
    /// </summary>
    /// <param name="loc"></param>
    /// <returns></returns>
    public bool CanWalk(Vector2Int loc)
    {
        if (loc.x < 0 || loc.y < 0 || loc.x >= CELL_X_COUNT || loc.y >= CELL_Y_COUNT) return false;
        if (GetCellCharacter(loc) != null) return false;

        return true;
    }

    /// <summary>
    /// プレイヤーの座標
    /// </summary>
    /// <returns></returns>
    public Vector2Int GetPlayerLoc() { return player.GetLocation(); }

    /// <summary>
    /// キャラクターを削除
    /// </summary>
    /// <param name="chara"></param>
    public void RemoveCharacter(BossGameBCharacterBase chara)
    {
        characterList.Remove(chara);
    }

    /// <summary>
    /// HP表示
    /// </summary>
    /// <returns></returns>
    public IEnumerator ShowHp()
    {
        var input = InputManager.GetInstance();

        var hpList = new List<BossGameBUIHPShow>();
        foreach (var character in characterList)
        {
            var show = Instantiate(hpDummy);
            var p = GetCellPosition(character.GetLocation());
            show.transform.SetParent(hpParent);
            show.Show(character.GetHp(), character.GetHpMax(), p);
            hpList.Add(show);
        }

        yield return null;
        yield return new WaitUntil(() => input.GetKeyPress(InputManager.Keys.South) ||
            input.GetKeyPress(InputManager.Keys.Up) ||
            input.GetKeyPress(InputManager.Keys.Down) ||
            input.GetKeyPress(InputManager.Keys.Left) ||
            input.GetKeyPress(InputManager.Keys.Right) ||
            input.GetKeyPress(InputManager.Keys.East)
            );

        foreach (var show in hpList)
        {
            Destroy(show.gameObject);
        }

        yield return new WaitWhile(() =>
            input.GetKey(InputManager.Keys.Up) ||
            input.GetKey(InputManager.Keys.Down) ||
            input.GetKey(InputManager.Keys.Left) ||
            input.GetKey(InputManager.Keys.Right)
        );
    }

    /// <summary>
    /// ダメージ表示
    /// </summary>
    /// <param name="loc"></param>
    /// <param name="damage">マイナスで回復</param>
    public IEnumerator ShowDamage(Vector2Int loc, int damage)
    {
        var dmg = Instantiate(damageDummy);
        dmg.transform.SetParent(hpParent);
        dmg.transform.localPosition = GetCellPosition(loc);
        yield return dmg.ShowDamage(damage);
        Destroy(dmg.gameObject);
    }

    #endregion

    #region エフェクト管理

    /// <summary>
    /// 速度バフ・デバフ演出
    /// </summary>
    /// <param name="characterList"></param>
    /// <param name="rate"></param>
    /// <returns></returns>
    public IEnumerator BuffSpeed(List<BossGameBCharacterBase> characterList, float rate)
    {
        var isUp = rate > 1f;
        foreach (var character in characterList)
        {
            if (!isUp && character.IsInvincible()) continue;

            character.ChangeSpeed(rate);
            var buff = Instantiate(buffEffectDummy);
            buff.SetParam(isUp, BossGameBStatusBuffEffect.Type.Speed);
            buff.transform.SetParent(cellEffectParent);
            buff.PlayAndDestroy(GetCellPosition(character.GetLocation()));
        }

        ManagerSceneScript.GetInstance().soundMan.PlaySE(isUp ? se_statusUp : se_statusDown);

        yield return new WaitForSeconds(0.5f);
    }

    /// <summary>
    /// 攻撃力バフ・デバフ演出
    /// </summary>
    /// <param name="characterList"></param>
    /// <param name="rate"></param>
    /// <returns></returns>
    public IEnumerator BuffAttack(List<BossGameBCharacterBase> characterList, float rate)
    {
        var isUp = rate > 1f;
        foreach (var character in characterList)
        {
            if (!isUp && character.IsInvincible()) continue;

            character.ChangeAttackRate(rate);
            var buff = Instantiate(buffEffectDummy);
            buff.SetParam(isUp, BossGameBStatusBuffEffect.Type.Strength);
            buff.transform.SetParent(cellEffectParent);
            buff.PlayAndDestroy(GetCellPosition(character.GetLocation()));
        }

        ManagerSceneScript.GetInstance().soundMan.PlaySE(isUp ? se_statusUp : se_statusDown);

        yield return new WaitForSeconds(0.5f);
    }

    /// <summary>
    /// 汎用エフェクト生成
    /// </summary>
    /// <param name="cellPosition"></param>
    /// <param name="kind">種類</param>
    public void CreateGeneralEffect(Vector3 cellPosition, BossGameBDataObject.EffectKind kind)
    {
        var eff = Instantiate(generalEffectDummy);
        eff.transform.SetParent(cellEffectParent);
        eff.SetParam(dataObj.GetGeneralEffectList(kind));
        eff.PlayAndDestroy(cellPosition);
    }

    /// <summary>
    /// 回復エフェクト
    /// </summary>
    /// <param name="center"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public IEnumerator PlayHealEffect(Vector2Int center, int width, int height)
    {
        var basePos = GetCellPosition(center);
        var ofsX = (width / 2 / 2f + 0.25f) * CELL_WIDTH;
        var ofsY = (height / 2 / 2f + 0.25f) * CELL_HEIGHT;
        var base1 = basePos + new Vector3(ofsX, ofsY);
        var base2 = basePos + new Vector3(-ofsX, -ofsY);
        var base3 = basePos + new Vector3(-ofsX, ofsY);
        var base4 = basePos + new Vector3(ofsX, -ofsY);

        for (var i = 0; i < 5; ++i)
        {
            var rand = new Vector3(Util.RandomFloat(-ofsX, ofsX), Util.RandomFloat(-ofsY, ofsY));
            CreateGeneralEffect(base1 + rand, BossGameBDataObject.EffectKind.Heal);
            yield return new WaitForSeconds(0.03f);
            rand = new Vector3(Util.RandomFloat(-ofsX, ofsX), Util.RandomFloat(-ofsY, ofsY));
            CreateGeneralEffect(base2 + rand, BossGameBDataObject.EffectKind.Heal);
            yield return new WaitForSeconds(0.03f);
            rand = new Vector3(Util.RandomFloat(-ofsX, ofsX), Util.RandomFloat(-ofsY, ofsY));
            CreateGeneralEffect(base3 + rand, BossGameBDataObject.EffectKind.Heal);
            yield return new WaitForSeconds(0.03f);
            rand = new Vector3(Util.RandomFloat(-ofsX, ofsX), Util.RandomFloat(-ofsY, ofsY));
            CreateGeneralEffect(base4 + rand, BossGameBDataObject.EffectKind.Heal);
            yield return new WaitForSeconds(0.03f);
        }
    }

    /// <summary>
    /// ホラーエフェクト再生
    /// </summary>
    /// <param name="center"></param>
    public void PlayHorrorEffect(Vector2Int center)
    {
        var eff = Instantiate(horrorEffectDummy);
        eff.transform.SetParent(cellEffectParent);
        eff.PlayAndDestroy(GetCellPosition(center));
    }

    /// <summary>
    /// 斬撃エフェクト
    /// </summary>
    /// <param name="center"></param>
    /// <param name="direction"></param>
    public void CreateSlashEffect(Vector2Int center, Vector3 direction)
    {
        var rad = Util.GetRadianFromVector(direction);
        var eff = Instantiate(generalEffectDummy);
        eff.transform.SetParent(cellEffectParent);
        eff.SetParam(dataObj.GetGeneralEffectList(BossGameBDataObject.EffectKind.Slash));
        eff.model.transform.localRotation = Util.GetRotateQuaternion(rad);
        eff.PlayAndDestroy(GetCellPosition(center));
    }

    /// <summary>
    /// オリジンエフェクト
    /// </summary>
    /// <param name="center"></param>
    public void CreateOriginEffect(Vector2Int center)
    {
        var eff = Instantiate(originEffectDummy);
        eff.transform.SetParent(cellEffectParent);
        eff.PlayAndDestroy(GetCellPosition(center));
    }

    #endregion
}

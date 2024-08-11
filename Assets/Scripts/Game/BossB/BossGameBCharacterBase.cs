using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// ラスボス本戦　キャラクター共通処理
/// </summary>
public class BossGameBCharacterBase : MonoBehaviour
{
    #region 定数

    /// <summary>キャラの向き</summary>
    public enum CharaDirection : int
    {
        LeftUp = 0,
        RightUp,
        LeftDown,
        RightDown,
    }

    /// <summary>キャラ属性</summary>
    public enum CharaType : int
    {
        Player = 0,
        Enemy,
    }

    #endregion

    #region メンバー

    /// <summary>ゲームシステム</summary>
    public BossGameSystemB system;

    /// <summary>モデル</summary>
    public SpriteRenderer model;

    /// <summary>右上画像</summary>
    public Sprite sp_rightup = null;
    /// <summary>左上画像</summary>
    public Sprite sp_leftup = null;
    /// <summary>右下画像</summary>
    public Sprite sp_rightdown = null;
    /// <summary>左下画像</summary>
    public Sprite sp_leftdown = null;

    /// <summary>横マス数</summary>
    public int body_width = 1;
    /// <summary>縦マス数</summary>
    public int body_height = 1;

    /// <summary>初期座標</summary>
    public int init_locx = 0;
    /// <summary>初期座標</summary>
    public int init_locy = 0;
    /// <summary>初期向き</summary>
    public int init_dir_x = 1;
    /// <summary>初期向き</summary>
    public int init_dir_y = 1;

    #endregion

    #region 変数

    /// <summary>現在の向き</summary>
    protected CharaDirection nowDirection;

    /// <summary>現在座標</summary>
    protected Vector2Int location;

    /// <summary>最大HP</summary>
    public int param_HP_max;
    /// <summary>現在HP</summary>
    protected int param_HP;
    /// <summary>攻撃力変動値</summary>
    protected float param_ATK_rate;
    /// <summary>基本速度</summary>
    public int param_SPD_base;
    /// <summary>速度変動値</summary>
    protected float param_SPD_rate;
    /// <summary>行動待ち時間</summary>
    protected int param_wait_time;

    /// <summary>使用可能スキル</summary>
    protected List<BossGameBDataBase.SkillID> skillList;

    /// <summary>キャラ属性</summary>
    public CharaType CharacterType { get; set; }

    /// <summary>ダメージ演出再生中</summary>
    private bool damageEffecting = false;

    /// <summary>無敵フラグ</summary>
    protected bool isInvincible = false;

    #endregion

    #region 初期化

    /// <summary>
    /// 初期化
    /// </summary>
    protected virtual void Start()
    {
        SetDirection(init_dir_x, init_dir_y);
        location = new Vector2Int(init_locx, init_locy);
        transform.localPosition = BossGameSystemB.GetCellPosition(location);

        skillList = new List<BossGameBDataBase.SkillID>();
    }

    #endregion

    #region 機能

    /// <summary>
    /// キャラID　派生先で実装
    /// </summary>
    /// <returns></returns>
    public virtual BossGameSystemB.CharacterID GetCharacterID() { return BossGameSystemB.CharacterID.Player; }

    /// <summary>
    /// Vectorから向きを設定
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void SetDirection(int x, int y)
    {
        // 上下・左右をフラグ判定
        var isRight = false;
        var isUp = false;

        if (x > 0) isRight = true;
        else if (x < 0) isRight = false;
        else isRight = nowDirection == CharaDirection.RightUp || nowDirection == CharaDirection.RightDown;

        if (y > 0) isUp = true;
        else if (y < 0) isUp = false;
        else isUp = nowDirection == CharaDirection.RightUp || nowDirection == CharaDirection.LeftUp;


        // 右上
        if (isRight && isUp) SetDirection(CharaDirection.RightUp);
        // 右下
        else if (isRight && !isUp) SetDirection(CharaDirection.RightDown);
        // 左上
        else if (!isRight && isUp) SetDirection(CharaDirection.LeftUp);
        // 左下
        else SetDirection(CharaDirection.LeftDown);
    }

    /// <summary>
    /// 向きを設定
    /// </summary>
    /// <param name="dir"></param>
    public void SetDirection(CharaDirection dir)
    {
        // 画像変更
        model.sprite = dir switch
        {
            CharaDirection.LeftUp => sp_leftup,
            CharaDirection.RightUp => sp_rightup,
            CharaDirection.RightDown => sp_rightdown,
            _ => sp_leftdown,
        };

        nowDirection = dir;
    }

    /// <summary>
    /// 現在の向き
    /// </summary>
    /// <returns></returns>
    public CharaDirection GetDirection() { return nowDirection; }

    /// <summary>
    /// 現在の向きに画像をリセット
    /// </summary>
    public void ResetDirection() { SetDirection(nowDirection); }

    /// <summary>
    /// 現在位置
    /// </summary>
    /// <returns></returns>
    public Vector2Int GetLocation() { return location; }

    #region パラメータ管理

    /// <summary>
    /// パラメータ初期化
    /// </summary>
    public void InitParameter()
    {
        param_ATK_rate = 1f;
        param_SPD_rate = 1f;
        param_HP = param_HP_max;

        param_wait_time = CalcWaitTime(param_SPD_base);
    }

    /// <summary>
    /// 基本の行動待機時間
    /// </summary>
    /// <returns></returns>
    public int GetMaxWaitTime() { return CalcWaitTime(Mathf.FloorToInt(param_SPD_base * param_SPD_rate)); }
    /// <summary>
    /// 現在の待ち時間
    /// </summary>
    /// <returns></returns>
    public int GetWaitTime() { return param_wait_time; }
    /// <summary>
    /// 待ち時間を減らす
    /// </summary>
    /// <param name="t"></param>
    public void DecreaseTime(int t) { param_wait_time -= t; }
    /// <summary>
    /// 行動遅延
    /// </summary>
    /// <param name="turn">遅延回数</param>
    public void DelayTime(int turn) { param_wait_time += turn * GetMaxWaitTime(); }

    /// <summary>
    /// 待ち時間を再設定
    /// </summary>
    /// <param name="init"></param>
    public virtual void ResetTime(bool init = false) { param_wait_time = GetMaxWaitTime(); }
    /// <summary>
    /// 速度変化
    /// </summary>
    /// <param name="mul"></param>
    public void ChangeSpeed(float mul)
    {
        var before = param_SPD_rate;
        param_SPD_rate *= mul;

        // 待ち時間を変動
        ChangeWaitTimeRate(before, param_SPD_rate);
    }

    /// <summary>
    /// 攻撃力変化
    /// </summary>
    /// <param name="mul"></param>
    public void ChangeAttackRate(float mul)
    {
        param_ATK_rate *= mul;
    }

    /// <summary>
    /// バフ・デバフをリセット
    /// </summary>
    public void ResetParam()
    {
        // 待ち時間を変動
        ChangeWaitTimeRate(param_SPD_rate, 1f);

        param_ATK_rate = 1f;
        param_SPD_rate = 1f;
    }

    /// <summary>
    /// SPD変化により待ち時間を変動
    /// </summary>
    /// <param name="beforeRate"></param>
    /// <param name="afterRate"></param>
    private void ChangeWaitTimeRate(float beforeRate, float afterRate)
    {
        var beforeTimeMax = CalcWaitTime(Mathf.FloorToInt(param_SPD_base * beforeRate));
        var afterTimeMax = CalcWaitTime(Mathf.FloorToInt(param_SPD_base * afterRate));
        var spdRate = (float)afterTimeMax / beforeTimeMax;

        var newTime = param_wait_time * spdRate;
        if (afterTimeMax > beforeTimeMax)
        {
            // 増える時は切り下げ
            param_wait_time = Mathf.FloorToInt(newTime);
        }
        else
        {
            // 減る時は切り上げ
            param_wait_time = Mathf.CeilToInt(newTime);
        }
    }

    /// <summary>
    /// 現在HP
    /// </summary>
    /// <returns></returns>
    public int GetHp() { return param_HP; }
    /// <summary>
    /// 最大HP
    /// </summary>
    /// <returns></returns>
    public int GetHpMax() { return param_HP_max; }

    /// <summary>
    /// 無敵中
    /// </summary>
    /// <returns></returns>
    public bool IsInvincible() { return isInvincible; }

    #endregion

    #endregion

    #region 汎用メソッド

    /// <summary>
    /// SPDによりかかる標準待機時間
    /// </summary>
    /// <param name="spd"></param>
    /// <returns></returns>
    protected static int CalcWaitTime(int spd)
    {
        var tmp = 100 - spd;
        if (tmp < 10) tmp = 10;
        return tmp;
    }

    /// <summary>
    /// スキルが当たる相手を検索
    /// </summary>
    /// <param name="skillID"></param>
    /// <param name="center"></param>
    /// <returns></returns>
    protected List<BossGameBCharacterBase> GetSkillHitCharacters(BossGameBDataBase.SkillID skillID, Vector2Int center)
    {
        var skill = BossGameBDataBase.SkillList[skillID];
        var list = new List<BossGameBCharacterBase>();
        var cellList = BossGameSystemB.CreateSkillEffectCellList(skillID, center);

        foreach (var cell in cellList)
        {
            var chara = system.GetCellCharacter(cell);
            if (chara == null) continue;
            if (skill.TargetType == BossGameBDataBase.TargetTypeEnum.Fellow && chara.CharacterType != CharacterType)
                continue;
            if (skill.TargetType == BossGameBDataBase.TargetTypeEnum.Enemy && chara.CharacterType == CharacterType)
                continue;

            if (!list.Contains(chara)) list.Add(chara);
        }

        return list;
    }

    /// <summary>
    /// ベクトルに該当するDirectionを選択
    /// </summary>
    /// <param name="vec"></param>
    /// <returns></returns>
    protected CharaDirection GetVectorDirection(Vector2Int vec)
    {
        var nowDirVec = BossGameSystemB.GetDirectionCell(nowDirection);
        // xが0の場合は現在の向きを採用
        if (vec.x == 0) vec.x = nowDirVec.x;
        // yが0の場合は現在の向きを採用
        if (vec.y == 0) vec.y = nowDirVec.y;

        if (vec.x > 0 && vec.y > 0) return CharaDirection.RightUp;
        else if (vec.x > 0 && vec.y < 0) return CharaDirection.RightDown;
        else if (vec.x < 0 && vec.y > 0) return CharaDirection.LeftUp;
        else return CharaDirection.LeftDown;
    }

    #endregion

    #region コルーチン

    /// <summary>
    /// 歩き移動
    /// </summary>
    /// <param name="x">移動距離</param>
    /// <param name="y">移動距離</param>
    /// <returns></returns>
    public IEnumerator Walk(int x, int y)
    {
        /// 画像変更
        SetDirection(x, y);

        location.x += x;
        location.y += y;

        var deltaPos = new DeltaVector3();
        deltaPos.Set(transform.localPosition);
        deltaPos.MoveTo(BossGameSystemB.GetCellPosition(location), 0.2f, DeltaFloat.MoveType.LINE);
        while (deltaPos.IsActive())
        {
            yield return null;
            deltaPos.Update(Time.deltaTime);
            transform.localPosition = deltaPos.Get();
        }
    }

    /// <summary>
    /// ふっとばされ
    /// </summary>
    /// <param name="x">移動距離</param>
    /// <param name="y">移動距離</param>
    /// <returns></returns>
    public IEnumerator Slide(int x, int y)
    {
        location.x += x;
        location.y += y;

        var deltaPos = new DeltaVector3();
        deltaPos.Set(transform.localPosition);
        deltaPos.MoveTo(BossGameSystemB.GetCellPosition(location), 0.05f, DeltaFloat.MoveType.LINE);
        while (deltaPos.IsActive())
        {
            yield return null;
            deltaPos.Update(Time.deltaTime);
            transform.localPosition = deltaPos.Get();
        }
    }

    /// <summary>
    /// 座標そのままで画像のみ移動
    /// </summary>
    /// <param name="targetLoc">移動先の座標</param>
    /// <param name="time">移動時間</param>
    /// <param name="jumpH">ジャンプする場合</param>
    /// <returns></returns>
    public IEnumerator EffectMove(Vector2Int targetLoc, float time, float jumpH = 0f)
    {
        var jumpR = new DeltaFloat();
        jumpR.Set(0f);
        jumpR.MoveTo(Mathf.PI, time, DeltaFloat.MoveType.LINE);
        var deltaPos = new DeltaVector3();
        deltaPos.Set(transform.localPosition);
        deltaPos.MoveTo(BossGameSystemB.GetCellPosition(targetLoc), time, DeltaFloat.MoveType.LINE);
        while (deltaPos.IsActive())
        {
            yield return null;
            deltaPos.Update(Time.deltaTime);
            jumpR.Update(Time.deltaTime);

            var pos = deltaPos.Get();
            pos += new Vector3(0, Mathf.Sin(jumpR.Get()) * jumpH);
            transform.localPosition = pos;
        }
    }

    /// <summary>
    /// ターン行動
    /// </summary>
    /// <returns></returns>
    public IEnumerator TurnProcessBase()
    {
        model.sortingOrder = 10;
        isInvincible = false;

        yield return TurnProcess();

        model.sortingOrder = 0;
    }

    /// <summary>
    /// ターン行動本体
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator TurnProcess()
    {
        // 派生先で実装
        yield return new WaitForSeconds(1f);
    }

    /// <summary>
    /// スキル使用
    /// </summary>
    /// <param name="skillID"></param>
    /// <param name="targetCell"></param>
    /// <returns></returns>
    protected IEnumerator UseSkillBase(BossGameBDataBase.SkillID skillID, Vector2Int targetCell)
    {
        var skill = BossGameBDataBase.SkillList[skillID];
        var nameUI = system.skillNameUI;
        var cells = system.cellUI;

        // スキル名表示
        nameUI.Show(skill.Name);

        // 使う方向を向く
        var dir = targetCell - GetLocation();
        SetDirection(dir.x, dir.y);

        // 少し待機
        if (CharacterType == CharaType.Player)
        {
            yield return new WaitForSeconds(0.3f);
        }
        else
        {
            // CPUは選択UI表示
            var ui = system.cellUI;
            var cellList = BossGameSystemB.CreateEnableCellList(skillID, this);
            var cursor = new BossGameBUICellSelect.CellUIParam(targetCell, skill.EffectRange * 2 + 1, skill.EffectRange * 2 + 1, false);
            ui.Show(cellList, new List<BossGameBUICellSelect.CellUIParam>() { cursor });
            yield return new WaitForSeconds(1.0f);
            ui.Hide();
        }

        // 使用
        // 敵味方使えるスキルの場合ここで実装
        var generalSkills = new[]
        {
            BossGameBDataBase.SkillID.Ami1,
            BossGameBDataBase.SkillID.Mana1,
            BossGameBDataBase.SkillID.Matuka1,
            BossGameBDataBase.SkillID.Mati1,
            BossGameBDataBase.SkillID.Menderu1,
            BossGameBDataBase.SkillID.Pierre1,
        };
        if (generalSkills.Contains(skillID))
        {
            yield return UseGeneralSkill(skillID, targetCell);
        }
        else
        {
            yield return UseSkill(skillID, targetCell);
        }

        // スキル名非表示
        nameUI.Hide();

        // 立ち絵リセット
        ResetDirection();
    }

    /// <summary>
    /// スキル使用本体
    /// </summary>
    /// <param name="skillID"></param>
    /// <param name="targetCell"></param>
    /// <returns></returns>
    protected virtual IEnumerator UseSkill(BossGameBDataBase.SkillID skillID, Vector2Int targetCell)
    {
        yield return null;
    }

    /// <summary>
    /// 敵味方共通スキル
    /// </summary>
    /// <returns></returns>
    protected IEnumerator UseGeneralSkill(BossGameBDataBase.SkillID skillID, Vector2Int targetCell)
    {
        var skill = BossGameBDataBase.SkillList[skillID];
        var sound = ManagerSceneScript.GetInstance().soundMan;
        yield return null;

        var targetList = GetSkillHitCharacters(skillID, targetCell);
        var cellList = BossGameSystemB.CreateSkillEffectCellList(skillID, targetCell);

        switch (skillID)
        {
            case BossGameBDataBase.SkillID.Ami1: // はるのとなり
                sound.PlaySE(system.dataObj.se_skill_harunotonari);
                yield return system.PlayHealEffect(location, 3, 3);
                // 回復
                yield return AttackDamage(targetList, -skill.Value);
                // 力アップ
                yield return system.BuffAttack(targetList, 1.1f);
                // 速度アップ
                yield return system.BuffSpeed(targetList, 1.1f);
                yield return new WaitForSeconds(0.5f);
                break;
            case BossGameBDataBase.SkillID.Mana1: //ショウダウン
                {
                    // 役を決定
                    var rand = Util.RandomInt(0, 99);
                    var yaku = BossGameBCardEffect.Yaku.Boo;
                    if (rand < 15) yaku = BossGameBCardEffect.Yaku.Boo;
                    else if (rand < 35) yaku = BossGameBCardEffect.Yaku.OnePair;
                    else if (rand < 51) yaku = BossGameBCardEffect.Yaku.TwoPair;
                    else if (rand < 64) yaku = BossGameBCardEffect.Yaku.ThreeCard;
                    else if (rand < 74) yaku = BossGameBCardEffect.Yaku.Straight;
                    else if (rand < 84) yaku = BossGameBCardEffect.Yaku.Flash;
                    else if (rand < 92) yaku = BossGameBCardEffect.Yaku.FullHouse;
                    else if (rand < 97) yaku = BossGameBCardEffect.Yaku.FourCard;
                    else if (rand < 99) yaku = BossGameBCardEffect.Yaku.StraightFlash;
                    else yaku = BossGameBCardEffect.Yaku.Loyal;

                    var cardParams = BossGameBCardEffect.DecideCard(yaku);
                    for (var i = 0; i < cardParams.Count; ++i)
                    {
                        sound.PlaySE(system.dataObj.se_skill_showdown_init);
                        system.CreateCardEffect(location, cardParams[i], i);
                        yield return new WaitForSeconds(BossGameBCardEffect.DELAY_ONE);
                    }
                    yield return new WaitForSeconds(1f);

                    //SE
                    sound.PlaySE(yaku == BossGameBCardEffect.Yaku.Boo ? system.dataObj.se_skill_showdown_fail : system.dataObj.se_skill_showdown_heal);
                    //エフェクト再生
                    yield return system.PlayHealEffect(new Vector2Int(BossGameSystemB.CELL_X_COUNT, BossGameSystemB.CELL_Y_COUNT) / 2, BossGameSystemB.CELL_X_COUNT, BossGameSystemB.CELL_Y_COUNT);
                    // 効果発動
                    switch (yaku)
                    {
                        case BossGameBCardEffect.Yaku.Loyal:
                            yield return AttackDamage(targetList, -1500);
                            yield return system.BuffAttack(targetList, 2f);
                            yield return system.BuffSpeed(targetList, 2f);
                            break;
                        case BossGameBCardEffect.Yaku.StraightFlash:
                            yield return AttackDamage(targetList, -600);
                            yield return system.BuffAttack(targetList, 1.5f);
                            yield return system.BuffSpeed(targetList, 1.5f);
                            break;
                        case BossGameBCardEffect.Yaku.FourCard:
                            yield return AttackDamage(targetList, -600);
                            yield return system.BuffSpeed(targetList, 1.5f);
                            break;
                        case BossGameBCardEffect.Yaku.FullHouse:
                            yield return AttackDamage(targetList, -600);
                            yield return system.BuffAttack(targetList, 1.5f);
                            break;
                        case BossGameBCardEffect.Yaku.Flash:
                            yield return AttackDamage(targetList, -200);
                            yield return system.BuffAttack(targetList, 1.2f);
                            yield return system.BuffSpeed(targetList, 1.2f);
                            break;
                        case BossGameBCardEffect.Yaku.Straight:
                            yield return system.BuffSpeed(targetList, 1.2f);
                            break;
                        case BossGameBCardEffect.Yaku.ThreeCard:
                            yield return system.BuffAttack(targetList, 1.2f);
                            break;
                        case BossGameBCardEffect.Yaku.TwoPair:
                            yield return AttackDamage(targetList, -600);
                            break;
                        case BossGameBCardEffect.Yaku.OnePair:
                            yield return AttackDamage(targetList, -200);
                            break;
                        case BossGameBCardEffect.Yaku.Boo:
                            yield return system.BuffAttack(targetList, 0.9f);
                            yield return system.BuffSpeed(targetList, 0.9f);
                            break;
                    }
                    yield return new WaitForSeconds(0.5f);
                }
                break;
            case BossGameBDataBase.SkillID.Mati1: // 刹那の見斬り
                {
                    var dist = targetCell - location;
                    yield return EffectMove(targetCell + dist, 0.03f);
                    system.CreateSlashEffect(targetCell, BossGameSystemB.GetCellPosition(dist));
                    sound.PlaySE(system.dataObj.se_skill_setuna);
                    yield return new WaitForSeconds(0.7f);
                    yield return EffectMove(location, 0.5f, 270f);
                    yield return AttackDamage(targetList, skill.Value);
                }
                break;
            case BossGameBDataBase.SkillID.Matuka1: // 喝
                {
                    ManagerSceneScript.GetInstance().mainCam.PlayShakeOne(Shaker.ShakeSize.Weak);
                    sound.PlayVoice(system.dataObj.se_skill_katu);
                    yield return new WaitForSeconds(1f);
                    targetList.Remove(this);
                    yield return AttackDamage(targetList, skill.Value, false);
                    foreach (var chara in targetList)
                    {
                        // 確率で行動時間増加
                        if (chara.CharacterType == CharacterType)
                        {
                            // 味方は低確率
                            if (Util.RandomCheck(20)) chara.DelayTime(1);
                        }
                        else
                        {
                            if (Util.RandomCheck(70)) chara.DelayTime(1);
                        }
                    }
                }
                break;
            case BossGameBDataBase.SkillID.Menderu1: // マントラップヴァイン
                {
                    // エフェクト
                    var basePos = BossGameSystemB.GetCellPosition(targetCell);
                    basePos.y += 30f;
                    var ofsX = BossGameSystemB.CELL_WIDTH * 0.75f;
                    var ofsY = BossGameSystemB.CELL_HEIGHT * 0.7f;
                    var base1 = basePos + new Vector3(ofsX, ofsY);
                    var base2 = basePos + new Vector3(-ofsX, -ofsY);
                    var base3 = basePos + new Vector3(-ofsX, ofsY);
                    var base4 = basePos + new Vector3(ofsX, -ofsY);

                    for (var i = 0; i < 5; ++i)
                    {
                        sound.PlaySE(system.dataObj.se_skill_mantrap);
                        var rand = new Vector3(Util.RandomFloat(-ofsX, ofsX), Util.RandomFloat(-ofsY, ofsY));
                        system.CreateGeneralEffect(base1 + rand, BossGameBDataObject.EffectKind.Mantrap);
                        yield return new WaitForSeconds(0.03f);
                        rand = new Vector3(Util.RandomFloat(-ofsX, ofsX), Util.RandomFloat(-ofsY, ofsY));
                        system.CreateGeneralEffect(base2 + rand, BossGameBDataObject.EffectKind.Mantrap);
                        yield return new WaitForSeconds(0.03f);
                        sound.PlaySE(system.dataObj.se_skill_mantrap);
                        rand = new Vector3(Util.RandomFloat(-ofsX, ofsX), Util.RandomFloat(-ofsY, ofsY));
                        system.CreateGeneralEffect(base3 + rand, BossGameBDataObject.EffectKind.Mantrap);
                        yield return new WaitForSeconds(0.03f);
                        rand = new Vector3(Util.RandomFloat(-ofsX, ofsX), Util.RandomFloat(-ofsY, ofsY));
                        system.CreateGeneralEffect(base4 + rand, BossGameBDataObject.EffectKind.Mantrap);
                        yield return new WaitForSeconds(0.03f);
                    }
                    yield return new WaitForSeconds(0.4f);

                    // 地形生成
                    foreach (var cell in cellList)
                    {
                        if (Util.RandomCheck(10)) continue;
                        system.SetFieldEffect(cell, BossGameBDataObject.FieldEffect.Mantrap, Util.RandomInt(60, 170));
                    }

                    // ダメージ
                    yield return AttackDamage(targetList, skill.Value, false);
                }
                break;
            case BossGameBDataBase.SkillID.Pierre1: // ジャグリングヒット
                {
                    var oldDir = nowDirection;
                    var p1 = new Vector3(40f, 20f);
                    var p2 = new Vector3(-40f, 20f);
                    var dmg = 0;
                    for (var i = 0; i < 5; ++i)
                    {
                        yield return new WaitForSeconds(0.05f);
                        SetDirection(1, 0);
                        if (Util.RandomCheck(50))
                        {
                            dmg += 30;
                            system.CreateJuggleEffect(location, p1, p2, targetCell, 0);
                        }
                        else
                        {
                            dmg += 110;
                            system.CreateJuggleEffect(location, p1, p2, targetCell, 1);
                        }

                        yield return new WaitForSeconds(0.05f);
                        SetDirection(-1, 0);
                        if (Util.RandomCheck(50))
                        {
                            dmg += 30;
                            system.CreateJuggleEffect(location, p2, p1, targetCell, 0);
                        }
                        else
                        {
                            dmg += 110;
                            system.CreateJuggleEffect(location, p2, p1, targetCell, 1);
                        }
                    }
                    SetDirection(oldDir);
                    yield return new WaitForSeconds(BossGameBJuggleEffect.JUGGLE_TIME + 0.5f);
                    yield return AttackDamage(targetList, dmg);
                }
                break;
        }
    }

    /// <summary>
    /// ダメージうける
    /// </summary>
    /// <param name="dmg"></param>
    /// <param name="safe">true: １残る</param>
    /// <returns></returns>
    public IEnumerator HitDamage(int dmg, bool safe = false)
    {
        damageEffecting = true;
        if (isInvincible) dmg = 0;
        else if (dmg > 9999) dmg = 9999;
        var basePos = BossGameSystemB.GetCellPosition(location);

        for (var i = 0; i < 12; ++i)
        {
            var addX = (1f - (Mathf.Pow(i, 2) / 144f)) * 10f;
            transform.localPosition = basePos + new Vector3(addX, 0);
            yield return new WaitForSeconds(0.03f);
            transform.localPosition = basePos + new Vector3(-addX, 0);
            yield return new WaitForSeconds(0.03f);
        }
        transform.localPosition = basePos;

        // ダメージUI表示
        yield return system.ShowDamage(location, dmg);

        param_HP -= dmg;
        if (param_HP <= 0)
        {
            if (safe)
            {
                param_HP = 1;
            }
            else
            {
                // 死亡演出して消去
                param_HP = 0;
                yield return DeadEffect();
                system.RemoveCharacter(this);
            }
        }

        damageEffecting = false;
    }

    /// <summary>
    /// ダメージ表示中
    /// </summary>
    /// <returns></returns>
    public bool IsDamageEffecting()
    {
        return damageEffecting;
    }

    /// <summary>
    /// 死亡演出
    /// </summary>
    protected virtual IEnumerator DeadEffect()
    {
        for (var i = 0; i < 14; ++i)
        {
            model.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.03f);
            model.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.03f);
        }

        model.gameObject.SetActive(false);
    }

    /// <summary>
    /// 回復する
    /// </summary>
    /// <param name="heal"></param>
    /// <returns></returns>
    public IEnumerator HealDamage(int heal)
    {
        damageEffecting = true;

        // 回復数字表示
        yield return system.ShowDamage(location, -heal);

        param_HP += heal;
        if (param_HP > param_HP_max) param_HP = param_HP_max;

        damageEffecting = false;
    }

    /// <summary>
    /// ダメージを与える キャラごとに乱数で上下
    /// </summary>
    /// <param name="targets"></param>
    /// <param name="baseDamage">マイナスで回復</param>
    /// <returns></returns>
    protected IEnumerator AttackDamage(List<BossGameBCharacterBase> targets, int baseDamage, bool useRate = true)
    {
        if (targets.Count == 0)
        {
            yield return new WaitForSeconds(0.5f);
            yield break;
        }

        // 攻撃のバフ倍率
        if (useRate && baseDamage > 0) baseDamage = Mathf.FloorToInt(baseDamage * param_ATK_rate);

        foreach (var t in targets)
        {
            var dmg = Util.RandomInt(Mathf.FloorToInt(baseDamage * 0.8f), baseDamage);
            if (dmg >= 0)
            {
                // フレンドリーファイアは軽減
                if (t.CharacterType == this.CharacterType) dmg = Mathf.FloorToInt(dmg * 0.4f);
                StartCoroutine(t.HitDamage(dmg));
            }
            else
                StartCoroutine(t.HealDamage(-dmg));
        }

        // 再生待ち
        foreach (var t in targets)
        {
            yield return new WaitWhile(() => t.IsDamageEffecting());
        }
    }

    #endregion
}

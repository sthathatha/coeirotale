using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

/// <summary>
/// ラスボス本戦　プレイヤー
/// </summary>
public class BossGameBPlayer : BossGameBCharacterBase
{
    #region 定数

    /// <summary>時間経過なしで歩ける歩数</summary>
    private const int WALK_DELAY_COUNT = 3;

    #endregion

    #region メンバー

    /// <summary>サイクロトロンSE</summary>
    public AudioClip se_cycrotron1;
    /// <summary>オクトストライクSE</summary>
    public AudioClip se_octstrike1;
    /// <summary>イーグルダイブSE</summary>
    public AudioClip se_eagleDive1;
    /// <summary>インビンシブルSE</summary>
    public AudioClip se_invincible1;
    /// <summary>リインカーネーションSE</summary>
    public AudioClip se_reincarnation1;
    /// <summary>オリジンSE</summary>
    public AudioClip se_origin1;
    /// <summary>イグニッションSE</summary>
    public AudioClip se_ignition1;
    /// <summary>キーニングシルフSE</summary>
    public AudioClip se_keeningsylph1;

    #endregion

    #region 変数

    /// <summary>歩いた歩数</summary>
    private int walkCount = 0;

    /// <summary>キャンセル中</summary>
    private int resetSelect = 0;

    #endregion

    #region 基底

    /// <summary>
    /// 初期化
    /// </summary>
    protected override void Start()
    {
        base.Start();

        skillList.Add(BossGameBDataBase.SkillID.Reko1_C);
        skillList.Add(BossGameBDataBase.SkillID.Reko2_O);
        skillList.Add(BossGameBDataBase.SkillID.Reko3_E);
        skillList.Add(BossGameBDataBase.SkillID.Reko4_I);
        skillList.Add(BossGameBDataBase.SkillID.Reko5_R);
        skillList.Add(BossGameBDataBase.SkillID.Reko6_O);
        skillList.Add(BossGameBDataBase.SkillID.Reko7_I);
        skillList.Add(BossGameBDataBase.SkillID.Reko8_N);
        skillList.Add(BossGameBDataBase.SkillID.Reko9_K);

        // 連戦結果でスキル追加
        var g = Global.GetTemporaryData();
        if (g.bossRushAmiWon) skillList.Add(BossGameBDataBase.SkillID.Ami1);
        if (g.bossRushManaWon) skillList.Add(BossGameBDataBase.SkillID.Mana1);
        if (g.bossRushMatiWon) skillList.Add(BossGameBDataBase.SkillID.Mati1);
        if (g.bossRushMenderuWon) skillList.Add(BossGameBDataBase.SkillID.Menderu1);
        if (g.bossRushMatukaWon) skillList.Add(BossGameBDataBase.SkillID.Matuka1);
        if (g.bossRushPierreWon) skillList.Add(BossGameBDataBase.SkillID.Pierre1);

        CharacterType = CharaType.Player;
    }

    /// <summary>
    /// キャラID
    /// </summary>
    /// <returns></returns>
    public override BossGameSystemB.CharacterID GetCharacterID()
    {
        return BossGameSystemB.CharacterID.Player;
    }

    /// <summary>
    /// ターン行動
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator TurnProcess()
    {
        var input = InputManager.GetInstance();
        var command = system.commandUI;
        var cells = system.cellUI;
        var sound = ManagerSceneScript.GetInstance().soundMan;

        while (true)
        {
            yield return null;

            if (input.GetKeyPress(InputManager.Keys.South) || resetSelect == 1)
            {
                // メニューを開いて処理
                if (resetSelect != 1) sound.PlaySE(sound.commonSeWindowOpen);
                command.SkillList = skillList;
                yield return command.Open(resetSelect != 1);
                if (command.Result == BossGameBUICommand.CommandResult.Cancel)
                {
                    sound.PlaySE(sound.commonSeError);
                    resetSelect = 0;
                    continue;
                }
                sound.PlaySE(sound.commonSeSelect);

                // 選択したスキルの対象選択UI
                yield return cells.ShowSelect(command.SelectSkill, this);
                if (cells.Result == BossGameBUICellSelect.CellSelectResult.Cancel)
                {
                    sound.PlaySE(sound.commonSeError);
                    resetSelect = 1;
                    continue;
                }
                resetSelect = 0;

                // スキル使用
                yield return UseSkillBase(command.SelectSkill, cells.SelectCell);

                system.playerWalkReset = true;
                break;
            }
            else if (input.GetKeyPress(InputManager.Keys.East))
            {
                // キャンセルボタンでHP表示
                yield return system.ShowHp();
            }
            else
            {
                var walkLoc = new Vector2Int(0, 0);
                if (input.GetKey(InputManager.Keys.Up))
                    walkLoc.y = 1;
                else if (input.GetKey(InputManager.Keys.Down))
                    walkLoc.y = -1;
                else if (input.GetKey(InputManager.Keys.Left))
                    walkLoc.x = -1;
                else if (input.GetKey(InputManager.Keys.Right))
                    walkLoc.x = 1;

                if (walkLoc.x != 0 || walkLoc.y != 0)
                {
                    if (system.CanWalk(GetLocation() + walkLoc))
                    {
                        yield return Walk(walkLoc.x, walkLoc.y);

                        // 地形効果チェック
                        var effect = system.GetCellFieldEffect(location);
                        if (effect == BossGameBDataObject.FieldEffect.Mantrap)
                        {
                            // マントラップに入ったら強制終了
                            sound.PlaySE(system.dataObj.se_skill_mantrap);
                            system.ClearFieldEffect(location);
                            yield return new WaitForSeconds(0.4f);
                            system.playerWalkReset = true;
                            break;
                        }
                        else if (effect == BossGameBDataObject.FieldEffect.Plasma)
                        {
                            // ダメージうける
                            sound.PlaySE(system.dataObj.se_field_plasma);
                            yield return HitDamage(Util.RandomInt(160, 240), true);
                            system.ClearFieldEffect(location);
                        }

                        if (WalkEndCheck() == false) break;
                    }
                    else
                    {
                        // 移動できない場所は向きを変えるだけ
                        SetDirection(walkLoc.x, walkLoc.y);
                    }
                }
            }

            resetSelect = 0;
        }
    }

    /// <summary>
    /// スキル実行
    /// </summary>
    /// <param name="skillID"></param>
    /// <param name="targetCell"></param>
    /// <returns></returns>
    protected override IEnumerator UseSkill(BossGameBDataBase.SkillID skillID, Vector2Int targetCell)
    {
        var skill = BossGameBDataBase.SkillList[skillID];
        var sound = ManagerSceneScript.GetInstance().soundMan;
        yield return null;

        var targetList = GetSkillHitCharacters(skillID, targetCell);

        switch (skillID)
        {
            case BossGameBDataBase.SkillID.Reko1_C: // サイクロトロン
                sound.PlaySE(se_cycrotron1);
                // 回転
                for (var i = 0; i < 16; ++i)
                {
                    model.sprite = sp_leftup;
                    yield return new WaitForSeconds(0.04f);
                    model.sprite = sp_rightup;
                    yield return new WaitForSeconds(0.04f);
                }
                ResetDirection();
                // 自分の速度アップ
                yield return system.BuffSpeed(targetList, 1.2f);
                yield return new WaitForSeconds(0.5f);
                break;
            case BossGameBDataBase.SkillID.Reko2_O: // オクトストライク
                yield return EffectMove(targetCell, 0.2f);
                var targetCellPos = BossGameSystemB.GetCellPosition(targetCell);
                for (var i = 0; i < 8; ++i)
                {
                    var randPos = new Vector3(Util.RandomFloat(-40f, 40f), Util.RandomFloat(-40f, 40f));
                    system.CreateGeneralEffect(targetCellPos + randPos, BossGameBDataObject.EffectKind.Hit);
                    sound.PlaySE(se_octstrike1);
                    model.sprite = sp_leftup;
                    yield return new WaitForSeconds(0.04f);
                    model.sprite = sp_rightup;
                    yield return new WaitForSeconds(0.04f);
                    model.sprite = sp_leftup;
                    yield return new WaitForSeconds(0.04f);
                    model.sprite = sp_rightup;
                    yield return new WaitForSeconds(0.04f);
                }
                ResetDirection();
                yield return EffectMove(location, 0.4f);
                yield return AttackDamage(targetList, skill.Value);
                break;
            case BossGameBDataBase.SkillID.Reko3_E: // イーグルダイブ
                yield return EffectMove(targetCell, 0.6f, 400f);
                sound.PlaySE(se_eagleDive1);
                system.CreateSlashEffect(targetCell, new Vector3(0, -1));
                yield return new WaitForSeconds(0.4f);
                yield return EffectMove(location, 0.6f, 300f);
                yield return AttackDamage(targetList, skill.Value);
                break;
            case BossGameBDataBase.SkillID.Reko4_I: // インビンシブル
                system.CreateGeneralEffect(BossGameSystemB.GetCellPosition(location), BossGameBDataObject.EffectKind.Invincible);
                yield return new WaitForSeconds(0.14f);
                sound.PlaySE(se_invincible1);
                yield return new WaitForSeconds(0.8f);
                isInvincible = true;
                break;
            case BossGameBDataBase.SkillID.Reko5_R: // リインカーネーション
                sound.PlaySE(se_reincarnation1);
                yield return system.PlayHealEffect(location, 1, 1);
                yield return HealDamage(param_HP_max);
                break;
            case BossGameBDataBase.SkillID.Reko6_O: // オリジン
                sound.PlaySE(se_origin1);
                system.CreateOriginEffect(location);
                yield return new WaitForSeconds(0.8f);
                foreach (var chara in targetList)
                {
                    chara.ResetParam();
                }
                system.ClearFieldEffect();
                break;
            case BossGameBDataBase.SkillID.Reko7_I: // イグニッション
                system.CreateGeneralEffect(BossGameSystemB.GetCellPosition(location), BossGameBDataObject.EffectKind.Ignition);
                yield return new WaitForSeconds(0.2f);
                sound.PlaySE(se_ignition1);
                yield return new WaitForSeconds(0.6f);
                yield return system.BuffAttack(targetList, 1.2f);
                yield return new WaitForSeconds(0.5f);
                break;
            case BossGameBDataBase.SkillID.Reko8_N: // ナイトメア
                {
                    system.PlayHorrorEffect(targetCell);
                    yield return new WaitForSeconds(2.6f);
                    yield return AttackDamage(targetList, skill.Value, false);
                    var slowChara = new List<BossGameBCharacterBase>();
                    foreach (var chara in targetList)
                    {
                        // 確率で行動時間増加
                        if (Util.RandomCheck(50)) chara.DelayTime(1);

                        // 確率で速度ダウン
                        if (Util.RandomCheck(50)) slowChara.Add(chara);
                    }
                    yield return system.BuffSpeed(slowChara, 0.9f);
                }
                break;
            case BossGameBDataBase.SkillID.Reko9_K: // キーニングシルフ
                var center = BossGameSystemB.GetCellPosition(location);
                var ofsX = BossGameSystemB.CELL_WIDTH;
                var ofsY = BossGameSystemB.CELL_HEIGHT;
                var cs = new[] {
                    center + new Vector3(ofsX, ofsY, 0),
                    center + new Vector3(-ofsX, -ofsY, 0),
                    center + new Vector3(-ofsX, ofsY, 0),
                    center + new Vector3(ofsX, -ofsY, 0)
                };
                // 回転
                for (var i = 0; i < 16; ++i)
                {
                    if (i % 2 == 0)
                    {
                        sound.PlaySE(se_keeningsylph1);
                    }
                    var effPos = cs[i % 4] + new Vector3(Util.RandomFloat(-ofsX, ofsX), Util.RandomFloat(-ofsY, ofsY));
                    system.CreateGeneralEffect(effPos, BossGameBDataObject.EffectKind.Cycrone);
                    model.sprite = sp_leftup;
                    yield return new WaitForSeconds(0.04f);
                    model.sprite = sp_rightup;
                    yield return new WaitForSeconds(0.04f);
                }
                ResetDirection();
                yield return AttackDamage(targetList, skill.Value, false);
                break;
        }
    }

    #endregion

    #region 機能

    /// <summary>
    /// 時間リセット
    /// </summary>
    /// <param name="init"></param>
    public override void ResetTime(bool init = false)
    {
        if (walkCount > 0)
        {
            // 歩いた時の時間
            param_wait_time = GetMaxWaitTime() / 3;
        }
        else if (init)
        {
            // プレイヤーの初期時間は少ない
            param_wait_time = GetMaxWaitTime() / 2;
        }
        else
        {
            base.ResetTime();
        }
    }

    /// <summary>
    /// 歩行終了時の時間経過判定
    /// </summary>
    /// <returns></returns>
    private bool WalkEndCheck()
    {
        ++walkCount;
        // ３歩までは時間たたない
        if (walkCount <= WALK_DELAY_COUNT) return true;

        var walkWait = GetMaxWaitTime() / 3;
        if (system.CanWalkWait(this, walkWait))
        {
            // まだ他が動かないなら減らして続行
            system.DecreaseAllCharacterWait(this, walkWait);
            return true;
        }

        return false;
    }

    public void ResetWalkCount()
    {
        walkCount = 0;
    }

    #endregion
}

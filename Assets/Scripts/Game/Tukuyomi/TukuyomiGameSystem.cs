using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

/// <summary>
/// つくよみちゃん戦
/// </summary>
public class TukuyomiGameSystem : GameSceneScriptBase
{
    #region 定数

    private bool DEBUG = true;

    /// <summary></summary>
    public const string START_FLG = "TukuGameShow";

    /// <summary>
    /// コマの種類
    /// </summary>
    public enum Koma : int
    {
        None = 0,
        Ou,
        Hisya,
        Kaku,
        Kin,
        Gin,
        Kei,
        Kyou,
        Hu,
    }

    /// <summary>コマ選択時の位置</summary>
    private const float KOMA_SELECT_INT = 160f;
    private const float KOMA_X_KEI = 0f;
    private const float KOMA_X_GIN = KOMA_X_KEI - KOMA_SELECT_INT;
    private const float KOMA_X_KIN = KOMA_X_GIN - KOMA_SELECT_INT;
    private const float KOMA_X_KYOU = KOMA_X_KEI + KOMA_SELECT_INT;
    private const float KOMA_X_HU = KOMA_X_KYOU + KOMA_SELECT_INT;
    private const float KOMA_X_HISYA = 200f;
    private const float KOMA_X_KAKU = -200f;

    /// <summary>コマ選択時のつくよみちゃん位置</summary>
    private const float KOMA_SELECT_TUKUYOMI_POS = -500f;

    /// <summary>フィールドのY位置　基本</summary>
    private const float AFIELD_Y_DEFAULT = -172f;
    /// <summary>フィールドのY位置　香車</summary>
    private const float AFIELD_Y_KYOU = -270f;

    #endregion

    #region メンバー

    public AudioClip bgm_scene1;
    public TukuyomiGameResource resource;

    public TukuyomiGameTukuyomiA tukuyomiA;
    public TukuyomiGamePlayer reko;

    public TukuyomiGamePlayField playField;

    public TukuyomiGameKomaBig koma_ou;
    public TukuyomiGameKomaBig koma_hisya;
    public TukuyomiGameKomaBig koma_kaku;
    public TukuyomiGameKomaBig koma_kin;
    public TukuyomiGameKomaBig koma_gin;
    public TukuyomiGameKomaBig koma_kei;
    public TukuyomiGameKomaBig koma_kyou;
    public TukuyomiGameKomaBig koma_hu;
    public TukuyomiGameKomaSmall small_dummy;
    public TukuyomiGameAttackEffect attack_dummy;
    public TukuyomiGameShot shot_dummy;

    public Transform objectParent;

    public TukuyomiGameMessageUI serifUI;
    public TukuyomiGameMessageUI systemUI;
    public TukuyomiGameLifeUI lifeUI;
    public GameObject pressUI;

    #endregion

    #region 変数

    private bool flg_beat_kin;
    private bool flg_beat_gin;
    private bool flg_beat_kei;
    private bool flg_beat_kyou;
    private bool flg_beat_hu;
    private int NokoriKoma
    {
        get
        {
            var ret = 0;
            if (!flg_beat_kin) ++ret;
            if (!flg_beat_gin) ++ret;
            if (!flg_beat_kei) ++ret;
            if (!flg_beat_kyou) ++ret;
            if (!flg_beat_hu) ++ret;
            return ret;
        }
    }

    private int reko_life = 5;

    /// <summary>ダメージで一旦消える状態</summary>
    private bool reko_damage_hide_mode = false;

    #endregion

    #region 基底

    /// <summary>
    /// フェードイン直前
    /// </summary>
    /// <returns></returns>
    public override IEnumerator Start()
    {
        yield return base.Start();

        koma_ou.Show(false);
        koma_hisya.Show(false);
        koma_kaku.Show(false);
        koma_kin.Show(false);
        koma_gin.Show(false);
        koma_kei.Show(false);
        koma_kyou.Show(false);
        koma_hu.Show(false);

        small_dummy.gameObject.SetActive(false);
        attack_dummy.gameObject.SetActive(false);
        shot_dummy.gameObject.SetActive(false);

        flg_beat_kin = false;
        flg_beat_gin = false;
        flg_beat_kei = false;
        flg_beat_kyou = false;
        flg_beat_hu = false;

        serifUI.Hide();
        systemUI.Hide();
        lifeUI.Hide();
        pressUI.SetActive(false);

        // 開始時の状態
        tukuyomiA.PlayAnim(TukuyomiGameTukuyomiA.AnimType.Before);
        ADefaultField();
    }

    /// <summary>
    /// フェードイン後
    /// </summary>
    /// <returns></returns>
    public override IEnumerator AfterFadeIn()
    {
        yield return base.AfterFadeIn();
        StartCoroutine(MainACoroutine());
    }

    #endregion

    #region ダメージ処理

    /// <summary>
    /// プレイヤーのダメージ
    /// </summary>
    public void PlayerDamage()
    {
        resource.PlaySE(resource.se_player_damage);
        reko_life--;
        lifeUI.ShowLife(reko_life);

        if (reko_life > 0)
        {
            if (reko_damage_hide_mode) reko.gameObject.SetActive(false);
            return;
        }

        // 0になったらゲーム終了
        reko.gameObject.SetActive(false);
        StartCoroutine(LoseExitCoroutine());
    }

    /// <summary>
    /// 敗北終了
    /// </summary>
    /// <returns></returns>
    private IEnumerator LoseExitCoroutine()
    {
        SetGameResult(false);
        yield return new WaitForSeconds(2f);
        ManagerSceneScript.GetInstance().ExitGame();
    }

    #endregion

    #region ショット管理

    private List<TukuyomiGameShot> shotList = new List<TukuyomiGameShot>();

    /// <summary>
    /// ショット作成
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="vec"></param>
    /// <param name="type"></param>
    public void CreateShot(Vector3 pos, Vector3 vec, TukuyomiGameShot.ShotType type)
    {
        var shot = Instantiate(shot_dummy, playField.transform);
        shotList.Add(shot);

        if (type == TukuyomiGameShot.ShotType.Enemy)
        {
        }
        shot.Shoot(pos, vec, type);
    }

    /// <summary>
    /// リストからショットを削除
    /// </summary>
    /// <param name="s"></param>
    public void ShotRemove(TukuyomiGameShot s)
    {
        shotList.Remove(s);
    }

    #endregion

    #region 前半

    /// <summary>
    /// 待機中の表示
    /// </summary>
    private void ADefaultField()
    {
        playField.Show(240f, 240f);
        reko.moveMode = TukuyomiGamePlayer.MoveMode.Free;
        reko.gameObject.SetActive(true);
        reko.transform.localPosition = Vector3.zero;
    }

    private Koma _ASelectKoma;

    /// <summary>
    /// メイン進行
    /// </summary>
    /// <returns></returns>
    private IEnumerator MainACoroutine()
    {
        var manager = ManagerSceneScript.GetInstance();

        // 開始会話
        yield return AStartCoroutine();
        if (reko_life <= 0) yield break;

        while (true)
        {
            // コマ選択
            yield return APlayerSelectCoroutine();
            reko_damage_hide_mode = true;
            if (_ASelectKoma == Koma.Kin)
            {
                yield return AKinCoroutine();
                if (reko_life <= 0) yield break;
                ADefaultField();
                if (flg_beat_kin) yield return APlaySerif();
            }
            else if (_ASelectKoma == Koma.Gin)
            {
                yield return AGinCoroutine();
                if (reko_life <= 0) yield break;
                ADefaultField();
                if (flg_beat_gin) yield return APlaySerif();
            }
            else if (_ASelectKoma == Koma.Kei)
            {
                yield return AKeiCoroutine();
                if (reko_life <= 0) yield break;
                ADefaultField();
                if (flg_beat_kei) yield return APlaySerif();
            }
            else if (_ASelectKoma == Koma.Kyou)
            {
                yield return AKyouCoroutine();
                if (reko_life <= 0) yield break;
                ADefaultField();
                if (flg_beat_kyou) yield return APlaySerif();
            }
            else if (_ASelectKoma == Koma.Hu)
            {
                yield return AHuCoroutine();
                if (reko_life <= 0) yield break;
                ADefaultField();
                if (flg_beat_hu) yield return APlaySerif();
            }
            reko_damage_hide_mode = false;

            if (NokoriKoma == 0)
            {
                break;
            }

            // つくよみちゃんの攻撃
            yield return ATukuTurnCoroutine(false);
            if (reko_life <= 0) yield break;
        }

        // 全部終わったら後半戦
        ManagerSceneScript.GetInstance().ExitGame();
    }

    /// <summary>
    /// セリフ再生
    /// </summary>
    /// <returns></returns>
    private IEnumerator APlaySerif()
    {
        if (DEBUG)
        {
            yield return new WaitForSeconds(1f);
            yield break;
        }
        if (NokoriKoma == 4)
        {
            serifUI.Show(StringMinigameMessage.Tuku_Game_1_1, resource.voice_game_1_1);
            yield return new WaitForSeconds(2.3f);
            serifUI.Show(StringMinigameMessage.Tuku_Game_1_2, resource.voice_game_1_2);
            yield return new WaitForSeconds(4.5f);
        }
        else if (NokoriKoma == 3)
        {
            serifUI.Show(StringMinigameMessage.Tuku_Game_2_1, resource.voice_game_2_1);
            yield return new WaitForSeconds(6.5f);
            serifUI.Show(StringMinigameMessage.Tuku_Game_2_2, resource.voice_game_2_2);
            yield return new WaitForSeconds(3f);
        }
        else if (NokoriKoma == 2)
        {
            serifUI.Show(StringMinigameMessage.Tuku_Game_3_1, resource.voice_game_3_1);
            yield return new WaitForSeconds(4f);
            serifUI.Show(StringMinigameMessage.Tuku_Game_3_2, resource.voice_game_3_2);
            yield return new WaitForSeconds(3f);
        }
        else if (NokoriKoma == 1)
        {
            serifUI.Show(StringMinigameMessage.Tuku_Game_4_1, resource.voice_game_4_1);
            yield return new WaitForSeconds(2f);
            serifUI.Show(StringMinigameMessage.Tuku_Game_4_2, resource.voice_game_4_2);
            yield return new WaitForSeconds(4f);
        }
        else if (NokoriKoma == 0)
        {
            serifUI.Show(StringMinigameMessage.Tuku_Game_5_1, resource.voice_game_5_1);
            yield return new WaitForSeconds(2f);
            serifUI.Show(StringMinigameMessage.Tuku_Game_5_2, resource.voice_game_5_2);
            yield return new WaitForSeconds(4f);
        }
        serifUI.Hide();
    }

    /// <summary>
    /// 開始時
    /// </summary>
    /// <returns></returns>
    private IEnumerator AStartCoroutine()
    {
        var manager = ManagerSceneScript.GetInstance();
        var save = Global.GetSaveData();

        // 会話から
        if (DEBUG)
        {
            yield return new WaitForSeconds(1f);
        }
        else
        {
            if (save.GetGameDataInt(START_FLG) != 1)
            {
                serifUI.Show(StringMinigameMessage.Tuku_Start_First_0, resource.voice_start_first_0);
                yield return new WaitForSeconds(2f);
                serifUI.Show(StringMinigameMessage.Tuku_Start_First_1, resource.voice_start_first_1);
                yield return new WaitForSeconds(5.5f);
                serifUI.Show(StringMinigameMessage.Tuku_Start_First_2, resource.voice_start_first_2);
                yield return new WaitForSeconds(4f);
            }
            else
            {
                serifUI.Show(StringMinigameMessage.Tuku_Start_Second, resource.voice_start_second);
                yield return new WaitForSeconds(2f);
            }
        }
        // 画面黒
        serifUI.Hide();
        manager.FadeOutNoWait();
        manager.soundMan.PlaySE(manager.soundMan.commonSeMove);
        yield return new WaitForSeconds(0.4f);

        // 攻撃前の画面設定して画面復帰
        playField.ShowCellField(5, 5);
        reko.moveMode = TukuyomiGamePlayer.MoveMode.Cell;
        reko.CellLocation = new Vector2(2, 2);
        tukuyomiA.PlayAnim(TukuyomiGameTukuyomiA.AnimType.Stop);
        reko_life = 5;
        lifeUI.ShowLife(reko_life);
        manager.FadeInNoWait();
        manager.soundMan.PlaySE(manager.soundMan.commonSeMove);
        yield return new WaitForSeconds(1f);

        // セリフ
        if (save.GetGameDataInt(START_FLG) != 1 && !DEBUG)
        {
            serifUI.Show(StringMinigameMessage.Tuku_Start_First_3, resource.voice_start_first_3);
            yield return new WaitForSeconds(3f);
            serifUI.Hide();
        }
        else
        {
            yield return new WaitForSeconds(1f);
        }

        // 攻撃
        yield return ATukuTurnCoroutine(true);

        // 攻撃後のセリフ
        if (DEBUG)
        {
            yield return new WaitForSeconds(1f);
        }
        else
        {
            if (save.GetGameDataInt(START_FLG) != 1)
            {
                serifUI.Show(StringMinigameMessage.Tuku_Start2_First_0, resource.voice_start2_first_0);
                yield return new WaitForSeconds(2.5f);
                serifUI.Show(StringMinigameMessage.Tuku_Start2_First_1, resource.voice_start2_first_1);
                yield return new WaitForSeconds(4f);
            }
            else
            {
                serifUI.Show(StringMinigameMessage.Tuku_Start2_Second, resource.voice_start2_second);
                yield return new WaitForSeconds(4f);
            }
            serifUI.Hide();
        }

        // フラグ設定して終了
        save.SetGameData(START_FLG, 1);
        ManagerSceneScript.GetInstance().soundMan.StartGameBgm(bgm_scene1);
        tukuyomiA.PlayAnim(TukuyomiGameTukuyomiA.AnimType.Idle);
    }

    /// <summary>
    /// つくよみちゃんの攻撃コルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator ATukuTurnCoroutine(bool firstAttack = false)
    {
        tukuyomiA.PlayAnim(TukuyomiGameTukuyomiA.AnimType.Down);
        koma_hisya.Appear(KOMA_X_HISYA);
        koma_kaku.Appear(KOMA_X_KAKU);

        if (!firstAttack)
        {
            // 5x5の中央に配置
            playField.ShowCellField(5, 5);
            reko.moveMode = TukuyomiGamePlayer.MoveMode.Cell;
            reko.CellLocation = new Vector2(2, 2);
            reko.gameObject.SetActive(true);
        }
        yield return new WaitForSeconds(1f);

        // 飛車と角をランダム生成
        var cnt = firstAttack ? 10 : 6;
        for (var i = 0; i < cnt; ++i)
        {
            StartCoroutine(AttackHisyaKaku(Util.RandomCheck(50)));

            yield return new WaitForSeconds(firstAttack ? 0.4f : 0.6f);
        }
        yield return new WaitForSeconds(1.5f);
        koma_hisya.Disappear();
        koma_kaku.Disappear();
    }

    /// <summary>
    /// 飛車と角の攻撃1個
    /// </summary>
    /// <returns></returns>
    private IEnumerator AttackHisyaKaku(bool hisya)
    {
        var small = Instantiate(small_dummy);
        small.transform.SetParent(playField.transform);
        small.transform.position = hisya ? koma_hisya.transform.position : koma_kaku.transform.position;
        small.SetKoma(hisya ? Koma.Hisya : Koma.Kaku);
        small.gameObject.SetActive(true);

        var p = new DeltaVector3();
        p.Set(small.transform.localPosition);

        // 打つ場所を選択
        var atkCell = new Vector2(Util.RandomInt(0, 4), Util.RandomInt(0, 4));
        p.MoveTo(playField.GetCellPosition(atkCell), 0.3f, DeltaFloat.MoveType.LINE);
        while (p.IsActive())
        {
            yield return null;
            p.Update(Time.deltaTime);
            small.transform.localPosition = p.Get();
        }
        resource.PlaySE(resource.se_koma_set);
        yield return new WaitForSeconds(0.5f);

        // 攻撃
        Destroy(small.gameObject);
        if (hisya)
        {
            // 飛車は縦横
            for (var i = 0; i <= 4; ++i)
            {
                var yoko = new Vector2(i, atkCell.y);
                var tate = new Vector2(atkCell.x, i);
                if (yoko != atkCell) CreateAttackCell(yoko);
                if (tate != atkCell) CreateAttackCell(tate);
            }
        }
        else
        {
            //角は斜め
            for (var i = 1; i <= 4; ++i)
            {
                var ru = atkCell + new Vector2(i, i);
                var rd = atkCell + new Vector2(i, -i);
                var lu = atkCell + new Vector2(-i, i);
                var ld = atkCell + new Vector2(-i, -i);
                if (playField.InField(ru)) CreateAttackCell(ru);
                if (playField.InField(rd)) CreateAttackCell(rd);
                if (playField.InField(lu)) CreateAttackCell(lu);
                if (playField.InField(ld)) CreateAttackCell(ld);
            }
        }
        // コマ自身の位置
        CreateAttackCell(atkCell);
        resource.PlaySE(resource.se_attack_effect_A);
    }

    /// <summary>
    /// セルに攻撃
    /// </summary>
    /// <param name="cell"></param>
    private void CreateAttackCell(Vector2 cell)
    {
        var atk = Instantiate(attack_dummy);
        atk.transform.SetParent(playField.transform);
        atk.Attack(playField.GetCellPosition(cell));
    }

    /// <summary>
    /// コマ選択コルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator APlayerSelectCoroutine()
    {
        _ASelectKoma = Koma.None;
        playField.Hide();
        reko.gameObject.SetActive(false);

        // つくよみちゃんが避け、コマが出てくる
        tukuyomiA.PlayAnim(TukuyomiGameTukuyomiA.AnimType.Idle);
        tukuyomiA.Move(new Vector3(KOMA_SELECT_TUKUYOMI_POS, tukuyomiA.transform.localPosition.y), 0.5f);
        var komaList = new List<Koma>();

        if (!flg_beat_kin) { koma_kin.Appear(KOMA_X_KIN); komaList.Add(Koma.Kin); }
        if (!flg_beat_gin) { koma_gin.Appear(KOMA_X_GIN); komaList.Add(Koma.Gin); }
        if (!flg_beat_kei) { koma_kei.Appear(KOMA_X_KEI); komaList.Add(Koma.Kei); }
        if (!flg_beat_kyou) { koma_kyou.Appear(KOMA_X_KYOU); komaList.Add(Koma.Kyou); }
        if (!flg_beat_hu) { koma_hu.Appear(KOMA_X_HU); komaList.Add(Koma.Hu); }
        yield return new WaitForSeconds(0.8f);

        // システムメッセージ
        systemUI.Show(NokoriKoma switch
        {
            5 => StringMinigameMessage.Tuku_System_1,
            4 => StringMinigameMessage.Tuku_System_2,
            3 => StringMinigameMessage.Tuku_System_3,
            2 => StringMinigameMessage.Tuku_System_4,
            _ => StringMinigameMessage.Tuku_System_5,
        });

        // カーソル
        var sel = 0;
        APlayerSelectShowCursor(komaList[sel]);

        var input = InputManager.GetInstance();
        while (true)
        {
            yield return null;

            if (input.GetKeyPress(InputManager.Keys.South)) break;

            if (input.GetKeyPress(InputManager.Keys.Right))
            {
                sel++;
                if (sel >= komaList.Count) sel = 0;
                APlayerSelectShowCursor(komaList[sel]);
            }
            else if (input.GetKeyPress(InputManager.Keys.Left))
            {
                sel--;
                if (sel < 0) sel = komaList.Count - 1;
                APlayerSelectShowCursor(komaList[sel]);
            }
        }

        // 決定
        systemUI.Hide();
        _ASelectKoma = komaList[sel];
        reko.gameObject.SetActive(false);

        // 選択しなかったコマが消える
        if (!flg_beat_kin && _ASelectKoma != Koma.Kin) koma_kin.Disappear();
        if (!flg_beat_gin && _ASelectKoma != Koma.Gin) koma_gin.Disappear();
        if (!flg_beat_kei && _ASelectKoma != Koma.Kei) koma_kei.Disappear();
        if (!flg_beat_kyou && _ASelectKoma != Koma.Kyou) koma_kyou.Disappear();
        if (!flg_beat_hu && _ASelectKoma != Koma.Hu) koma_hu.Disappear();
        // 選択したコマが中央に表示
        switch (_ASelectKoma)
        {
            case Koma.Kin: koma_kin.Move(Vector3.zero, 0.5f); break;
            case Koma.Gin: koma_gin.Move(Vector3.zero, 0.5f); break;
            case Koma.Kei: koma_kei.Move(Vector3.zero, 0.5f); break;
            case Koma.Kyou: koma_kyou.Move(Vector3.zero, 0.5f); break;
            case Koma.Hu: koma_hu.Move(Vector3.zero, 0.5f); break;
        }
        yield return new WaitForSeconds(0.5f);
    }

    /// <summary>
    /// カーソル表示
    /// </summary>
    /// <param name="koma"></param>
    private void APlayerSelectShowCursor(Koma koma)
    {
        const float cursor_Y = 80f;
        reko.gameObject.SetActive(true);
        reko.moveMode = TukuyomiGamePlayer.MoveMode.Disable;

        reko.transform.position = new Vector3(koma switch
        {
            Koma.Kin => KOMA_X_KIN,
            Koma.Gin => KOMA_X_GIN,
            Koma.Kei => KOMA_X_KEI,
            Koma.Kyou => KOMA_X_KYOU,
            _ => KOMA_X_HU
        }, cursor_Y);
    }

    #region 金

    /// <summary>
    /// 金のゴールマス
    /// </summary>
    public Vector2 AKinGoal { get; set; }
    /// <summary>金の戦闘中</summary>
    public bool AKinPlayMode { get; set; } = false;
    private bool leachKinGoal = false;

    /// <summary>
    /// プレイヤーが金のゴールに到達
    /// </summary>
    public void ALeachKinGoal()
    {
        AKinPlayMode = false;
        resource.PlaySE(resource.se_koma_damage);
        leachKinGoal = true;
    }

    /// <summary>
    /// 金
    /// </summary>
    /// <returns></returns>
    private IEnumerator AKinCoroutine()
    {
        // フィールド表示
        AKinPlayMode = true;
        playField.ShowCellField(7, 5);
        reko.moveMode = TukuyomiGamePlayer.MoveMode.Cell;
        reko.CellLocation = new Vector2(3, 0);
        reko.gameObject.SetActive(true);

        // ゴールマス
        AKinGoal = new Vector2(Util.RandomInt(2, 4), 4);
        playField.ShowTargetCell(AKinGoal);

        // 金を生成
        var kin = Instantiate(small_dummy, playField.transform);
        kin.SetKoma(Koma.Kin);
        var kinPos = new Vector2(3, 4);
        kin.transform.localPosition = playField.GetCellPosition(kinPos);
        kin.transform.localRotation = Util.GetRotateQuaternion(Mathf.PI);
        kin.gameObject.SetActive(true);
        kin.CellLocation = kinPos;
        enemyKomaList.Add(kin);

        // 動き出すまで待機
        yield return new WaitWhile(() => reko.CellLocation == new Vector2(3, 0));

        while (true)
        {
            // プレイヤーめがけて動く
            var moveVec = Vector2.zero;
            var dist = reko.CellLocation - kinPos;
            if (dist.x > 0 && dist.y < 0) moveVec = new Vector2(1, -1); // 右下
            else if (dist.x < 0 && dist.y < 0) moveVec = new Vector2(-1, -1); // 左下
            else if (dist.x == 0) moveVec.y = dist.y > 0 ? 1 : -1; //上、下
            else if (dist.y == 0) moveVec.x = dist.x > 0 ? 1 : -1; //右、左
            else
            {
                // 右上、左上は縦横で遠い側を選択
                if (Mathf.Abs(dist.x) >= Mathf.Abs(dist.y)) moveVec.x = dist.x > 0 ? 1 : -1;
                else moveVec.y = 1;
            }
            kinPos += moveVec;
            kin.CellLocation = kinPos;
            kin.transform.localPosition = playField.GetCellPosition(kinPos);
            resource.PlaySE(resource.se_koma_set);
            // そこにプレイヤーが居たらダメージ
            if (reko.CellLocation == kinPos)
            {
                PlayerDamage();
            }

            yield return new WaitForSeconds(0.35f);

            if (!reko.gameObject.activeSelf || leachKinGoal) break;
        }
        AKinPlayMode = false;
        yield return new WaitForSeconds(0.6f);

        enemyKomaList.Clear();
        Destroy(kin.gameObject);

        // プレイヤーが生きてたら成功
        if (leachKinGoal)
        {
            flg_beat_kin = true;
        }

        koma_kin.Disappear();
        tukuyomiA.Move(new Vector3(0, tukuyomiA.transform.localPosition.y), 0.5f);
        yield return new WaitForSeconds(1f);
    }

    #endregion

    #region 銀

    /// <summary>
    /// 銀
    /// </summary>
    /// <returns></returns>
    private IEnumerator AGinCoroutine()
    {
        // フィールド表示
        playField.ShowCellField(3, 3);
        reko.moveMode = TukuyomiGamePlayer.MoveMode.Cell;
        reko.CellLocation = new Vector2(1, 0);
        reko.gameObject.SetActive(true);

        // 銀を生成
        var gin = Instantiate(small_dummy, playField.transform);
        gin.SetKoma(Koma.Gin);
        gin.transform.localPosition = playField.GetCellPosition(new Vector2(1, 1));
        var rot = Mathf.PI;
        gin.transform.localRotation = Util.GetRotateQuaternion(rot);
        gin.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);

        var timer = new DeltaFloat();
        timer.Set(0);
        for (var i = 0; i < 5; ++i)
        {
            gin.gameObject.SetActive(true);
            // 向きを決定
            var muki = Util.RandomInt(0, 3);
            var mukiRot = muki switch
            {
                0 => 0f,                //0:上
                1 => Mathf.PI * 0.5f,   //1:左
                2 => Mathf.PI,          //2:下
                _ => Mathf.PI * 1.5f    //3:右
            };
            var atkCell = muki switch
            {
                0 => new Vector2(1, 2),
                1 => new Vector2(0, 1),
                2 => new Vector2(1, 0),
                _ => new Vector2(2, 1),
            };
            // 回転
            timer.MoveTo(0, Util.RandomFloat(2f, 3f), DeltaFloat.MoveType.LINE);
            while (timer.IsActive())
            {
                yield return null;
                timer.Update(Time.deltaTime);
                rot -= Mathf.PI * 16f * Time.deltaTime;
                if (rot < 0f) rot += Mathf.PI * 2f;
                gin.transform.localRotation = Util.GetRotateQuaternion(rot);
            }
            // 1回転するまで
            while (rot > 0f)
            {
                yield return null;
                rot -= Mathf.PI * 16f * Time.deltaTime;
                gin.transform.localRotation = Util.GetRotateQuaternion(rot);
            }
            // 決まった方向で止める
            while (rot > mukiRot)
            {
                yield return null;
                rot -= Mathf.PI * 16f * Time.deltaTime;
                gin.transform.localRotation = Util.GetRotateQuaternion(rot);
            }
            gin.transform.localRotation = Util.GetRotateQuaternion(mukiRot);
            resource.PlaySE(resource.se_koma_set);
            // 攻撃
            yield return new WaitForSeconds(0.6f);
            resource.PlaySE(resource.se_attack_effect_A);
            CreateAttackCell(new Vector2(0, 0));
            CreateAttackCell(new Vector2(2, 0));
            CreateAttackCell(new Vector2(0, 2));
            CreateAttackCell(new Vector2(2, 2));
            CreateAttackCell(new Vector2(1, 1));
            CreateAttackCell(atkCell);
            yield return new WaitForSeconds(1f);
            if (!reko.gameObject.activeSelf) break;
        }

        Destroy(gin.gameObject);

        // プレイヤーが生きてたら成功
        if (reko.gameObject.activeSelf)
        {
            flg_beat_gin = true;
        }

        koma_gin.Disappear();
        tukuyomiA.Move(new Vector3(0, tukuyomiA.transform.localPosition.y), 0.5f);
        yield return new WaitForSeconds(1f);
    }

    #endregion

    #region 桂馬

    /// <summary>
    /// 桂馬
    /// </summary>
    /// <returns></returns>
    private IEnumerator AKeiCoroutine()
    {
        // フィールド表示
        enemyKomaList.Clear();
        playField.ShowCellField(5, 5);
        reko.moveMode = TukuyomiGamePlayer.MoveMode.Cell;
        reko.CellLocation = new Vector2(2, 2);
        reko.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);

        // 桂馬をランダム生成
        for (var i = 0; i < 8; ++i)
        {
            if (!reko.gameObject.activeSelf) break;

            StartCoroutine(AttackKei());

            yield return new WaitForSeconds(1f);
        }
        yield return new WaitForSeconds(1.5f);

        // プレイヤーが生きてたら成功
        if (reko.gameObject.activeSelf)
        {
            flg_beat_kei = true;
        }

        koma_kei.Disappear();
        tukuyomiA.Move(new Vector3(0, tukuyomiA.transform.localPosition.y), 0.5f);
        yield return new WaitForSeconds(1f);
    }

    /// <summary>
    /// 桂馬攻撃2個
    /// </summary>
    /// <returns></returns>
    private IEnumerator AttackKei()
    {
        var small1 = Instantiate(small_dummy, playField.transform);
        small1.transform.position = koma_kei.transform.position;
        small1.SetKoma(Koma.Kei);
        small1.gameObject.SetActive(true);
        var small2 = Instantiate(small_dummy, playField.transform);
        small2.transform.position = koma_kei.transform.position;
        small2.SetKoma(Koma.Kei);
        small2.gameObject.SetActive(true);

        var p1 = new DeltaVector3();
        p1.Set(small1.transform.localPosition);
        var p2 = new DeltaVector3();
        p2.Set(small2.transform.localPosition);

        // 打つ場所を選択
        var atkCell1 = new Vector2(Util.RandomInt(0, 4), Util.RandomInt(0, 4));
        var atkCell2 = new Vector2((atkCell1.x + Util.RandomInt(1, 4)) % 5, (atkCell1.y + Util.RandomInt(1, 4)) % 5);
        p1.MoveTo(playField.GetCellPosition(atkCell1), 0.3f, DeltaFloat.MoveType.LINE);
        p2.MoveTo(playField.GetCellPosition(atkCell2), 0.3f, DeltaFloat.MoveType.LINE);
        while (p1.IsActive() || p2.IsActive())
        {
            yield return null;
            p1.Update(Time.deltaTime);
            p2.Update(Time.deltaTime);
            small1.transform.localPosition = p1.Get();
            small2.transform.localPosition = p2.Get();
        }
        resource.PlaySE(resource.se_koma_set);
        yield return new WaitForSeconds(0.7f);

        // 攻撃
        Destroy(small1.gameObject);
        Destroy(small2.gameObject);
        var atkList = new List<Vector2>();
        var distList = new List<Vector2>(){
            new Vector2(2, 1),
            new Vector2(1, 2),
            new Vector2(-1, 2),
            new Vector2(-2, 1),
            new Vector2(-2, -1),
            new Vector2(-1, -2),
            new Vector2(1, -2),
            new Vector2(2, -1)
        };
        foreach (var dist in distList)
        {
            var atk1 = atkCell1 + dist;
            var atk2 = atkCell2 + dist;
            if (!atkList.Contains(atk1)) atkList.Add(atk1);
            if (!atkList.Contains(atk2)) atkList.Add(atk2);
        }
        // コマ自身の位置
        if (!atkList.Contains(atkCell1)) atkList.Add(atkCell1);
        if (!atkList.Contains(atkCell2)) atkList.Add(atkCell2);

        foreach (var a in atkList)
            CreateAttackCell(a);

        resource.PlaySE(resource.se_attack_effect_A);
    }

    #endregion

    #region 香車

    /// <summary>
    /// 香車
    /// </summary>
    /// <returns></returns>
    private IEnumerator AKyouCoroutine()
    {
        // フィールド表示
        playField.transform.localPosition = new Vector3(0, AFIELD_Y_KYOU);
        playField.ShowCellField(1, 1);
        reko.moveMode = TukuyomiGamePlayer.MoveMode.Disable;
        reko.ShotEnable = true;
        reko.CellLocation = new Vector2(0, 0);
        reko.gameObject.SetActive(true);

        // 香車を生成
        var kyou = Instantiate(small_dummy, playField.transform);
        kyou.transform.localPosition = playField.GetCellPosition(new Vector2(0, 7));
        kyou.SetKoma(Koma.Kyou);
        kyou.EnableDamageMode();
        kyou.transform.localRotation = Util.GetRotateQuaternion(Mathf.PI);
        kyou.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);

        pressUI.SetActive(true);
        yield return new WaitUntil(() => InputManager.GetInstance().GetKeyPress(InputManager.Keys.South));
        pressUI.SetActive(false);

        // 香車のショット開始
        shotList.Clear();
        while (true)
        {
            if (!kyou.gameObject.activeSelf || !reko.gameObject.activeSelf)
            {
                // 香車撃破またはプレイヤー消滅で終了
                break;
            }

            // 続いてたら香車ショット生成
            CreateShot(kyou.transform.localPosition, new Vector3(0, -500f), TukuyomiGameShot.ShotType.Enemy);

            yield return new WaitForSeconds(Util.RandomFloat(0.1f, 0.16f));
        }

        reko.ShotEnable = false;
        // ショット消えるまで待機
        yield return new WaitWhile(() => shotList.Any());

        // プレイヤーが生きてたら成功
        if (reko.gameObject.activeSelf)
        {
            flg_beat_kyou = true;
        }

        playField.transform.localPosition = new Vector3(0, AFIELD_Y_DEFAULT);
        Destroy(kyou.gameObject);
        koma_kyou.Disappear();
        tukuyomiA.Move(new Vector3(0, tukuyomiA.transform.localPosition.y), 0.5f);
        yield return new WaitForSeconds(1f);
    }

    #endregion

    #region 歩

    private List<TukuyomiGameKomaSmall> enemyKomaList = new List<TukuyomiGameKomaSmall>();

    /// <summary>
    /// その座標に歩が居るか判定
    /// </summary>
    /// <param name="cell"></param>
    /// <returns></returns>
    public bool AEnemyIsInField(Vector2 cell)
    {
        return enemyKomaList.Any(h => h.CellLocation == cell && h.gameObject.activeSelf);
    }

    /// <summary>
    /// 歩
    /// </summary>
    /// <returns></returns>
    private IEnumerator AHuCoroutine()
    {
        // フィールド表示
        enemyKomaList.Clear();
        playField.ShowCellField(5, 5);
        reko.moveMode = TukuyomiGamePlayer.MoveMode.Cell;
        reko.CellLocation = new Vector2(2, 2);
        reko.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);

        // 歩を作成
        const int Offset = 8;
        var tmp0 = 2;
        var tmp1 = 2;
        var tmp2 = 2;
        var add0 = Util.RandomInt(1, 3);
        var add1 = Util.RandomInt(1, 3);
        var add2 = Util.RandomInt(1, 3);
        for (var i = 0; i < 5; ++i)
        {
            // 上から下
            enemyKomaList.Add(ACreateHu(0, i, tmp0 + Offset));
            // 右から左
            enemyKomaList.Add(ACreateHu(1, tmp1 + Offset, i));
            // 左から右
            enemyKomaList.Add(ACreateHu(2, tmp2 - Offset, i));

            var hugo = i % 2 == 0 ? 1 : -1;
            tmp0 += add0 * hugo;
            tmp1 += add1 * hugo;
            tmp2 += add2 * hugo;
            add0 = (add0 == 1) ? 2 : Util.RandomInt(1, 3);
            add1 = (add1 == 1) ? 2 : Util.RandomInt(1, 3);
            add2 = (add2 == 1) ? 2 : Util.RandomInt(1, 3);
        }

        // 全部消えるまで待つ
        yield return new WaitWhile(() => enemyKomaList.Any(hu => hu.gameObject.activeSelf));
        yield return new WaitForSeconds(1f);
        foreach (var hu in enemyKomaList) Destroy(hu);
        enemyKomaList.Clear();

        // プレイヤーが生きてたら成功
        if (reko.gameObject.activeSelf)
        {
            flg_beat_hu = true;
        }

        koma_hu.Disappear();
        tukuyomiA.Move(new Vector3(0, tukuyomiA.transform.localPosition.y), 0.5f);
        yield return new WaitForSeconds(1f);
    }

    /// <summary>
    /// 歩を作成
    /// </summary>
    /// <param name="type"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private TukuyomiGameKomaSmall ACreateHu(int type, int x, int y)
    {
        var hu = Instantiate(small_dummy, playField.transform);
        hu.SetKoma(Koma.Hu);
        hu.CellLocation = new Vector2(x, y);
        hu.transform.localPosition = playField.GetCellPosition(hu.CellLocation);
        hu.transform.localRotation = Util.GetRotateQuaternion(
            type switch
            {
                0 => Mathf.PI,
                1 => Mathf.PI / 2f,
                _ => -Mathf.PI / 2f,
            });
        hu.gameObject.SetActive(true);
        StartCoroutine(AHuAttackCoroutine(hu, type));

        return hu;
    }

    /// <summary>
    /// 歩が進むコルーチン
    /// </summary>
    /// <param name="hu"></param>
    /// <param name="type">0:上から下　1:右から左　2:左から右</param>
    /// <returns></returns>
    private IEnumerator AHuAttackCoroutine(TukuyomiGameKomaSmall hu, int type)
    {
        var cell = hu.CellLocation;
        while (true)
        {
            yield return new WaitForSeconds(0.6f);

            if (type == 0)
            {
                cell.y--;
                if (cell.y < 0) break;
            }
            else if (type == 1)
            {
                cell.x--;
                if (cell.x < 0) break;
            }
            else if (type == 2)
            {
                cell.x++;
                if (cell.x >= 0 && !playField.InField(cell)) break;
            }
            hu.CellLocation = cell;
            hu.transform.localPosition = playField.GetCellPosition(cell);

            // 移動先に★が居たらダメージ与えて消す
            if (reko.CellLocation == cell && reko.gameObject.activeSelf)
            {
                reko.gameObject.SetActive(false);
                PlayerDamage();
            }
        }

        // 出たら消える
        hu.gameObject.SetActive(false);
    }

    #endregion

    #endregion
}

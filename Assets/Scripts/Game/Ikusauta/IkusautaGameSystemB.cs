using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Windows;

/// <summary>
/// 軍歌マチ ボスラッシュ用管理
/// </summary>
public class IkusautaGameSystemB : GameSceneScriptBase
{
    #region 定数

    /// <summary></summary>
    private const float COMMAND_SIZE = 80f;

    /// <summary>待機時間</summary>
    private const float TIME_MAX = 0.55f;
    /// <summary>追加時間</summary>
    private const float TIME_ADD = 0.18f;

    /// <summary>勝利条件・成功回数</summary>
    private const int WIN_SUCCESS_CNT = 7;
    /// <summary>敗北条件・失敗回数</summary>
    private const int LOSE_FAIL_CNT = 3;

    /// <summary>斬った後元の位置の戻るジャンプ時間</summary>
    private const float RESULT_JUMP_TIME = 0.5f;

    #region enum

    /// <summary>コマンド向き</summary>
    public enum ArrowDir
    {
        Up = 0,
        Down,
        Right,
        Left,
        Button,
    }

    /// <summary>表示キャラクター</summary>
    private enum CharacterMode
    {
        Waiting = 0,
        Up,
        Down,
        Right,
        Left,
        Result,
    }

    /// <summary>軌跡表示シチュエーション</summary>
    private enum SlashSituation : int
    {
        /// <summary>上</summary>
        Up = 0,
        /// <summary>下</summary>
        Down,
        /// <summary>右</summary>
        Right,
        /// <summary>左</summary>
        Left,
        /// <summary>成功時</summary>
        Win,
        /// <summary>失敗時</summary>
        Lose,
        /// <summary>成功から戻る時</summary>
        BackWin,
        /// <summary>失敗から戻る時</summary>
        BackLose,
    }

    #endregion

    #endregion

    #region メンバー

    public GameObject parent_wait;
    public GameObject parent_up;
    public GameObject parent_down;
    public GameObject parent_left;
    public GameObject parent_right;
    public GameObject parent_result;
    public IkusautaGameBResultCharacter matiA;
    public IkusautaGameBResultCharacter matiB;
    public Transform matiA_up;
    public Transform matiB_up;
    public Transform matiA_down;
    public Transform matiB_down;
    public Transform matiA_right;
    public Transform matiB_right;
    public Transform matiA_left;
    public Transform matiB_left;

    public Transform command_bg;
    public Transform time_gauge;
    public Transform arrow_parent;
    public IkusautaGameBArrow arrow_dummy;
    public Transform slash_parent;
    public IkusautaGameBSlash slash_dummy;

    /// <summary>コマンド表示SE</summary>
    public AudioClip se_bikkuri;
    /// <summary>打ち合いSE</summary>
    public AudioClip se_utiai;
    /// <summary>攻撃成功SE</summary>
    public AudioClip se_attack;
    /// <summary>素振りSE</summary>
    public AudioClip se_suburi;
    /// <summary>剣をしまうSE</summary>
    public AudioClip se_stack;

    #endregion

    #region 変数

    /// <summary>入力待ちインデックス</summary>
    private int waitIndex;
    /// <summary>矢印リスト</summary>
    private List<IkusautaGameBArrow> arrowList;

    /// <summary>成功回数</summary>
    private int successCount;
    /// <summary>失敗回数</summary>
    private int failCount;

    #endregion

    #region 基底

    /// <summary>
    /// 開始時
    /// </summary>
    /// <returns></returns>
    public override IEnumerator Start()
    {
        yield return base.Start();

        arrowList = new List<IkusautaGameBArrow>();
        ShowCharacter(CharacterMode.Waiting);
        DispTimeGauge(-1);
        arrow_dummy.gameObject.SetActive(false);
        slash_dummy.gameObject.SetActive(false);
        command_bg.gameObject.SetActive(false);
    }

    /// <summary>
    /// フェードイン後
    /// </summary>
    /// <returns></returns>
    public override IEnumerator AfterFadeIn()
    {
        var tutorial = ManagerSceneScript.GetInstance().GetMinigameTutorialWindow();
        var input = InputManager.GetInstance();

        yield return base.AfterFadeIn();

        yield return new WaitForSeconds(1f);
        // チュートリアル表示
        tutorial.SetTitle(StringMinigameMessage.MatiB_Title);
        tutorial.SetText(StringMinigameMessage.MatiB_Tutorial);
        yield return tutorial.Open();
        yield return new WaitUntil(() => input.GetKeyPress(InputManager.Keys.South));
        yield return tutorial.Close();

        // 開始
        StartCoroutine(GameCoroutine());
    }

    #endregion

    #region 進行コルーチン

    /// <summary>
    /// ゲーム開始コルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator GameCoroutine()
    {
        ShowCharacter(CharacterMode.Waiting);
        var sound = ManagerSceneScript.GetInstance().soundMan;
        var input = InputManager.GetInstance();

        sound.PlaySE(se_stack);
        command_bg.gameObject.SetActive(true);
        var bgWidth = (CalcCommandCount() + 1.1f) * COMMAND_SIZE;
        command_bg.localScale = new Vector3(bgWidth, COMMAND_SIZE * 1.1f, 1f);

        var time_limit = TIME_MAX;
        DispTimeGauge(time_limit);

        yield return new WaitForSeconds(Util.RandomFloat(2f, 4f));

        // 作成して表示
        sound.PlaySE(se_bikkuri);
        CreateArrowList();

        // 時間制限開始

        // 入力待ち
        while (true)
        {
            yield return null;

            // 入力
            var up = input.GetKeyPress(InputManager.Keys.Up);
            var down = input.GetKeyPress(InputManager.Keys.Down);
            var right = input.GetKeyPress(InputManager.Keys.Right);
            var left = input.GetKeyPress(InputManager.Keys.Left);
            var button = input.GetKeyPress(InputManager.Keys.South);

            if (up || down || right || left || button)
            {
                // 入力待ちのやつ
                var waitDir = arrowList[waitIndex].GetDirection();

                if (up && waitDir == ArrowDir.Up ||
                    down && waitDir == ArrowDir.Down ||
                    right && waitDir == ArrowDir.Right ||
                    left && waitDir == ArrowDir.Left ||
                    button && waitDir == ArrowDir.Button)
                {
                    // 入力成功
                    time_limit += TIME_ADD;
                    if (time_limit > TIME_MAX) time_limit = TIME_MAX;
                    DispTimeGauge(time_limit);

                    if (waitIndex >= arrowList.Count - 1)
                    {
                        // ラストなら成功コルーチンへ
                        StartCoroutine(ResultCoroutine(true));
                        yield break;
                    }

                    // 打ち合い
                    sound.PlaySE(se_utiai);

                    // 矢印1個消す
                    arrowList[waitIndex].gameObject.SetActive(false);
                    waitIndex++;

                    // キャラ表示
                    ShowCharacter(waitDir switch
                    {
                        ArrowDir.Up => CharacterMode.Up,
                        ArrowDir.Down => CharacterMode.Down,
                        ArrowDir.Right => CharacterMode.Right,
                        _ => CharacterMode.Left,
                    });

                    // 軌跡
                    CreateSlashToObj(waitDir switch
                    {
                        ArrowDir.Up => SlashSituation.Up,
                        ArrowDir.Down => SlashSituation.Down,
                        ArrowDir.Right => SlashSituation.Right,
                        _ => SlashSituation.Left,
                    });
                }
                else
                {
                    // 失敗
                    StartCoroutine(ResultCoroutine(false));
                    yield break;
                }
            }
            else
            {
                // 押してないと時間減る
                time_limit -= Time.deltaTime;
                if (time_limit <= 0f)
                {
                    // 時間切れで失敗
                    StartCoroutine(ResultCoroutine(false));
                    yield break;
                }
                else
                {
                    // 次へ
                    DispTimeGauge(time_limit);
                }
            }
        }
    }

    /// <summary>
    /// 入力成功コルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator ResultCoroutine(bool isSuccess)
    {
        // 片付け
        DispTimeGauge(-1);
        ReleaseArrows();

        // 攻撃SE
        var sound = ManagerSceneScript.GetInstance().soundMan;
        sound.PlaySE(se_attack);

        // 表示
        ShowCharacter(CharacterMode.Result, isSuccess);
        CreateSlashToObj(isSuccess ? SlashSituation.Win : SlashSituation.Lose);

        yield return new WaitForSeconds(1.5f);

        // 結果判定
        if (isSuccess)
        {
            successCount++;
            if (successCount >= WIN_SUCCESS_CNT)
            {
                yield return new WaitForSeconds(1f);
                // 勝利で終了
                Global.GetTemporaryData().bossRushMatiWon = true;
                ExitGame();
                yield break;
            }
        }
        else
        {
            failCount++;
            if (failCount >= LOSE_FAIL_CNT)
            {
                yield return new WaitForSeconds(1f);
                // 負けで終了
                Global.GetTemporaryData().bossRushMatiWon = false;
                ExitGame();
                yield break;
            }
        }

        // 元の位置に戻ってゲーム再開
        StartCoroutine(matiA.BackToBase(RESULT_JUMP_TIME));
        StartCoroutine(matiB.BackToBase(RESULT_JUMP_TIME));
        // 勝ったほうに軌跡表示
        yield return new WaitForSeconds(RESULT_JUMP_TIME * 0.29f);
        sound.PlaySE(se_suburi);
        var slashRot = Util.RandomFloat(0, 2f);
        for (var i = 0; i < 3; ++i)
        {
            CreateSlashToObj(isSuccess ? SlashSituation.BackWin : SlashSituation.BackLose, Mathf.PI * slashRot);
            yield return new WaitForSeconds(RESULT_JUMP_TIME * 0.07f);

            slashRot += 0.67f;
            if (slashRot >= 2f) slashRot -= 2f;
        }
        yield return new WaitForSeconds(RESULT_JUMP_TIME * 0.5f);

        StartCoroutine(GameCoroutine());
    }

    #endregion

    #region 機能メソッド

    /// <summary>
    /// キャラ表示
    /// </summary>
    /// <param name="mode"></param>
    /// <param name="isSuccess"></param>
    private void ShowCharacter(CharacterMode mode, bool isSuccess = true)
    {
        parent_wait.SetActive(mode == CharacterMode.Waiting);
        parent_up.SetActive(mode == CharacterMode.Up);
        parent_down.SetActive(mode == CharacterMode.Down);
        parent_right.SetActive(mode == CharacterMode.Right);
        parent_left.SetActive(mode == CharacterMode.Left);
        parent_result.SetActive(mode == CharacterMode.Result);

        if (mode == CharacterMode.Result)
        {
            matiA.SetResultDisp(isSuccess);
            matiB.SetResultDisp(!isSuccess);
        }
    }

    private int CalcCommandCount()
    {
        return 3 + successCount; //作成数　実際は最後にボタンをつけるので＋１
    }

    /// <summary>
    /// コマンド作成して表示
    /// </summary>
    private void CreateArrowList()
    {
        var createCnt = CalcCommandCount();
        var createX = -0.5f * (createCnt) * COMMAND_SIZE; // 1個目の表示場所

        var arrowDir = Util.RandomInt((int)ArrowDir.Up, (int)ArrowDir.Left);
        for (var i = 0; i < createCnt; ++i)
        {
            var arrow = CreateArrow(arrowDir, createX);
            arrowList.Add(arrow);

            createX += COMMAND_SIZE;
            // 次のコマンド　同じのを連続では出さない
            var next = Util.RandomInt((int)ArrowDir.Up, (int)ArrowDir.Left - 1);
            arrowDir = next >= arrowDir ? next + 1 : next;
        }

        // 最後に決定ボタン
        var button = CreateArrow((int)ArrowDir.Button, createX);
        arrowList.Add(button);

        waitIndex = 0;
    }

    /// <summary>
    /// 矢印作成
    /// </summary>
    /// <param name="arrowDir"></param>
    /// <param name="x"></param>
    /// <returns></returns>
    private IkusautaGameBArrow CreateArrow(int arrowDir, float x)
    {
        var inst = Instantiate(arrow_dummy);
        inst.transform.SetParent(arrow_parent);
        inst.transform.localPosition = new Vector3(x, 0, 0);
        inst.SetDirection((ArrowDir)arrowDir);

        inst.gameObject.SetActive(true);
        return inst;
    }

    /// <summary>
    /// 矢印を削除
    /// </summary>
    private void ReleaseArrows()
    {
        foreach (var a in arrowList)
        {
            Destroy(a.gameObject);
        }

        arrowList.Clear();
        command_bg.gameObject.SetActive(false);
    }

    /// <summary>
    /// ゲージ表示
    /// </summary>
    /// <param name="now"></param>
    /// <param name="max"></param>
    private void DispTimeGauge(float now, float max = TIME_MAX)
    {
        if (now <= 0f)
        {
            time_gauge.gameObject.SetActive(false);
            return;
        }

        time_gauge.gameObject.SetActive(true);

        var width = Constant.SCREEN_WIDTH * now / max;
        var height = time_gauge.localScale.y;
        time_gauge.localScale = new Vector3(width, height, 1);
    }

    /// <summary>
    /// 軌跡表示
    /// </summary>
    /// <param name="sit"></param>
    /// <param name="rot"></param>
    private void CreateSlashToObj(SlashSituation sit, float rot = 0f)
    {
        var colorA = Color.white;
        var colorB = new Color(0.8f, 0.4f, 0.4f);

        switch (sit)
        {
            case SlashSituation.Up:
                CreateSlash(matiA_up.position, CalcRandomSlash(3), Mathf.PI * Util.RandomFloat(0.25f, 1.5f), colorA);
                CreateSlash(matiB_up.position, CalcRandomSlash(3), Mathf.PI * Util.RandomFloat(-0.5f, 0f), colorB);
                break;
            case SlashSituation.Down:
                CreateSlash(matiA_down.position, CalcRandomSlash(1), Mathf.PI * Util.RandomFloat(1f, 1.5f), colorA);
                CreateSlash(matiB_down.position, CalcRandomSlash(1), Mathf.PI * Util.RandomFloat(-0.5f, 0f), colorB);
                break;
            case SlashSituation.Right:
                CreateSlash(matiA_right.position, IkusautaGameBSlash.Type.Curve, Mathf.PI * Util.RandomFloat(0.25f, 1.75f), colorA);
                CreateSlash(matiB_right.position, IkusautaGameBSlash.Type.Line, 0f, colorB);
                break;
            case SlashSituation.Left:
                CreateSlash(matiA_left.position, CalcRandomSlash(3), Mathf.PI * Util.RandomFloat(0.7f, 1.3f), colorA);
                CreateSlash(matiB_left.position, IkusautaGameBSlash.Type.Curve, Mathf.PI * Util.RandomFloat(-0.3f, 0.5f), colorB);
                break;
            case SlashSituation.Win:
                CreateSlash(matiB.transform.position, IkusautaGameBSlash.Type.Line, Mathf.PI * 0.9f, colorA);
                break;
            case SlashSituation.Lose:
                CreateSlash(matiA.transform.position, IkusautaGameBSlash.Type.Curve, Mathf.PI * -0.2f, colorB);
                break;
            case SlashSituation.BackWin:
                CreateSlash(matiA.transform.position, IkusautaGameBSlash.Type.Curve, rot, colorA);
                break;
            case SlashSituation.BackLose:
                CreateSlash(matiB.transform.position, IkusautaGameBSlash.Type.Curve, -rot, colorB);
                break;
        }
    }

    /// <summary>
    /// 剣軌跡表示
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="type"></param>
    /// <param name="dir">方向</param>
    /// <param name="color">色</param>
    private void CreateSlash(Vector3 pos, IkusautaGameBSlash.Type type, float dir, Color? color = null)
    {
        var inst = Instantiate(slash_dummy);
        inst.transform.SetParent(slash_parent);
        inst.transform.localPosition = pos;
        inst.Show(type, dir, color);
    }

    /// <summary>
    /// ランダムで軌跡タイプを決める
    /// </summary>
    /// <param name="curve_rate">曲線のほうの出やすさ　1:等倍　3:３対１で曲線</param>
    /// <returns></returns>
    private IkusautaGameBSlash.Type CalcRandomSlash(int curve_rate)
    {
        return Util.RandomInt(0, curve_rate) == 0 ? IkusautaGameBSlash.Type.Line : IkusautaGameBSlash.Type.Curve;
    }

    /// <summary>
    /// 終了処理
    /// </summary>
    private void ExitGame()
    {
        //ManagerSceneScript.GetInstance().ExitGame();
        ManagerSceneScript.GetInstance().NextGame("GameScenePierreB");
    }

    #endregion
}

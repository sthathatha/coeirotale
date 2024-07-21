using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

/// <summary>
/// ボスラッシュピエール
/// </summary>
public class PierreGameSystemB : GameSceneScriptBase
{
    #region 定数

    /// <summary>床オブジェクト生成位置</summary>
    private const float OBJ_INIT_X = (Constant.SCREEN_WIDTH + PierreGameBGObject.OBJECT_WIDTH_MAX) / (-2f);

    /// <summary>上端の座標</summary>
    public const float FIELD_MAX_Y = 210f;
    /// <summary>下端の座標</summary>
    public const float FIELD_MIN_Y = -365f;
    /// <summary>フィールド中央</summary>
    public const float FIELD_CENTER_Y = (FIELD_MAX_Y + FIELD_MIN_Y) / 2f;

    /// <summary>HP最大のマスク位置</summary>
    private const float GAUGE_MAX_X = 1000f;
    /// <summary>HP0のマスク位置</summary>
    private const float GAUGE_MIN_X = 0f;

    /// <summary>ゲーム進行</summary>
    public enum GameState : int
    {
        LOADING = 0,
        PLAY,
        ENDING,
    }

    #endregion

    #region メンバー

    /// <summary>オブジェクト親</summary>
    public GameObject objectParent = null;
    /// <summary>ボール追加用親</summary>
    public Transform ballParent = null;
    /// <summary>地面テンプレ</summary>
    public GameObject ground_dummy = null;
    /// <summary>ボール0のテンプレート</summary>
    public PierreGameBallB ball_dummy = null;

    /// <summary>ピエールA</summary>
    public PierreGameBPlayer pierreA = null;
    /// <summary>ピエールB</summary>
    public PierreGameBEnemy pierreB = null;

    /// <summary>ボスHPゲージのマスク</summary>
    public Transform bossHPGauge;

    /// <summary>曲名</summary>
    public PierreGameBFadeUI musicText;
    /// <summary>カード名</summary>
    public PierreGameBFadeUI cardNameText;

    /// <summary>スペル発動時SE</summary>
    public AudioClip se_phaseChange;
    /// <summary>敵死亡時のSE</summary>
    public AudioClip se_enemy_death;

    #endregion

    #region プロパティ

    /// <summary>ゲーム状態</summary>
    public GameState State { get; private set; } = GameState.LOADING;

    #endregion

    #region 変数

    /// <summary>最新の地面</summary>
    private GameObject newBG = null;

    #endregion

    #region 基底

    /// <summary>
    /// 初期化
    /// </summary>
    /// <returns></returns>
    override public IEnumerator Start()
    {
        DisplayHP(1);
        GenerateInitObjects();
        StartCoroutine(GenerateGroundCoroutine());
        yield return musicText.Hide();
        yield return cardNameText.Hide();

        yield return base.Start();
    }

    /// <summary>
    /// フェードイン後ゲーム開始
    /// </summary>
    /// <returns></returns>
    public override IEnumerator AfterFadeIn()
    {
        var tutorial = ManagerSceneScript.GetInstance().GetMinigameTutorialWindow();
        var input = InputManager.GetInstance();

        yield return base.AfterFadeIn();

        yield return musicText.Show(StringMinigameMessage.PierreB_Music, 0.5f);
        tutorial.SetTitle(StringMinigameMessage.PierreB_Title);
        tutorial.SetText(StringMinigameMessage.PierreB_Tutorial);
        yield return tutorial.Open();
        yield return new WaitUntil(() => input.GetKeyPress(InputManager.Keys.South));
        StartCoroutine(musicText.Hide(0.5f));
        yield return tutorial.Close();

        State = GameState.PLAY;
    }

    #endregion

    #region 機能呼び出し

    /// <summary>
    /// ボール生成
    /// </summary>
    /// <param name="startPos">生成位置</param>
    /// <param name="direction">速度</param>
    /// <param name="type">敵味方</param>
    /// <param name="color">色</param>
    /// <param name="power">攻撃力</param>
    public void GenerateBall(Vector3 startPos, Vector3 direction, PierreGameBallB.AttackType type, Color color, int power = 1)
    {
        var ball = Instantiate(ball_dummy);
        ball.transform.SetParent(ballParent, false);
        ball.SetParam(type, startPos, direction, color, power);
        ball.gameObject.SetActive(true);
    }

    /// <summary>
    /// ボール全消し
    /// </summary>
    public void DeleteAllBall(PierreGameBallB.AttackType deleteType)
    {
        var balls = ballParent.GetComponentsInChildren<PierreGameBallB>();
        foreach (var ball in balls)
        {
            if (ball.attacktype == deleteType)
            {
                ball.DestroyWait();
            }
        }
    }

    /// <summary>
    /// HPゲージ表示
    /// </summary>
    /// <param name="rate"></param>
    public void DisplayHP(float rate)
    {
        var x = Util.CalcBetweenFloat(rate, GAUGE_MIN_X, GAUGE_MAX_X);
        bossHPGauge.localPosition = new Vector3(x, 0, 0);
    }

    /// <summary>
    /// フェーズメッセージ表示
    /// </summary>
    /// <param name="phase"></param>
    public void ShowPhaseMessage(int phase)
    {
        StartCoroutine(ShowPhaseCoroutine(phase));
    }

    /// <summary>
    /// フェーズメッセージ表示
    /// </summary>
    /// <param name="phase"></param>
    /// <returns></returns>
    private IEnumerator ShowPhaseCoroutine(int phase)
    {
        var sound = ManagerSceneScript.GetInstance().soundMan;
        sound.PlaySE(se_phaseChange);

        if (phase == 1)
        {
            yield return cardNameText.Show(StringMinigameMessage.PierreB_Spell1, 0.5f);
        }
        else
        {
            yield return cardNameText.Show(StringMinigameMessage.PierreB_Spell2, 0.5f);
        }
        yield return new WaitForSeconds(2f);
        yield return cardNameText.Hide(0.5f);
    }

    #endregion

    #region ゲーム流れ処理

    /// <summary>
    /// 終了処理
    /// </summary>
    /// <param name="isWin"></param>
    public void EndGame(bool isWin)
    {
        if (State != GameState.PLAY) return;
        State = GameState.ENDING;

        Global.GetTemporaryData().bossRushPierreWon = isWin;

        StartCoroutine(EndGameCoroutine(isWin));
    }

    /// <summary>
    /// 終了処理コルーチン
    /// </summary>
    /// <param name="isWin"></param>
    /// <returns></returns>
    private IEnumerator EndGameCoroutine(bool isWin)
    {
        var sound = ManagerSceneScript.GetInstance().soundMan;
        if (isWin)
        {
            for (var i = 0; i < 10; ++i)
            {
                sound.PlaySE(se_enemy_death);
                yield return new WaitForSeconds(0.25f);
            }
        }

        yield return new WaitForSeconds(2f);
        ManagerSceneScript.GetInstance().ExitGame();
        //ManagerSceneScript.GetInstance().NextGame("GameSceneBossB");
    }

    #endregion

    #region 背景オブジェクトまわり

    /// <summary>
    /// 初期オブジェクト作成
    /// </summary>
    private void GenerateInitObjects()
    {
        // BG　右端だけ作ればすぐ作られる
        newBG = Instantiate(ground_dummy);
        newBG.transform.SetParent(objectParent.transform);
        newBG.transform.localPosition = new Vector3(0, 0);
    }

    /// <summary>
    /// 地面を常に生成
    /// </summary>
    /// <returns></returns>
    private IEnumerator GenerateGroundCoroutine()
    {
        while (true)
        {
            yield return new WaitUntil(() => newBG.transform.localPosition.x > 0f);

            var bg = Instantiate(ground_dummy);
            bg.transform.SetParent(objectParent.transform, false);
            bg.transform.localPosition = new Vector3(newBG.transform.localPosition.x - Constant.SCREEN_WIDTH, 0);

            newBG = bg;
        }
    }
    #endregion
}

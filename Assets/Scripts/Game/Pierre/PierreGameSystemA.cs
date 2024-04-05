using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class PierreGameSystemA : GameSceneScriptBase
{
    #region 定数
    /// <summary>スクロール速度</summary>
    public const float SCROLL_SPEED = 600f;

    /// <summary>にぎやかしオブジェクト生成位置</summary>
    private const float OBJ_INIT_X = (Constant.SCREEN_WIDTH + PierreGameBGObject.OBJECT_WIDTH_MAX) / (-2f);
    /// <summary>道の枠生成Y座標</summary>
    private const float WAKU_Y = 120f;

    /// <summary>プレイヤー初期位置</summary>
    public const float PLAYER_INIT_X = 500f;

    public enum GameState : int
    {
        Main = 0,
        Talk,
        Warp,
    }
    /// <summary>ゲーム状態</summary>
    public GameState state;

    /// <summary>ランダム表示メッセージ</summary>
    private string[] randomMessage =
    {
        StringMinigameMessage.PierreA_Random0,
        StringMinigameMessage.PierreA_Random1,
        StringMinigameMessage.PierreA_Random2,
        StringMinigameMessage.PierreA_Random3,
        StringMinigameMessage.PierreA_Random4,
        StringMinigameMessage.PierreA_Random5,
        StringMinigameMessage.PierreA_Random6,
    };
    #endregion

    #region メンバー

    /// <summary>道の枠上側</summary>
    public GameObject wakuTopTemplate = null;
    /// <summary>道の枠下側</summary>
    public GameObject wakuBottomTemplate = null;
    /// <summary>木１</summary>
    public GameObject tree1Template = null;
    /// <summary>木２</summary>
    public GameObject tree2Template = null;
    /// <summary>石１</summary>
    public GameObject stone1Template = null;
    /// <summary>石２</summary>
    public GameObject stone2Template = null;
    /// <summary>石３</summary>
    public GameObject stone3Template = null;

    /// <summary>奥オブジェクト追加用Parent</summary>
    public GameObject backObjectParent = null;
    /// <summary>手前オブジェクト追加用Parent</summary>
    public GameObject frontObjectParent = null;
    /// <summary>道の枠追加用Parent</summary>
    public GameObject wakuParent = null;

    /// <summary>上枠Sprite１</summary>
    public Sprite wakuTopSprite0 = null;
    /// <summary>上枠Sprite２</summary>
    public Sprite wakuTopSprite1 = null;
    /// <summary>上枠Sprite３</summary>
    public Sprite wakuTopSprite2 = null;
    /// <summary>上枠Sprite４</summary>
    public Sprite wakuTopSprite3 = null;
    /// <summary>下枠Sprite１</summary>
    public Sprite wakuBottomSprite0 = null;
    /// <summary>下枠Sprite２</summary>
    public Sprite wakuBottomSprite1 = null;
    /// <summary>下枠Sprite３</summary>
    public Sprite wakuBottomSprite2 = null;
    /// <summary>下枠Sprite４</summary>
    public Sprite wakuBottomSprite3 = null;

    /// <summary>ボール0のテンプレート</summary>
    public GameObject ball0Template = null;

    /// <summary>道アイテム追加する用Parent</summary>
    public GameObject roadParent = null;

    /// <summary>ピエール</summary>
    public PierreGamePierreA pierreA = null;
    /// <summary>プレイヤー</summary>
    public PierreGamePlayerA playerA = null;

    /// <summary>左の帽子</summary>
    public PierreGameRoadObject hutL = null;
    /// <summary>右の帽子</summary>
    public PierreGameRoadObject hutR = null;

    /// <summary>メッセージUI</summary>
    public SimpleMessageWindow messageUI = null;
    #endregion

    #region プライベート
    /// <summary>最新の上枠</summary>
    private GameObject newWakuTop = null;
    /// <summary>最新の下枠</summary>
    private GameObject newWakuBottom = null;

    public enum PierreLevel : int
    {
        L1 = 0,
        L2,
        L3,
    }
    /// <summary>ピエールAI段階</summary>
    private PierreLevel pLevel;
    public PierreLevel GetPierreLevel() { return pLevel; }
    #endregion

    /// <summary>
    /// 初期化
    /// </summary>
    /// <returns></returns>
    override public IEnumerator Start()
    {
        yield return base.Start();

        hutL.gameObject.SetActive(false);
        hutR.gameObject.SetActive(false);
        messageUI.Close();

        GenerateInitObjects();

        StartCoroutine(GenerateTreeCoroutine());
        StartCoroutine(GenerateBackStoneCoroutine());
        StartCoroutine(GenerateFrontStoneCoroutine());
        StartCoroutine(GenerateWakuCoroutine());
    }

    /// <summary>
    /// フェードイン後ゲーム開始
    /// </summary>
    /// <returns></returns>
    public override IEnumerator AfterFadeIn()
    {
        var tutorial = ManagerSceneScript.GetInstance().GetMinigameTutorialWindow();
        var input = InputManager.GetInstance();

        state = GameState.Talk;
        pLevel = PierreLevel.L1;

        yield return base.AfterFadeIn();

        messageUI.Open();
        messageUI.SetMessage(StringMinigameMessage.PierreA_Serif0);

        yield return new WaitUntil(() => input.GetKeyPress(InputManager.Keys.South));

        tutorial.SetTitle(StringMinigameMessage.PierreA_Title);
        tutorial.SetText(StringMinigameMessage.PierreA_Tutorial);
        yield return tutorial.Open();
        yield return new WaitUntil(() => input.GetKeyPress(InputManager.Keys.South));
        yield return tutorial.Close();

        StartCoroutine(RandomMessageCoroutine());
        pierreA.StartGame();
        state = GameState.Main;
    }

    #region 機能呼び出し
    /// <summary>
    /// ボール生成
    /// </summary>
    /// <param name="farPosition"></param>
    /// <param name="ballType"></param>
    public void GenerateBall(float farPosition, PierreGameBall.BallType ballType)
    {
        var ball = GameObject.Instantiate(ball0Template);
        ball.transform.SetParent(roadParent.transform, false);
        ball.transform.localPosition = new Vector3(-500, 0, 0);
        var scr = ball.GetComponent<PierreGameBall>();
        scr.SetFarPosition(farPosition);
        scr.SetBallType(ballType);
    }

    /// <summary>
    /// ピエールにタッチする処理
    /// </summary>
    public void TatchPierre()
    {
        if (pLevel == PierreLevel.L3)
        {
            StartCoroutine(ClearCoroutine());
            return;
        }

        pLevel = pLevel switch
        {
            PierreLevel.L1 => PierreLevel.L2,
            _ => PierreLevel.L3,
        };
        StartCoroutine(WarpCoroutine());
    }

    /// <summary>
    /// 
    /// </summary>
    public void GameOver()
    {
        StartCoroutine(GameOverCoroutine());
    }
    #endregion

    #region ゲーム流れ処理
    /// <summary>
    /// ワープ処理コルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator WarpCoroutine()
    {
        state = GameState.Warp;

        // 左の帽子
        hutL.SetFarPosition(pierreA.GetFarPosition());
        hutL.gameObject.SetActive(true);
        var hutHeight = new DeltaFloat();
        hutHeight.Set(150f);
        hutHeight.MoveTo(0f, 0.3f, DeltaFloat.MoveType.LINE);

        pierreA.StopForWarp();

        while (hutHeight.IsActive())
        {
            hutHeight.Update(Time.deltaTime);
            var p = hutL.transform.localPosition;
            p.y = hutHeight.Get();
            hutL.transform.localPosition = p;

            yield return null;
        }

        yield return new WaitForSeconds(1f);

        // 右に帽子出現
        hutR.gameObject.SetActive(true);
        playerA.SetFarPosition(0f);
        playerA.transform.localPosition = new Vector3(hutR.transform.localPosition.x, 250f, 0);
        yield return new WaitForSeconds(1f);

        // 再開
        hutR.gameObject.SetActive(false);
        hutL.gameObject.SetActive(false);
        pierreA.RestartForWarp();
        state = GameState.Main;
    }

    /// <summary>
    /// ゲームクリア処理コルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator ClearCoroutine()
    {
        state = GameState.Talk;

        pierreA.StopForGameEnd();
        yield return new WaitForSeconds(1f);

        messageUI.Open();
        messageUI.SetMessage(StringMinigameMessage.PierreA_Win);

        yield return new WaitUntil(() => InputManager.GetInstance().GetKeyPress(InputManager.Keys.South));

        messageUI.Close();
        ManagerSceneScript.GetInstance().ExitGame();
    }

    /// <summary>
    /// ゲームオーバー処理コルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator GameOverCoroutine()
    {
        state = GameState.Talk;

        pierreA.StopForGameEnd();
        yield return new WaitForSeconds(1f);

        messageUI.Open();
        messageUI.SetMessage(StringMinigameMessage.PierreA_Lose);

        yield return new WaitUntil(() => InputManager.GetInstance().GetKeyPress(InputManager.Keys.South));

        messageUI.Close();
        ManagerSceneScript.GetInstance().ExitGame();
    }

    /// <summary>
    /// ランダムメッセージ表示コルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator RandomMessageCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);

            if (state != GameState.Main) { continue; }

            var idx = Util.RandomInt(0, randomMessage.Length);
            messageUI.SetMessage(randomMessage[idx]);
        }
    }
    #endregion

    #region 背景オブジェクトまわり
    /// <summary>
    /// 初期オブジェクト作成
    /// </summary>
    private void GenerateInitObjects()
    {
        const float LEFT_X = (Constant.SCREEN_WIDTH + PierreGameBGObject.OBJECT_WIDTH_MAX) / (-2f);
        // 木
        var treeX = Constant.SCREEN_WIDTH / 2f;
        while (treeX > LEFT_X)
        {
            treeX -= Util.RandomFloat(70f, 350f);
            GenerateTreeOne(treeX);
        }

        // 奥の石
        var backX = Constant.SCREEN_WIDTH / 2f;
        while (backX > LEFT_X)
        {
            backX -= Util.RandomFloat(450f, 900f);
            GenerateBackStoneOne(backX);
        }

        // 手前の石
        var frontX = Constant.SCREEN_WIDTH / 2f;
        while (frontX > LEFT_X)
        {
            frontX -= Util.RandomFloat(600f, 1000f);
            GenerateFrontStoneOne(frontX);
        }

        // 道の枠　右端だけ作ればすぐ作られる
        newWakuTop = GameObject.Instantiate(wakuTopTemplate);
        newWakuBottom = GameObject.Instantiate(wakuBottomTemplate);
        newWakuTop.transform.SetParent(wakuParent.transform, false);
        newWakuBottom.transform.SetParent(wakuParent.transform, false);
        newWakuTop.transform.localPosition = new Vector3(Constant.SCREEN_WIDTH / 2f, WAKU_Y, 0f);
        newWakuBottom.transform.localPosition = new Vector3(Constant.SCREEN_WIDTH / 2f, -WAKU_Y, 0f);

    }

    /// <summary>
    /// 木を常に生成
    /// </summary>
    /// <returns></returns>
    private IEnumerator GenerateTreeCoroutine()
    {
        while (true)
        {
            var wait = Util.RandomFloat(0.2f, 0.8f);
            yield return new WaitForSeconds(wait);

            GenerateTreeOne(OBJ_INIT_X);
        }
    }
    /// <summary>
    /// 木１本生成
    /// </summary>
    /// <param name="posX">X座標</param>
    /// <returns></returns>
    private GameObject GenerateTreeOne(float posX)
    {
        var obj = Util.RandomInt(0, 5) switch
        {
            int x when x < 4 => GameObject.Instantiate(tree1Template),
            _ => GameObject.Instantiate(tree2Template),
        };

        obj.transform.SetParent(backObjectParent.transform, false);
        obj.transform.localPosition = new Vector3(posX, Util.RandomFloat(36f, 40f), 0f);

        return obj;
    }
    /// <summary>
    /// 手前の石を常に生成
    /// </summary>
    /// <returns></returns>
    private IEnumerator GenerateFrontStoneCoroutine()
    {
        while (true)
        {
            var wait = Util.RandomFloat(1f, 4f);
            yield return new WaitForSeconds(wait);

            GenerateFrontStoneOne(OBJ_INIT_X);
        }
    }
    /// <summary>
    /// 手前の石１個生成
    /// </summary>
    /// <param name="posX">X座標</param>
    /// <returns></returns>
    private GameObject GenerateFrontStoneOne(float posX)
    {
        var obj = Util.RandomInt(0, 3) switch
        {
            int x when x < 2 => GameObject.Instantiate(stone1Template),
            _ => GameObject.Instantiate(stone2Template),
        };

        obj.transform.SetParent(frontObjectParent.transform, false);
        obj.transform.localPosition = new Vector3(posX, Util.RandomFloat(-15f, -15f), 0f);

        return obj;
    }
    /// <summary>
    /// 奥の石を常に生成
    /// </summary>
    /// <returns></returns>
    private IEnumerator GenerateBackStoneCoroutine()
    {
        while (true)
        {
            var wait = Util.RandomFloat(0.7f, 3f);
            yield return new WaitForSeconds(wait);

            GenerateBackStoneOne(OBJ_INIT_X);
        }
    }
    /// <summary>
    /// 奥の石１個生成
    /// </summary>
    /// <param name="posX">X座標</param>
    /// <returns></returns>
    private GameObject GenerateBackStoneOne(float posX)
    {
        var obj = GameObject.Instantiate(stone3Template);

        obj.transform.SetParent(backObjectParent.transform, false);
        obj.transform.localPosition = new Vector3(posX, Util.RandomFloat(-60f, -50f), 0f);

        return obj;
    }
    /// <summary>
    /// 道の枠を常に生成
    /// </summary>
    /// <returns></returns>
    private IEnumerator GenerateWakuCoroutine()
    {
        while (true)
        {
            yield return new WaitUntil(() => newWakuTop.transform.localPosition.x > -Constant.SCREEN_WIDTH / 2f);

            var t = GameObject.Instantiate(wakuTopTemplate);
            var b = GameObject.Instantiate(wakuBottomTemplate);
            t.transform.SetParent(wakuParent.transform, false);
            b.transform.SetParent(wakuParent.transform, false);
            t.transform.localPosition = new Vector3(newWakuTop.transform.localPosition.x - PierreGameBGObject.OBJECT_WIDTH_MAX, WAKU_Y, 0f);
            b.transform.localPosition = new Vector3(newWakuBottom.transform.localPosition.x - PierreGameBGObject.OBJECT_WIDTH_MAX, -WAKU_Y, 0f);
            t.gameObject.GetComponent<SpriteRenderer>().sprite = Util.RandomInt(0, 4) switch
            {
                0 => wakuTopSprite0,
                1 => wakuTopSprite1,
                2 => wakuTopSprite2,
                _ => wakuTopSprite3,
            };
            b.gameObject.GetComponent<SpriteRenderer>().sprite = Util.RandomInt(0, 4) switch
            {
                0 => wakuBottomSprite0,
                1 => wakuBottomSprite1,
                2 => wakuBottomSprite2,
                _ => wakuBottomSprite3,
            };

            newWakuTop = t;
            newWakuBottom = b;
        }
    }
    #endregion
}

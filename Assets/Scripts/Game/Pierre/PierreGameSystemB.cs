using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class PierreGameSystemB : GameSceneScriptBase
{
    #region 定数

    /// <summary>床オブジェクト生成位置</summary>
    private const float OBJ_INIT_X = (Constant.SCREEN_WIDTH + PierreGameBGObject.OBJECT_WIDTH_MAX) / (-2f);

    #endregion

    #region メンバー

    /// <summary>オブジェクト親</summary>
    public GameObject objectParent = null;
    /// <summary>地面テンプレ</summary>
    public GameObject ground_dummy = null;
    /// <summary>ボール0のテンプレート</summary>
    public GameObject ball_dummy = null;

    /// <summary>ピエールA</summary>
    public PierreGameBPlayer pierreA = null;
    /// <summary>ピエールB</summary>
    public PierreGameBEnemy pierreB = null;

    #endregion

    #region プライベート

    /// <summary>最新の地面</summary>
    private GameObject newBG = null;

    #endregion

    /// <summary>
    /// 初期化
    /// </summary>
    /// <returns></returns>
    override public IEnumerator Start()
    {
        yield return base.Start();

        GenerateInitObjects();

        StartCoroutine(GenerateGroundCoroutine());
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

        tutorial.SetTitle(StringMinigameMessage.PierreB_Title);
        tutorial.SetText(StringMinigameMessage.PierreB_Tutorial);
        yield return tutorial.Open();
        yield return new WaitUntil(() => input.GetKeyPress(InputManager.Keys.South));
        yield return tutorial.Close();

        //pierreA.StartGame();
    }

    #region 機能呼び出し
    /// <summary>
    /// ボール生成
    /// </summary>
    /// <param name="farPosition"></param>
    /// <param name="ballType"></param>
    public void GenerateBall(float farPosition, PierreGameBall.BallType ballType)
    {
        var ball = Instantiate(ball_dummy);
        ball.transform.SetParent(objectParent.transform, false);
        ball.transform.localPosition = new Vector3(-500, 0, 0);
        var scr = ball.GetComponent<PierreGameBall>();
        scr.SetFarPosition(farPosition);
        scr.SetBallType(ballType);
    }

    #endregion

    #region ゲーム流れ処理

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

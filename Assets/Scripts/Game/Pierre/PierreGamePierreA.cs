using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PierreGamePierreA : PierreGameRoadObject
{
    #region メンバー
    /// <summary>腕</summary>
    public SpriteRenderer armSprite = null;
    /// <summary>体</summary>
    public GameObject bodyObject = null;

    /// <summary>上下アニメーション</summary>
    public Animator jumpAnimator = null;
    /// <summary>足元のボール</summary>
    public SpriteRenderer ballSprite = null;

    /// <summary>手元のボール</summary>
    public SpriteRenderer ballOnArmSprite = null;

    /// <summary>腕基本画像</summary>
    public Sprite arm0 = null;
    /// <summary>腕上げる画像</summary>
    public Sprite arm1 = null;
    #endregion

    #region 変数
    /// <summary>上下移動コルーチン</summary>
    private IEnumerator randomMoveCoroutine = null;
    /// <summary>ボール生成コルーチン</summary>
    private IEnumerator generateBallCoroutine = null;
    #endregion

    /// <summary>
    /// 初期化
    /// </summary>
    override public void Start()
    {
        base.Start();

        AddRenderList(bodyObject.GetComponent<SpriteRenderer>());
        AddRenderList(ballSprite);

        SetFarPosition(0f);

        armSprite.sprite = arm0;
        ballOnArmSprite.gameObject.SetActive(false);
        randomMoveCoroutine = RandomMoveCoroutine();
        generateBallCoroutine = GenerateBallCoroutine();
    }

    /// <summary>
    /// 位置設定時に腕の描画順を+1
    /// </summary>
    /// <param name="_far"></param>
    public override void SetFarPosition(float _far)
    {
        base.SetFarPosition(_far);

        var order = CalcBaseSortingOrder();
        armSprite.sortingOrder = order + 1;
        ballOnArmSprite.sortingOrder = order + 2;
    }

    /// <summary>
    /// 上下ランダム移動
    /// </summary>
    /// <returns></returns>
    private IEnumerator RandomMoveCoroutine()
    {
        var far = new DeltaFloat();
        far.Set(0f);
        while (true)
        {
            var target = Util.RandomFloat(-ROAD_FAR_MAX, ROAD_FAR_MAX);
            var time = Mathf.Abs(target - GetFarPosition()) / ROAD_FAR_MAX;
            far.MoveTo(target, time, DeltaFloat.MoveType.LINE);
            while (far.IsActive())
            {
                yield return null;
                SetFarPosition(far.Get());
                far.Update(Time.deltaTime);
            }

            yield return new WaitForSeconds(Util.RandomFloat(0.1f, 1f));
        }
    }

    /// <summary>
    /// ボール生成
    /// </summary>
    /// <returns></returns>
    private IEnumerator GenerateBallCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Util.RandomFloat(1f, 2f));

            var ballType = system.GetPierreLevel() switch
            {
                PierreGameSystemA.PierreLevel.L1 => PierreGameBall.BallType.Normal,
                PierreGameSystemA.PierreLevel.L2 => PierreGameBall.BallType.Drift,
                _ => (PierreGameBall.BallType)Util.RandomInt(0, 3)
            };

            // 手元に持つ
            ballOnArmSprite.gameObject.SetActive(true);
            armSprite.sprite = arm1;
            ballOnArmSprite.color = PierreGameBall.CalcBallColor(ballType);

            yield return new WaitForSeconds(1f);

            // 投げる
            ballOnArmSprite.gameObject.SetActive(false);
            armSprite.sprite = arm0;

            system.GenerateBall(GetFarPosition(), ballType);
        }
    }

    /// <summary>
    /// ゲーム開始時動き出し
    /// </summary>
    public void StartGame()
    {
        StartCoroutine(generateBallCoroutine);
        StartCoroutine(randomMoveCoroutine);
    }

    /// <summary>
    /// ワープ処理のため停止
    /// </summary>
    public void StopForWarp()
    {
        var bodyAnim = bodyObject.GetComponent<Animator>();

        StopCoroutine(generateBallCoroutine);
        StopCoroutine(randomMoveCoroutine);

        bodyAnim.Play("stop");
        jumpAnimator.Play("stop");
    }

    /// <summary>
    /// ワープ処理の後再開
    /// </summary>
    public void RestartForWarp()
    {
        var bodyAnim = bodyObject.GetComponent<Animator>();

        StartCoroutine(generateBallCoroutine);
        StartCoroutine(randomMoveCoroutine);

        bodyAnim.Play("run");
        jumpAnimator.Play("run");
    }

    /// <summary>
    /// ゲーム終了時の停止
    /// </summary>
    public void StopForGameEnd()
    {
        StopCoroutine(generateBallCoroutine);
        StopCoroutine(randomMoveCoroutine);

        ballOnArmSprite.gameObject.SetActive(false);
        armSprite.sprite = arm0;
    }
}
